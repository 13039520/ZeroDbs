using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZeroDbs.DataAccess.SqlServer
{
    internal class DbSqlBuilder : ZeroDbs.Interfaces.IDbSqlBuilder
    {
        private Interfaces.IDb db = null;
        public Interfaces.IDb ZeroDb { get { return db; } }
         
        public DbSqlBuilder(Interfaces.IDb db)
        {
            this.db = db;
        }
        private string GetUniqueFieldName<T>(Interfaces.Common.DbDataTableInfo tableInfo)
        {
            var keys = tableInfo.Colunms.FindAll(o => o.IsPrimaryKey);
            if (keys != null && keys.Count == 1)
            {
                return keys[0].Name;
            }
            var key = tableInfo.Colunms.Find(o => o.IsIdentity);
            if (key != null)
            {
                return key.Name;
            }
            return "";
        }
        public string BuildSqlPage<T>(long page, long size, string where, string orderby, string[] returnFieldNames, string uniqueFieldName = "") where T : class, new()
        {
            var tableInfo = this.ZeroDb.GetDbDataTableInfo<T>();
            
            page = page < 1 ? 1 : page;
            size = size < 1 ? 1 : size;

            long startIndex = page * size - size + 1;
            long endIndex = page * size;
            StringBuilder sql = new StringBuilder();
            StringBuilder fieldStr = new StringBuilder();
            if (returnFieldNames == null)
            {
                returnFieldNames = tableInfo.Colunms.FindAll(o => o.MaxLength < 1000).Select(o => o.Name).ToArray();
            }
            for (int i = 0; i < returnFieldNames.Length; i++)
            {
                if (!string.IsNullOrEmpty(returnFieldNames[i]))
                {
                    fieldStr.AppendFormat("{0},", returnFieldNames[i]);
                }
            }
            if (fieldStr.Length > 0)
            {
                fieldStr.Remove(fieldStr.Length - 1, 1);
            }
            else
            {
                fieldStr.Append("*");
            }
            if (string.IsNullOrEmpty(orderby))
            {
                orderby = "(SELECT NULL)";
            }
            if (string.IsNullOrEmpty(uniqueFieldName))
            {
                uniqueFieldName = GetUniqueFieldName<T>(tableInfo);
            }

            if (!string.IsNullOrEmpty(uniqueFieldName))//具有唯一性字段
            {
                string resultFieldName = uniqueFieldName;
                sql.AppendFormat("SELECT {0} FROM {1}", fieldStr, GetTableName(tableInfo));
                //获取唯一性字段集合
                sql.AppendFormat(" WHERE {0} IN(SELECT {0} FROM(", resultFieldName);
                sql.AppendFormat("SELECT ROW_NUMBER() OVER (ORDER BY {0})AS Row,{1} FROM {2}", orderby, resultFieldName, GetTableName(tableInfo));
                if (!string.IsNullOrEmpty(where))
                {
                    sql.AppendFormat(" WHERE {0}", where);
                }
                sql.AppendFormat(")TT WHERE TT.Row BETWEEN {0} AND {1}) ORDER BY {2}", startIndex, endIndex, orderby);
            }
            else//没有唯一性字段
            {
                sql.AppendFormat("SELECT {0} FROM (", fieldStr);
                sql.AppendFormat("SELECT ROW_NUMBER() OVER (ORDER BY {0})AS Row,{1} FROM {2}", orderby, fieldStr, GetTableName(tableInfo));
                if (!string.IsNullOrEmpty(where))
                {
                    sql.AppendFormat(" WHERE {0}", where);
                }
                sql.Append(") TT");
                sql.AppendFormat(" WHERE TT.Row BETWEEN {0} AND {1}", startIndex, endIndex);
            }
            return sql.ToString();
        }
        public string BuildSqlPage<T>(long page, long size, string where, string orderby, int lengthThreshold, string uniqueFieldName = "") where T : class, new()
        {
            var tableInfo = this.ZeroDb.GetDbDataTableInfo<T>();
            var dv = Common.DbMapping.GetDbConfigDataViewInfo<T>();
            List<string> names = tableInfo.Colunms.FindAll(o => o.MaxLength < lengthThreshold).Select(o => o.Name).ToList();
            string[] returnFieldNames = names.ToArray();
            return BuildSqlPage<T>(page, size, where, orderby, returnFieldNames, uniqueFieldName);
        }

        public string BuildSqlInsert<T>(T sourceEntity, string[] skipFieldNames) where T : class, new()
        {
            List<T> li = new List<T>(1);
            li.Add(sourceEntity);
            List<string> reval = BuildSqlInsert(li, skipFieldNames);
            return reval[0];
        }
        public List<string> BuildSqlInsert<T>(List<T> sourceEntityList, string[] skipFieldNames) where T : class, new()
        {
            var tableInfo = ZeroDb.GetDbDataTableInfo<T>();
            if (tableInfo.IsView)
            {
                throw new Exception("不支持对视图做Insert操作");
            }
            if (skipFieldNames == null || skipFieldNames.Length < 1)
            {
                var cs = tableInfo.Colunms.FindAll(o => o.IsIdentity);
                if (cs != null && cs.Count > 0)
                {
                    skipFieldNames = cs.Select(o => o.Name).ToArray();
                }
            }
            else
            {
                skipFieldNames = skipFieldNames.Distinct().ToArray();
            }
            if (sourceEntityList == null || sourceEntityList.Count < 1) { throw new Exception("sourceEntityList为null或包含元素为0"); }
            if (tableInfo == null) { throw new Exception("tableInfo不能为空"); }
            if (skipFieldNames == null) { skipFieldNames = new string[] { }; }
            if (sourceEntityList.Contains(null)) { throw new Exception("sourceEntityList包含null元素"); }

            System.Reflection.PropertyInfo[] pi = typeof(T).GetProperties();
            if (pi.Length < 1) { throw new Exception("泛型参数缺少公开的属性"); }
            Dictionary<string, System.Reflection.PropertyInfo> dic = new Dictionary<string, System.Reflection.PropertyInfo>(pi.Length);
            string keep = "";
            foreach (System.Reflection.PropertyInfo p in pi)
            {
                string name = p.Name.ToLower();
                bool inSkipFieldNames = false;
                for (var i = 0; i < skipFieldNames.Length; i++)
                {
                    if (string.Equals(skipFieldNames[i], name, StringComparison.OrdinalIgnoreCase))
                    {
                        inSkipFieldNames = true;
                        break;
                    }
                }
                if (!inSkipFieldNames)
                {
                    dic.Add(name, p);
                    keep += name + ",";
                }
            }
            string[] keepFieldNames = keep.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            if (keepFieldNames.Length < 1)
            {
                throw new Exception("字段全部被跳过了");
            }
            List<string> sqlList = new List<string>(sourceEntityList.Count);
            foreach (T entity in sourceEntityList)
            {
                System.Text.StringBuilder columnNames = new System.Text.StringBuilder();
                System.Text.StringBuilder columnValues = new System.Text.StringBuilder();
                foreach (string s in keepFieldNames)
                {
                    string columnName = ("" + s).ToLower().Trim();
                    if (columnName.Length > 0 && dic.ContainsKey(columnName))
                    {
                        columnNames.AppendFormat("{0},", dic[columnName].Name);
                        columnValues.AppendFormat("{0},", Common.ValueConvert.SqlValueStrByValue(dic[columnName].GetValue(entity, null)));
                    }
                }
                if (columnNames.Length < 1)
                {
                    throw new Exception("生成的Insert命令不可用");
                }
                columnNames.Remove(columnNames.Length - 1, 1);
                columnValues.Remove(columnValues.Length - 1, 1);
                sqlList.Add(string.Format("INSERT INTO {0}({1}) VALUES({2})", GetTableName(tableInfo), columnNames, columnValues));
            }
            return sqlList;
        }
        public string BuildSqlInsert<T>(System.Collections.Specialized.NameValueCollection nvc) where T : class, new()
        {
            return BuildSqlInsert<T>(nvc, "");
        }
        public string BuildSqlInsert<T>(System.Collections.Specialized.NameValueCollection nvc, string appendWhere) where T : class, new()
        {
            List<System.Collections.Specialized.NameValueCollection> li = new List<System.Collections.Specialized.NameValueCollection>(1);
            li.Add(nvc);
            List<string> reval = BuildSqlInsert<T>(li, appendWhere);
            return reval[0];
        }
        public List<string> BuildSqlInsert<T>(List<System.Collections.Specialized.NameValueCollection> nvcList) where T : class, new()
        {
            return BuildSqlInsert<T>(nvcList, "");
        }
        public List<string> BuildSqlInsert<T>(List<System.Collections.Specialized.NameValueCollection> nvcList, string appendWhere) where T : class, new()
        {
            if (nvcList == null || nvcList.Count < 1) { throw new Exception("nvcList是null或长度为0"); }
            if (nvcList.Count > 5000)
            {
                throw new Exception("nvcList的长度不宜超过5000项");
            }
            if (nvcList.Contains(null))
            {
                throw new Exception("nvcList存在null的项");
            }
            var tableInfo = ZeroDb.GetDbDataTableInfo<T>();
            if (tableInfo.IsView)
            {
                throw new Exception("不支持对视图做Insert操作");
            }

            var identityKeys = tableInfo.Colunms.FindAll(o=>o.IsIdentity);
            bool isAppendWhere = false;
            if (!string.IsNullOrEmpty(appendWhere))
            {
                appendWhere = appendWhere.Trim();
                if (appendWhere.StartsWith("where", StringComparison.OrdinalIgnoreCase))
                {
                    appendWhere = appendWhere.Remove(0, 5);
                }
                appendWhere = appendWhere.Trim();
                isAppendWhere = !string.IsNullOrEmpty(appendWhere);
            }

            List<string> result = new List<string>();
            T t = (T)Activator.CreateInstance(typeof(T));
            System.Reflection.PropertyInfo[] properties = t.GetType().GetProperties();
            List<System.Reflection.PropertyInfo> propertyInfoList = new List<System.Reflection.PropertyInfo>(properties.Length);
            foreach (System.Reflection.PropertyInfo p in properties)
            {
                propertyInfoList.Add(p);
            }
            int nvcListCount = nvcList.Count;
            for (int i = 0; i < nvcListCount; i++)
            {

                StringBuilder sqlInsertFields = new StringBuilder();
                StringBuilder sqlInsertValues = new StringBuilder();
                foreach (string key in nvcList[i].Keys)
                {
                    System.Reflection.PropertyInfo p = propertyInfoList.Find(delegate (System.Reflection.PropertyInfo temp) { return string.Equals(key, temp.Name, StringComparison.OrdinalIgnoreCase); });
                    if (p != null)
                    {
                        if (identityKeys.Find(delegate (ZeroDbs.Interfaces.Common.DbDataColumnInfo temp) { return string.Equals(temp.Name, p.Name, StringComparison.OrdinalIgnoreCase); }) == null)
                        {
                            sqlInsertFields.AppendFormat("{0},", p.Name);
                            object TargetValue = Common.ValueConvert.StrToTargetType(nvcList[i][key], p.PropertyType);
                            string ValueString = Common.ValueConvert.SqlValueStrByValue(TargetValue);
                            sqlInsertValues.AppendFormat("{0},", ValueString);
                        }
                    }
                }
                if (sqlInsertFields.Length < 1 || sqlInsertValues.Length < 1) { continue; }

                sqlInsertFields.Remove(sqlInsertFields.Length - 1, 1);
                sqlInsertValues.Remove(sqlInsertValues.Length - 1, 1);

                string sql = string.Format("INSERT INTO {0}({1}) VALUES({2}){3}", GetTableName(tableInfo), sqlInsertFields, sqlInsertValues, isAppendWhere ? " WHERE " + appendWhere : "");
                if (!result.Contains(sql))
                {
                    result.Add(sql);
                }
            }
            return result;
        }

        public string BuildSqlUpdate<T>(T sourceEntity, string[] setFieldNames, string[] whereFieldNames) where T : class, new()
        {
            List<T> li = new List<T>(1);
            li.Add(sourceEntity);
            List<string> reval = BuildSqlUpdate(li, setFieldNames,whereFieldNames);
            return reval[0];
        }
        public List<string> BuildSqlUpdate<T>(List<T> sourceEntityList, string[] setFieldNames, string[] whereFieldNames) where T : class, new()
        {
            var tableInfo = ZeroDb.GetDbDataTableInfo<T>();
            if (tableInfo.IsView)
            {
                throw new Exception("不支持对视图做Update操作");
            }
            string[] fieldArray = tableInfo.Colunms.Select(o => o.Name).ToArray();
            if (setFieldNames != null && setFieldNames.Length > 0)
            {
                int i = 0;
                List<string> temp = new List<string>();
                while(i< setFieldNames.Length)
                {
                    int j = 0;
                    while(j< fieldArray.Length)
                    {
                        if(string.Equals(setFieldNames[i], fieldArray[j], StringComparison.OrdinalIgnoreCase))
                        {
                            temp.Add(fieldArray[j]);
                            break;
                        }
                        j++;
                    }
                    i++;
                }
                setFieldNames = temp.Distinct().ToArray();
                if (setFieldNames.Length < 1)
                {
                    setFieldNames = fieldArray;
                }
            }
            else
            {
                setFieldNames = fieldArray;
            }
            if (whereFieldNames != null && whereFieldNames.Length > 0)
            {
                int i = 0;
                List<string> temp = new List<string>();
                while (i < whereFieldNames.Length)
                {
                    int j = 0;
                    while (j < fieldArray.Length)
                    {
                        if (string.Equals(whereFieldNames[i], fieldArray[j], StringComparison.OrdinalIgnoreCase))
                        {
                            temp.Add(fieldArray[j]);
                            break;
                        }
                        j++;
                    }
                    i++;
                }
                whereFieldNames = temp.Distinct().ToArray();
                if (whereFieldNames.Length < 1)
                {
                    whereFieldNames = fieldArray;
                }
            }
            else
            {
                whereFieldNames = fieldArray;
            }
            if (sourceEntityList == null || sourceEntityList.Count < 1) { throw new Exception("sourceEntityList为null或包含元素为0"); }
            if (sourceEntityList.Contains(null)) { throw new Exception("sourceEntityList包含null元素"); }
            if (setFieldNames == null || setFieldNames.Length < 1) { throw new Exception("setFieldNames为空或长度为0"); }
            if (whereFieldNames == null || whereFieldNames.Length < 1) { throw new Exception("whereFieldNames为空或长度为0"); }
            for (var i = 0; i < setFieldNames.Length; i++)
            {
                setFieldNames[i] = setFieldNames[i].Trim();
                if (string.IsNullOrEmpty(setFieldNames[i]))
                {
                    throw new Exception("setFieldNames存在空项");
                }
            }
            for (var i = 0; i < whereFieldNames.Length; i++)
            {
                whereFieldNames[i] = whereFieldNames[i].Trim();
                if (string.IsNullOrEmpty(whereFieldNames[i]))
                {
                    throw new Exception("whereFieldNames存在空项");
                }
            }
            for (var i = 0; i < whereFieldNames.Length; i++)
            {
                for (var j = 0; j < setFieldNames.Length; j++)
                {
                    if (string.Equals(whereFieldNames[i], setFieldNames[j], StringComparison.OrdinalIgnoreCase))
                    {
                        setFieldNames[j] = "";
                    }
                }
            }
            string SetFieldNameStr = "";
            for (var i = 0; i < setFieldNames.Length; i++)
            {
                SetFieldNameStr += setFieldNames[i] + ",";
            }
            setFieldNames = SetFieldNameStr.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            if (setFieldNames.Length < 1)
            {
                throw new Exception("setFieldNames被whereFieldNames全部排除");
            }

            System.Reflection.PropertyInfo[] pi = typeof(T).GetProperties();
            if (pi.Length < 1) { throw new Exception("泛型参数缺少公开的属性"); }
            Dictionary<string, System.Reflection.PropertyInfo> dic = new Dictionary<string, System.Reflection.PropertyInfo>(pi.Length);
            foreach (System.Reflection.PropertyInfo p in pi)
            {
                dic.Add(p.Name.ToLower(), p);
            }
            List<string> sqlList = new List<string>(sourceEntityList.Count);
            foreach (T entity in sourceEntityList)
            {
                System.Text.StringBuilder setColumnValue = new System.Text.StringBuilder();
                foreach (string s in setFieldNames)
                {
                    string columnName = ("" + s).ToLower().Trim();
                    if (columnName.Length > 0 && dic.ContainsKey(columnName))
                    {
                        setColumnValue.AppendFormat(
                            "{0}={1},",
                            dic[columnName].Name,
                            Common.ValueConvert.SqlValueStrByValue(dic[columnName].GetValue(entity, null)));
                    }
                }
                if (setColumnValue.Length < 1)
                {
                    throw new Exception("setFieldNames最后留存的字段与实体属性对应不上");
                }
                setColumnValue.Remove(setColumnValue.Length - 1, 1);
                System.Text.StringBuilder updateWhere = new System.Text.StringBuilder();
                foreach (string s in whereFieldNames)
                {
                    string ColumnName = ("" + s).ToLower().Trim();
                    if (ColumnName.Length > 0 && dic.ContainsKey(ColumnName))
                    {
                        updateWhere.AppendFormat(
                            "{0}={1} AND ",
                            dic[ColumnName].Name,
                            Common.ValueConvert.SqlValueStrByValue(dic[ColumnName].GetValue(entity, null)));
                    }
                }
                if (updateWhere.Length < 1)
                {
                    throw new Exception("whereFieldNames没有一个字段与实体属性对应得上");
                }
                updateWhere.Remove(updateWhere.Length - 5, 5);

                sqlList.Add(string.Format("UPDATE {0} SET {1} WHERE {2}", GetTableName(tableInfo), setColumnValue, updateWhere));
            }
            return sqlList;
        }
        public string BuildSqlUpdate<T>(System.Collections.Specialized.NameValueCollection nvc) where T : class, new()
        {
            return BuildSqlUpdate<T>(nvc,"");
        }
        public string BuildSqlUpdate<T>(System.Collections.Specialized.NameValueCollection nvc, string appendWhere) where T : class, new()
        {
            var li = new List<System.Collections.Specialized.NameValueCollection>();
            li.Add(nvc);
            List<string> reval = BuildSqlUpdate<T>(li, appendWhere);
            return reval[0];
        }
        public List<string> BuildSqlUpdate<T>(List<System.Collections.Specialized.NameValueCollection> nvcList) where T : class, new()
        {
            return BuildSqlUpdate<T>(nvcList, "");
        }
        public List<string> BuildSqlUpdate<T>(List<System.Collections.Specialized.NameValueCollection> nvcList, string appendWhere) where T : class, new()
        {
            if (nvcList == null || nvcList.Count < 1) { throw new Exception("nvcList是null或长度为0"); }
            if (nvcList.Count > 5000)
            {
                throw new Exception("nvcList的长度不宜超过5000项");
            }
            if (nvcList.Contains(null))
            {
                throw new Exception("nvcList存在null的项");
            }
            var tableInfo = ZeroDb.GetDbDataTableInfo<T>();
            if (tableInfo.IsView)
            {
                throw new Exception("不支持对视图做Update操作");
            }
            var primaryKeys = tableInfo.Colunms.FindAll(o=>o.IsPrimaryKey);
            if (primaryKeys == null || primaryKeys.Count < 1)
            {
                var uniqueFieldName = GetUniqueFieldName<T>(tableInfo);
                if (string.IsNullOrEmpty(uniqueFieldName))
                {
                    throw new Exception("目标tableInfo缺少主键和唯一标识列");
                }
                var col = tableInfo.Colunms.Find(o => string.Equals(o.Name, uniqueFieldName, StringComparison.OrdinalIgnoreCase));
                if (col == null)
                {
                    throw new Exception("目标tableInfo配置的唯一列" + uniqueFieldName + "不存在");
                }
                primaryKeys.Clear();
                primaryKeys.Add(col);
            }

            bool isAppendWhere = false;
            if (!string.IsNullOrEmpty(appendWhere))
            {
                appendWhere = appendWhere.Trim();
                if (appendWhere.StartsWith("where", StringComparison.OrdinalIgnoreCase))
                {
                    appendWhere = appendWhere.Remove(0, 5);
                }
                appendWhere = appendWhere.Trim();
                isAppendWhere = !string.IsNullOrEmpty(appendWhere);
            }

            List<string> reval = new List<string>();
            T result = (T)Activator.CreateInstance(typeof(T));
            System.Reflection.PropertyInfo[] properties = result.GetType().GetProperties();
            List<System.Reflection.PropertyInfo> propertyInfoList = new List<System.Reflection.PropertyInfo>(properties.Length);
            foreach (System.Reflection.PropertyInfo p in properties)
            {
                propertyInfoList.Add(p);
            }
            foreach (var c in primaryKeys)
            {
                if (propertyInfoList.Find(delegate (System.Reflection.PropertyInfo t) {
                    return string.Equals(t.Name, c.Name, StringComparison.OrdinalIgnoreCase);
                }) == null)
                {
                    throw new Exception("实体缺少对字段" + c.Name + "的映射");
                }
            }
            int nvcListCount = nvcList.Count;
            for (int i = 0; i < nvcListCount; i++)
            {
                bool nvcContainsKeyKeys = true;
                StringBuilder sqlWhere = new StringBuilder();
                foreach (var c in primaryKeys)
                {
                    if (null == nvcList[i][c.Name])
                    {
                        nvcContainsKeyKeys = false;
                        continue;
                    }
                    else
                    {
                        System.Reflection.PropertyInfo p = propertyInfoList.Find(delegate (System.Reflection.PropertyInfo t)
                        {
                            return string.Equals(t.Name, c.Name, StringComparison.OrdinalIgnoreCase);
                        });
                        object targetValue = Common.ValueConvert.StrToTargetType(nvcList[i][c.Name], p.PropertyType);
                        string valueString = Common.ValueConvert.SqlValueStrByValue(targetValue);
                        sqlWhere.AppendFormat("{0}={1} AND ", c.Name, valueString);
                        nvcList[i].Remove(c.Name);
                    }
                }
                if (!nvcContainsKeyKeys) { throw new Exception("未传入主键"); }
                if (nvcList[i].Count < 1) { throw new Exception("未传入任何键值"); }

                if (isAppendWhere)
                {
                    sqlWhere.Append(appendWhere);
                }
                else
                {
                    sqlWhere.Remove(sqlWhere.Length - 5, 5);
                }

                StringBuilder sqlSet = new StringBuilder();
                foreach (string key in nvcList[i].Keys)
                {
                    foreach (System.Reflection.PropertyInfo p in properties)
                    {
                        if (string.Equals(p.Name, key, StringComparison.OrdinalIgnoreCase))
                        {
                            if (tableInfo.Colunms.Find(o=> string.Equals(o.Name, p.Name, StringComparison.OrdinalIgnoreCase)) != null)
                            {
                                object targetValue = Common.ValueConvert.StrToTargetType(nvcList[i][key], p.PropertyType);
                                string valueString = Common.ValueConvert.SqlValueStrByValue(targetValue);
                                sqlSet.AppendFormat("{0}={1},", p.Name, valueString);
                            }
                        }
                    }
                }
                if (sqlSet.Length < 1) { continue; }
                sqlSet.Remove(sqlSet.Length - 1, 1);
                string sql = string.Format("UPDATE {0} SET {1} WHERE {2}", GetTableName(tableInfo), sqlSet, sqlWhere);
                if (!reval.Contains(sql))
                {
                    reval.Add(sql);
                }
            }
            return reval;
        }

        public string BuildSqlDelete<T>(string sqlWhere) where T : class, new()
        {
            if (string.IsNullOrEmpty(sqlWhere))
            {
                sqlWhere = "1>0";
            }
            sqlWhere = sqlWhere.Trim();
            if (sqlWhere.StartsWith("WHERE ", StringComparison.OrdinalIgnoreCase))
            {
                sqlWhere = sqlWhere.Remove(0, 5);
            }
            var tableInfo = ZeroDb.GetDbDataTableInfo<T>();
            if (tableInfo.IsView)
            {
                throw new Exception("不支持对视图做Delete操作");
            }
            return string.Format("DELETE {0} WHERE {1}", tableInfo.Name, sqlWhere);
        }
        public string BuildSqlDelete<T>(T sourceEntity, string[] useFiled) where T : class, new()
        {
            List<T> li = new List<T>(1);
            li.Add(sourceEntity);
            List<string> reval = BuildSqlDelete(li, useFiled);
            return reval[0];
        }
        public List<string> BuildSqlDelete<T>(List<T> sourceEntityList, string[] useFiled) where T : class, new()
        {
            if (sourceEntityList == null || sourceEntityList.Count < 1) { throw new Exception("sourceEntityList为null或包含元素为0"); }
            
            if (sourceEntityList.Contains(null)) { throw new Exception("sourceEntityList包含null元素"); }
            
            var tableInfo = ZeroDb.GetDbDataTableInfo<T>();
            if (tableInfo.IsView)
            {
                throw new Exception("不支持对视图做Delete操作");
            }

            bool useFiledFlag = (useFiled != null && useFiled.Length > 0);
            if (useFiledFlag)
            {
                useFiled = useFiled.Distinct().ToArray();
            }
            else
            {
                var cs = tableInfo.Colunms.FindAll(o => o.IsPrimaryKey);
                if (cs == null || cs.Count < 1)
                {
                    cs= tableInfo.Colunms.FindAll(o => o.IsIdentity);
                }
                if (cs == null || cs.Count < 1)
                {
                    cs = tableInfo.Colunms;
                }
                useFiled = cs.Select(o => o.Name).ToArray();
            }
            useFiledFlag = (useFiled != null && useFiled.Length > 0);

            System.Reflection.PropertyInfo[] pi = typeof(T).GetProperties();
            if (pi.Length < 1) { throw new Exception("泛型参数缺少公开的属性"); }
            Dictionary<string, System.Reflection.PropertyInfo> dic = new Dictionary<string, System.Reflection.PropertyInfo>(pi.Length);
            foreach (System.Reflection.PropertyInfo p in pi)
            {
                dic.Add(p.Name.ToLower(), p);
            }

            List<string> reval = new List<string>(sourceEntityList.Count);
            foreach (T entity in sourceEntityList)
            {
                System.Text.StringBuilder where = new System.Text.StringBuilder();
                if (useFiledFlag)
                {
                    foreach (string s in useFiled)
                    {
                        string fieldName = s.ToLower();
                        if (dic.ContainsKey(fieldName))
                        {
                            where.AppendFormat("{0}={1} AND ", dic[fieldName].Name, Common.ValueConvert.SqlValueStrByValue(dic[fieldName].GetValue(entity, null)));
                        }
                    }
                }
                if (where.Length < 1)
                {
                    foreach (string fieldName in dic.Keys)
                    {
                        if (dic.ContainsKey(fieldName))
                        {
                            where.AppendFormat("{0}={1} AND ", dic[fieldName].Name, Common.ValueConvert.SqlValueStrByValue(dic[fieldName].GetValue(entity, null)));
                        }
                    }
                }
                if (where.Length < 1)
                {
                    throw new Exception("生成的where条件不可用");
                }
                where.Remove(where.Length - 5, 5);
                reval.Add(string.Format("DELETE {0} WHERE {1}", GetTableName(tableInfo), where));
            }
            return reval;
        }
        public string BuildSqlDelete<T>(System.Collections.Specialized.NameValueCollection nvc) where T : class, new()
        {
            return BuildSqlDelete<T>(nvc, "");
        }
        public string BuildSqlDelete<T>(System.Collections.Specialized.NameValueCollection nvc, string appendWhere) where T : class, new()
        {
            var li = new List<System.Collections.Specialized.NameValueCollection>(1);
            var reval = BuildSqlDelete<T>(li, appendWhere);
            return reval[0];
        }
        public List<string> BuildSqlDelete<T>(List<System.Collections.Specialized.NameValueCollection> nvcList) where T : class, new()
        {
            return BuildSqlDelete<T>(nvcList, "");
        }
        public List<string> BuildSqlDelete<T>(List<System.Collections.Specialized.NameValueCollection> nvcList, string appendWhere) where T : class, new()
        {
            if (nvcList == null || nvcList.Count < 1) { throw new Exception("nvcList为null或包含元素为0"); }

            if (nvcList.Contains(null)) { throw new Exception("nvcList包含null元素"); }
            if (nvcList.Count > 5000)
            {
                throw new Exception("nvcList的长度不宜超过5000项");
            }
            var tableInfo = ZeroDb.GetDbDataTableInfo<T>();
            if (tableInfo.IsView)
            {
                throw new Exception("不支持对视图做Delete操作");
            }
            var primaryKeys = tableInfo.Colunms.FindAll(o=>o.IsPrimaryKey);
            if (primaryKeys == null || primaryKeys.Count < 1)
            {
                var uniqueFieldName = GetUniqueFieldName<T>(tableInfo);
                if (string.IsNullOrEmpty(uniqueFieldName))
                {
                    throw new Exception("目标tableInfo缺少主键和唯一标识列");
                }
                var col = tableInfo.Colunms.Find(o => string.Equals(o.Name, uniqueFieldName, StringComparison.OrdinalIgnoreCase));
                if (col == null)
                {
                    throw new Exception("目标tableInfo配置的唯一列" + uniqueFieldName + "不存在");
                }
                primaryKeys.Clear();
                primaryKeys.Add(col);
            }

            bool isAppendWhere = false;
            if (!string.IsNullOrEmpty(appendWhere))
            {
                appendWhere = appendWhere.Trim();
                if (appendWhere.StartsWith("where", StringComparison.OrdinalIgnoreCase))
                {
                    appendWhere = appendWhere.Remove(0, 5);
                }
                appendWhere = appendWhere.Trim();
                isAppendWhere = !string.IsNullOrEmpty(appendWhere);
            }

            List<string> reval = new List<string>();
            T result = (T)Activator.CreateInstance(typeof(T));
            System.Reflection.PropertyInfo[] properties = result.GetType().GetProperties();
            List<System.Reflection.PropertyInfo> propertyInfoList = new List<System.Reflection.PropertyInfo>(properties.Length);
            foreach (System.Reflection.PropertyInfo p in properties)
            {
                propertyInfoList.Add(p);
            }
            foreach (var c in primaryKeys)
            {
                if (propertyInfoList.Find(delegate (System.Reflection.PropertyInfo t) {
                    return string.Equals(t.Name, c.Name, StringComparison.OrdinalIgnoreCase);
                }) == null)
                {
                    throw new Exception("实体缺少对字段" + c.Name + "的映射");
                }
            }
            int nvcListCount = nvcList.Count;
            if (primaryKeys.Count > 1)
            {
                for (int i = 0; i < nvcListCount; i++)
                {
                    bool nvcContainsKeyKeys = true;
                    StringBuilder sqlWhere = new StringBuilder();
                    foreach (var c in primaryKeys)
                    {
                        if (null == nvcList[i][c.Name])
                        {
                            nvcContainsKeyKeys = false;
                            continue;
                        }
                        else
                        {
                            System.Reflection.PropertyInfo p = propertyInfoList.Find(o=> string.Equals(o.Name, c.Name, StringComparison.OrdinalIgnoreCase));
                            object TargetValue = Common.ValueConvert.StrToTargetType(nvcList[i][c.Name], p.PropertyType);
                            string ValueString = Common.ValueConvert.SqlValueStrByValue(TargetValue);
                            sqlWhere.AppendFormat("{0}={1} AND ", c.Name, ValueString);
                            nvcList[i].Remove(c.Name);
                        }
                    }
                    if (!nvcContainsKeyKeys) { continue; }
                    if (isAppendWhere)
                    {
                        sqlWhere.Append(appendWhere);
                    }
                    else
                    {
                        sqlWhere.Remove(sqlWhere.Length - 5, 5);
                    }

                    string sql = string.Format("DELETE {0} WHERE {1}", GetTableName(tableInfo), sqlWhere);
                    if (!reval.Contains(sql))
                    {
                        reval.Add(sql);
                    }
                }
            }
            else
            {
                string primaryKeyName = primaryKeys[0].Name;
                System.Reflection.PropertyInfo p = propertyInfoList.Find(o=> string.Equals(o.Name, primaryKeyName, StringComparison.OrdinalIgnoreCase));
                Type primaryKeyType = p.PropertyType;
                List<string> primaryKeyList = new List<string>();
                int groupIndex = 0;
                int groupMax = 100;
                StringBuilder keys = new StringBuilder();
                for (int i = 0; i < nvcListCount; i++)
                {
                    object targetValue = Common.ValueConvert.StrToTargetType(nvcList[i][primaryKeyName], p.PropertyType);
                    string valueString = Common.ValueConvert.SqlValueStrByValue(targetValue);
                    if (primaryKeyList.Contains(valueString))
                    {
                        continue;
                    }
                    primaryKeyList.Add(valueString);
                    if (groupIndex < groupMax)
                    {
                        keys.AppendFormat("{0},", valueString);
                        groupIndex++;
                        continue;
                    }
                    else
                    {
                        keys.Remove(keys.Length - 1, 1);
                        reval.Add(string.Format("DELETE {0} WHERE {1} IN({2}){3}", GetTableName(tableInfo), primaryKeyName, keys, isAppendWhere ? " WHERE " + appendWhere : ""));
                        keys.Clear();
                        keys.AppendFormat("{0},", valueString);
                        groupIndex = 1;
                    }
                }
                if (keys.Length > 0)
                {
                    keys.Remove(keys.Length - 1, 1);
                    reval.Add(string.Format("DELETE {0} WHERE {1} IN({2}){3}", GetTableName(tableInfo), primaryKeyName, keys, isAppendWhere ? " WHERE " + appendWhere : ""));
                }
            }
            return reval;
        }

        public string BuildSqlSelect<T>(T sourceEntity, string[] whereField, string orderby) where T : class, new()
        {
            var tableInfo = ZeroDb.GetDbDataTableInfo<T>();
            if (whereField == null || whereField.Length < 1)
            {
                var temp = tableInfo.Colunms.FindAll(o => o.IsPrimaryKey);
                if (temp == null || temp.Count < 1)
                {
                    temp = tableInfo.Colunms.FindAll(o => o.IsIdentity);
                }
                if (temp == null || temp.Count < 1)
                {
                    temp = tableInfo.Colunms;
                }
                whereField = temp.Select(o => o.Name).ToArray();
            }
            else
            {
                whereField = whereField.Distinct().ToArray();
            }

            System.Reflection.PropertyInfo[] pi = sourceEntity.GetType().GetProperties();
            Dictionary<string, System.Reflection.PropertyInfo> dic = new Dictionary<string, System.Reflection.PropertyInfo>(pi.Length);
            StringBuilder field = new StringBuilder();
            foreach (System.Reflection.PropertyInfo p in pi)
            {
                field.AppendFormat("{0},", p.Name);
                dic.Add(p.Name.ToLower(), p);
            }
            if (field.Length < 1)
            {
                throw new Exception("生成的返回字段无效");
            }
            field.Remove(field.Length - 1, 1);
            StringBuilder where = new StringBuilder();
            if (whereField != null && whereField.Length > 0)
            {
                int i = 0;
                while (i < whereField.Length)
                {
                    string key = whereField[i].ToLower();
                    if (dic.ContainsKey(key))
                    {
                        string Name = dic[key].Name;
                        where.AppendFormat("{0}={1} AND ", dic[key].Name, Common.ValueConvert.SqlValueStrByValue(dic[key].GetValue(sourceEntity, null)));
                    }
                    i++;
                }
                if (where.Length > 0)
                {
                    where.Remove(where.Length - 5, 5);
                }
                else
                {
                    where.Append("1>0");
                }
            }
            else
            {
                where.Append("1>0");
            }
            if (string.IsNullOrEmpty(orderby))
            {
                return string.Format("SELECT {0} FROM {1} WHERE {2}", field, GetTableName(tableInfo), where);
            }
            else
            {
                return string.Format("SELECT {0} FROM {1} WHERE {2} ORDER BY {3}", field, GetTableName(tableInfo), where, orderby);
            }
        }
        public string BuildSqlSelect<T>(string where, string orderby) where T : class, new()
        {
            return BuildSqlSelect<T>(where, orderby, new string[] { });
        }
        public string BuildSqlSelect<T>(string where, string orderby, string[] returnFieldNames) where T : class, new()
        {
            var tableInfo = ZeroDb.GetDbDataTableInfo<T>();
            string[] fieldArray = tableInfo.Colunms.Select(o=>o.Name).ToArray();
            if (returnFieldNames != null && returnFieldNames.Length > 0)
            {
                int i = 0;
                List<string> temp = new List<string>();
                while (i < returnFieldNames.Length)
                {
                    int j = 0;
                    while (j < fieldArray.Length)
                    {
                        if (string.Equals(returnFieldNames[i], fieldArray[j], StringComparison.OrdinalIgnoreCase))
                        {
                            temp.Add(fieldArray[j]);
                            break;
                        }
                        j++;
                    }
                    i++;
                }
                returnFieldNames = temp.Distinct().ToArray();
                if (returnFieldNames.Length > 0)
                {
                    fieldArray = returnFieldNames;
                }
            }
            StringBuilder field = new StringBuilder();
            foreach (string s in fieldArray)
            {
                field.AppendFormat("{0},", s);
            }
            if (field.Length < 1)
            {
                throw new Exception("生成的返回字段无效");
            }
            field.Remove(field.Length - 1, 1);
            if (string.IsNullOrEmpty(where))
            {
                where = "1>0";
            }
            if (string.IsNullOrEmpty(orderby))
            {
                return string.Format("SELECT {0} FROM {1} WHERE {2}", field, GetTableName(tableInfo), where);
            }
            else
            {
                return string.Format("SELECT {0} FROM {1} WHERE {2} ORDER BY {3}", field, GetTableName(tableInfo), where, orderby);
            }
        }
        public string BuildSqlSelect<T>(string where, string orderby, int top) where T : class, new()
        {
            return BuildSqlSelect<T>(where, orderby, top, new string[] { });
        }
        public string BuildSqlSelect<T>(string where, string orderby, int top, string[] returnFieldNames) where T : class, new()
        {
            var tableInfo = ZeroDb.GetDbDataTableInfo<T>();
            string[] fieldArray = tableInfo.Colunms.Select(o => o.Name).ToArray();
            if (returnFieldNames != null && returnFieldNames.Length > 0)
            {
                int i = 0;
                List<string> temp = new List<string>();
                while (i < returnFieldNames.Length)
                {
                    int j = 0;
                    while (j < fieldArray.Length)
                    {
                        if (string.Equals(returnFieldNames[i], fieldArray[j], StringComparison.OrdinalIgnoreCase))
                        {
                            temp.Add(fieldArray[j]);
                            break;
                        }
                        j++;
                    }
                    i++;
                }
                returnFieldNames = temp.Distinct().ToArray();
                if (returnFieldNames.Length > 0)
                {
                    fieldArray = returnFieldNames;
                }
            }
            StringBuilder field = new StringBuilder();
            foreach (string s in fieldArray)
            {
                field.AppendFormat("{0},", s);
            }
            if (field.Length < 1)
            {
                throw new Exception("生成的返回字段无效");
            }
            field.Remove(field.Length - 1, 1);
            if (string.IsNullOrEmpty(where))
            {
                where = "1>0";
            }
            if (top < 1)
            {
                if (string.IsNullOrEmpty(orderby))
                {
                    return string.Format("SELECT {0} FROM {1} WHERE {2}", field, GetTableName(tableInfo), where);
                }
                else
                {
                    return string.Format("SELECT {0} FROM {1} WHERE {2} ORDER BY {3}", field, GetTableName(tableInfo), where, orderby);
                }
            }
            else
            {
                if (string.IsNullOrEmpty(orderby))
                {
                    return string.Format("SELECT TOP {0} {1} FROM {2} WHERE {3}", top, field, GetTableName(tableInfo), where);
                }
                else
                {
                    return string.Format("SELECT TOP {0} {1} FROM {2} WHERE {3} ORDER BY {4}", top, field, GetTableName(tableInfo), where, orderby);
                }
            }
        }
        public string BuildSqlSelect<T>(string where, string orderby, int top, int lengthThreshold) where T : class, new()
        {
            var tableInfo = ZeroDb.GetDbDataTableInfo<T>();
            var temp = tableInfo.Colunms.FindAll(o => o.MaxLength < lengthThreshold);
            if (temp == null || temp.Count < 1)
            {
                throw new Exception("未能找到符合指定长度的返回字段");
            }
            string[] fieldArray = temp.Select(o => o.Name).ToArray();
            StringBuilder field = new StringBuilder();
            foreach (string s in fieldArray)
            {
                field.AppendFormat("{0},", s);
            }
            field.Remove(field.Length - 1, 1);
            if (string.IsNullOrEmpty(where))
            {
                where = "1>0";
            }
            if (top < 1)
            {
                if (string.IsNullOrEmpty(orderby))
                {
                    return string.Format("SELECT {0} FROM {1} WHERE {2}", field, GetTableName(tableInfo), where);
                }
                else
                {
                    return string.Format("SELECT {0} FROM {1} WHERE {2} ORDER BY {3}", field, GetTableName(tableInfo), where, orderby);
                }
            }
            else
            {
                if (string.IsNullOrEmpty(orderby))
                {
                    return string.Format("SELECT TOP {0} {1} FROM {2} WHERE {3}", top, field, GetTableName(tableInfo), where);
                }
                else
                {
                    return string.Format("SELECT TOP {0} {1} FROM {2} WHERE {3} ORDER BY {4}", top, field, GetTableName(tableInfo), where, orderby);
                }
            }
        }
        public string BuildSqlSelect<T>(System.Collections.Specialized.NameValueCollection nvc) where T : class, new()
        {
            return BuildSqlSelect<T>(nvc, "");
        }
        public string BuildSqlSelect<T>(System.Collections.Specialized.NameValueCollection nvc, string appendWhere) where T : class, new()
        {
            var nvcList = new List<System.Collections.Specialized.NameValueCollection>(1);
            nvcList.Add(nvc);
            return BuildSqlSelect<T>(nvcList, appendWhere)[0];
        }
        public List<string> BuildSqlSelect<T>(List<System.Collections.Specialized.NameValueCollection> nvcList) where T : class, new()
        {
            return BuildSqlSelect<T>(nvcList, "");
        }
        public List<string> BuildSqlSelect<T>(List<System.Collections.Specialized.NameValueCollection> nvcList, string appendWhere) where T : class, new()
        {
            if (nvcList == null || nvcList.Count < 1) { throw new Exception("nvcList是null或长度为0"); }
            if (nvcList.Contains(null))
            {
                throw new Exception("nvcList存在null的项");
            }

            var tableInfo = ZeroDb.GetDbDataTableInfo<T>();
            var primaryKeys = tableInfo.Colunms.FindAll(o=>o.IsPrimaryKey);
            if (primaryKeys == null || primaryKeys.Count < 1)
            {
                var uniqueFieldName = GetUniqueFieldName<T>(tableInfo);
                if (string.IsNullOrEmpty(uniqueFieldName))
                {
                    throw new Exception("目标tableInfo缺少主键和唯一标识列");
                }
                var col = tableInfo.Colunms.Find(o => string.Equals(o.Name, uniqueFieldName, StringComparison.OrdinalIgnoreCase));
                if (col == null)
                {
                    throw new Exception("目标tableInfo配置的唯一列" + uniqueFieldName + "不存在");
                }
                primaryKeys.Clear();
                primaryKeys.Add(col);
            }

            bool isAppendWhere = false;
            if (!string.IsNullOrEmpty(appendWhere))
            {
                appendWhere = appendWhere.Trim();
                if (appendWhere.StartsWith("where", StringComparison.OrdinalIgnoreCase))
                {
                    appendWhere = appendWhere.Remove(0, 5);
                }
                appendWhere = appendWhere.Trim();
                isAppendWhere = !string.IsNullOrEmpty(appendWhere);
            }

            T result = (T)Activator.CreateInstance(typeof(T));
            System.Reflection.PropertyInfo[] properties = result.GetType().GetProperties();
            List<System.Reflection.PropertyInfo> propertyInfoList = new List<System.Reflection.PropertyInfo>(properties.Length);
            foreach (System.Reflection.PropertyInfo p in properties)
            {
                propertyInfoList.Add(p);
            }
            foreach (var c in primaryKeys)
            {
                if (propertyInfoList.Find(o=> string.Equals(o.Name, c.Name, StringComparison.OrdinalIgnoreCase)) == null)
                {
                    throw new Exception("实体缺少对字段" + c.Name + "的映射");
                }
            }
            List<string> reval = new List<string>();
            var sqlColumns = tableInfo.Colunms.Select(o => o.Name).ToArray();
            int nvcListCount = nvcList.Count;
            for (int i = 0; i < nvcListCount; i++)
            {
                if (nvcList[i].Count - primaryKeys.Count < 1)
                {
                    continue;
                }
                bool nvcContainsKeyKeys = true;
                StringBuilder sqlWhere = new StringBuilder();
                foreach (var c in primaryKeys)
                {
                    if (null == nvcList[i][c.Name])
                    {
                        nvcContainsKeyKeys = false;
                        continue;
                    }
                    else
                    {
                        System.Reflection.PropertyInfo p = propertyInfoList.Find(delegate (System.Reflection.PropertyInfo t) {
                            return string.Equals(t.Name, c.Name, StringComparison.OrdinalIgnoreCase);
                        });
                        object targetValue = Common.ValueConvert.StrToTargetType(nvcList[i][c.Name], p.PropertyType);
                        string valueString = Common.ValueConvert.SqlValueStrByValue(targetValue);
                        sqlWhere.AppendFormat("{0}={1} AND ", c.Name, valueString);
                    }
                }
                if (!nvcContainsKeyKeys) { continue; }
                if (nvcList[i].Count < 1) { continue; }

                if (isAppendWhere)
                {
                    sqlWhere.Append(appendWhere);
                }
                else
                {
                    sqlWhere.Remove(sqlWhere.Length - 5, 5);
                }

                reval.Add(string.Format("SELECT {0} FROM {1} WHERE {2}", sqlColumns, GetTableName(tableInfo), sqlWhere));
            }
            if (reval == null || reval.Count < 1)
            {
                throw new Exception("未能生成查询命令，可能目标tableInfo缺少主键或标识列或是NameValueCollection为空或未包含主键与标识列");
            }
            return reval;
        }
        public string BuildSqlCount<T>(string where) where T : class, new()
        {
            var tableInfo = this.ZeroDb.GetDbDataTableInfo<T>();
            return string.Format("SELECT COUNT(1) FROM {0} WHERE {1}", GetTableName(tableInfo), string.IsNullOrEmpty(where) ? "1>0" : where);
        }
        public string BuildSqlSelectByKey<T>(object key) where T : class, new()
        {
            var tableInfo = ZeroDb.GetDbDataTableInfo<T>();
            ZeroDbs.Interfaces.Common.DbDataColumnInfo dbDataColumn = null;
            var temp = tableInfo.Colunms.FindAll(o => o.IsPrimaryKey);
            if (temp != null && temp.Count == 1)
            {
                dbDataColumn = temp[0];
            }
            if (dbDataColumn == null)
            {
                temp = tableInfo.Colunms.FindAll(o => o.IsIdentity);
                if (temp != null && temp.Count > 0)
                {
                    dbDataColumn = temp[0];
                }
            }
            if (dbDataColumn == null)
            {
                throw new Exception("缺少唯一标识列");
            }
            System.Reflection.PropertyInfo[] pi = typeof(T).GetProperties();
            var p = pi.ToList().Find(o => string.Equals(dbDataColumn.Name, o.Name, StringComparison.OrdinalIgnoreCase));
            if (key.GetType() != p.PropertyType)
            {
                throw new Exception("key类型错误");
            }
            var fieldNames = string.Join(",", tableInfo.Colunms.Select(o => o.Name).ToArray());
            var value = Common.ValueConvert.SqlValueStrByValue(key);
            var where = string.Format("{0}={1}", dbDataColumn.Name, value);

            return string.Format("SELECT {0} FROM {1} WHERE {2}", fieldNames, GetTableName(tableInfo), where);
        }


        private string GetTableName(ZeroDbs.Interfaces.Common.DbDataTableInfo tableInfo) {
            return "[" + tableInfo.DbName + "].[dbo].[" + tableInfo.Name + "]";
        }


    }
}
