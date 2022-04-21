using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZeroDbs.Common
{
    /// <summary>
    /// based sqlserver
    /// </summary>
    public abstract class SqlBuilder
    {
        private IDb db = null;
        public IDb ZeroDb { get { return db; } }

        public SqlBuilder(IDb db)
        {
            this.db = db;
        }

        public Common.DbDataTableInfo GetTable<DbEntity>() where DbEntity : class, new()
        {
            var reval = this.db.GetTable<DbEntity>();
            if(reval!= null)
            {
                return reval;
            }
            throw new Exception("类型" + typeof(DbEntity).FullName + "没有映射到" + ZeroDb.Database.dbKey + "上");
        }
        public virtual string[] GetUniqueFieldName(Common.DbDataTableInfo tableInfo)
        {
            var keys = tableInfo.Colunms.FindAll(o => o.IsPrimaryKey);
            if (keys != null && keys.Count > 0)
            {
                return keys.Select(o=>o.Name).ToArray();
            }
            var key = tableInfo.Colunms.Find(o => o.IsIdentity);
            if (key != null)
            {
                return new string[] { key.Name };
            }
            return new string[0];
        }
        public virtual string GetTableName(ZeroDbs.Common.DbDataTableInfo tableInfo)
        {
            return tableInfo.Name;
        }
        public virtual string GetColunmName(string colName)
        {
            return colName;
        }

        public virtual string Count<DbEntity>(string where) where DbEntity : class, new()
        {
            var tableInfo = this.GetTable<DbEntity>();
            return string.Format("SELECT COUNT(1) FROM {0} WHERE {1}", GetTableName(tableInfo), string.IsNullOrEmpty(where) ? "1>0" : where);
        }
        
        public virtual string Page<DbEntity>(long page, long size, string where, string orderby, string[] returnFieldNames, string uniqueFieldName = "") where DbEntity : class, new()
        {
            var tableInfo = this.ZeroDb.GetTable<DbEntity>();

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
                    fieldStr.AppendFormat("{0},", GetColunmName(returnFieldNames[i]));
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
                var ts = GetUniqueFieldName(tableInfo);
                uniqueFieldName = ts.Length == 1 ? ts[0] : string.Empty;
            }
            if (!string.IsNullOrEmpty(uniqueFieldName))//具有唯一性字段
            {
                string resultFieldName = GetColunmName(uniqueFieldName);
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

        public virtual SqlInfo Select<DbEntity>(string where, string orderby, int top, string[] returnFieldNames, params object[] paras) where DbEntity : class, new()
        {
            SqlInfo reval = new SqlInfo();
            var tableInfo = this.GetTable<DbEntity>();
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
                field.AppendFormat("{0},", GetColunmName(s));
            }
            if (field.Length < 1)
            {
                throw new Exception("The resulting return field is invalid");
            }
            field.Remove(field.Length - 1, 1);
            if (string.IsNullOrEmpty(where))
            {
                where = "1>0";
            }
            var tableName = GetTableName(tableInfo);
            if (top < 1)
            {
                if (string.IsNullOrEmpty(orderby))
                {
                    reval.Sql = string.Format("SELECT {0} FROM {1} WHERE {2}", field, tableName, where);
                }
                else
                {
                    reval.Sql = string.Format("SELECT {0} FROM {1} WHERE {2} ORDER BY {3}", field, tableName, where, orderby);
                }
            }
            else
            {
                if (string.IsNullOrEmpty(orderby))
                {
                    reval.Sql = string.Format("SELECT {1} FROM {2} WHERE {3} LIMIT {0}", top, field, tableName, where);
                }
                else
                {
                    reval.Sql = string.Format("SELECT {1} FROM {2} WHERE {3} ORDER BY {4} LIMIT {0}", top, field, tableName, where, orderby);
                }
            }
            int n = 0;
            int m = paras.Length;
            while (n < m)
            {
                reval.Paras.Add(n.ToString(),paras[n]);
                n++;
            }
            return reval;
        }

        public string Insert<DbEntity>() where DbEntity : class, new()
        {
            return Insert<DbEntity>(new string[0], null);
        }
        public string Insert<DbEntity>(string[] skipFieldNames) where DbEntity : class, new()
        {
            return Insert<DbEntity>(skipFieldNames, null);
        }
        public virtual string Insert<DbEntity>(string[] skipFieldNames, string appendWhere) where DbEntity : class, new()
        {
            var tableInfo = this.GetTable<DbEntity>();
            if (tableInfo.IsView)
            {
                throw new Exception("Does not support Insert operation on the view");
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
            if (tableInfo == null) { throw new Exception("tableInfo is null"); }
            if (skipFieldNames == null) { skipFieldNames = new string[] { }; }

            System.Reflection.PropertyInfo[] pi = typeof(DbEntity).GetProperties();
            if (pi.Length < 1) { throw new Exception("Generic parameters are missing public properties"); }
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
                throw new Exception("All fields are skipped");
            }
            System.Text.StringBuilder columnNames = new System.Text.StringBuilder();
            System.Text.StringBuilder columnValues = new System.Text.StringBuilder();
            foreach (string s in keepFieldNames)
            {
                string columnName = ("" + s).ToLower().Trim();
                if (columnName.Length > 0 && dic.ContainsKey(columnName))
                {
                    columnNames.AppendFormat("{0},", GetColunmName(dic[columnName].Name));
                    columnValues.AppendFormat("@{0},", dic[columnName].Name);
                }
            }
            if (columnNames.Length < 1)
            {
                throw new Exception("The generated Insert command is not available");
            }
            columnNames.Remove(columnNames.Length - 1, 1);
            columnValues.Remove(columnValues.Length - 1, 1);
            if (!string.IsNullOrWhiteSpace(appendWhere))
            {
                appendWhere = " WHERE " + appendWhere.Trim();
            }
            else
            {
                appendWhere = "";
            }
            return string.Format("INSERT INTO {0}({1}) VALUES({2}){3}", GetTableName(tableInfo), columnNames, columnValues, appendWhere);
        }
        public string Insert<DbEntity>(System.Collections.Specialized.NameValueCollection nvc) where DbEntity : class, new()
        {
            return Insert<DbEntity>(nvc, "");
        }
        public string Insert<DbEntity>(System.Collections.Specialized.NameValueCollection nvc, string appendWhere) where DbEntity : class, new()
        {
            List<System.Collections.Specialized.NameValueCollection> li = new List<System.Collections.Specialized.NameValueCollection>(1);
            li.Add(nvc);
            List<string> reval = Insert<DbEntity>(li, appendWhere);
            return reval[0];
        }
        public List<string> Insert<DbEntity>(List<System.Collections.Specialized.NameValueCollection> nvcList) where DbEntity : class, new()
        {
            return Insert<DbEntity>(nvcList, "");
        }
        public virtual List<string> Insert<DbEntity>(List<System.Collections.Specialized.NameValueCollection> nvcList, string appendWhere) where DbEntity : class, new()
        {
            if (nvcList == null || nvcList.Count < 1) { throw new Exception("nvcList is null or contains 0 items"); }
            if (nvcList.Count > 5000)
            {
                throw new Exception("The length of nvcList should not exceed 5000 items");
            }
            if (nvcList.Contains(null))
            {
                throw new Exception("nvcList contains null items");
            }
            var tableInfo = this.GetTable<DbEntity>();
            if (tableInfo.IsView)
            {
                throw new Exception("Does not support Insert operation on the view");
            }

            var identityKeys = tableInfo.Colunms.FindAll(o => o.IsIdentity);
            
            if (!string.IsNullOrEmpty(appendWhere))
            {
                appendWhere = " WHERE "+appendWhere.Trim();
            }
            var tableName = GetTableName(tableInfo);

            List<string> result = new List<string>();
            List<System.Reflection.PropertyInfo> propertyInfoList = typeof(DbEntity).GetProperties().ToList();
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
                        if (identityKeys.Find(delegate (ZeroDbs.Common.DbDataColumnInfo temp) { return string.Equals(temp.Name, p.Name, StringComparison.OrdinalIgnoreCase); }) == null)
                        {
                            sqlInsertFields.AppendFormat("{0},", GetColunmName(p.Name));
                            object TargetValue = Common.ValueConvert.StrToTargetType(nvcList[i][key], p.PropertyType);
                            string ValueString = Common.ValueConvert.SqlValueStrByValue(TargetValue);
                            sqlInsertValues.AppendFormat("{0},", ValueString);
                        }
                    }
                }
                if (sqlInsertFields.Length < 1 || sqlInsertValues.Length < 1) { continue; }

                sqlInsertFields.Remove(sqlInsertFields.Length - 1, 1);
                sqlInsertValues.Remove(sqlInsertValues.Length - 1, 1);

                string sql = string.Format("INSERT INTO {0}({1}) VALUES({2}){3}", tableName, sqlInsertFields, sqlInsertValues, appendWhere);
                if (!result.Contains(sql))
                {
                    result.Add(sql);
                }
            }
            return result;
        }

        public SqlInfo UpdateFromCustomEntity<DbEntity>(object entity) where DbEntity : class, new()
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            var tableInfo = this.GetTable<DbEntity>();
            if (tableInfo.IsView)
            {
                throw new Exception("Target does not support update operation");
            }
            var pKeys = tableInfo.Colunms.FindAll(o => o.IsPrimaryKey);
            if(pKeys.Count == 0)
            {
                throw new ArgumentNullException("The target table is missing a primary key");
            }
            var ps = entity.GetType().GetProperties().ToList();
            var dic = new Dictionary<System.Reflection.PropertyInfo, DbDataColumnInfo>();
            int keyCount = 0;
            for(int i = 0; i < ps.Count; i++)
            {
                var col = tableInfo.Colunms.Find(o => string.Equals(o.Name, ps[i].Name, StringComparison.OrdinalIgnoreCase));
                if (col == null)
                {
                    throw new Exception("The " + ps[i].Name + " field does not exist in the target table");
                }
                if (pKeys.Contains(col))
                {
                    keyCount++;
                }
                dic.Add(ps[i], col);
            }
            if (keyCount < pKeys.Count)
            {
                throw new Exception("Input source is missing fields other than primary key fields");
            }
            if (keyCount == ps.Count)
            {
                throw new Exception("Missing field that needs to be updated");
            }

            SqlInfo reval = new SqlInfo();
            StringBuilder set = new StringBuilder();
            StringBuilder where = new StringBuilder();
            foreach(var key in dic.Keys)
            {
                var col = dic[key];
                if (col.IsPrimaryKey)
                {
                    where.AppendFormat("{0}=@{1} AND ", GetColunmName(col.Name), col.Name);
                }
                else
                {
                    if (col.IsIdentity) { continue; }
                    set.AppendFormat("{0}=@{1},", GetColunmName(col.Name), col.Name);
                }
                reval.Paras.Add(col.Name, key.GetValue(entity, null));
            }
            if (set.Length < 1)
            {
                throw new Exception("Missing field that needs to be updated");
            }
            set.Remove(set.Length - 1, 1);
            where.Remove(where.Length - 5, 5);
            reval.Sql = String.Format("UPDATE {0} SET {1} WHERE {2}", GetTableName(tableInfo), set, where);

            return reval;
        }
        public SqlInfo UpdateFromDictionary<DbEntity>(Dictionary<string,object> source) where DbEntity : class, new()
        {
            if (source == null|| source.Count < 1)
            {
                throw new ArgumentNullException("source");
            }
            var tableInfo = this.GetTable<DbEntity>();
            if (tableInfo.IsView)
            {
                throw new Exception("Target does not support update operation");
            }
            var pKeys = tableInfo.Colunms.FindAll(o => o.IsPrimaryKey);
            if (pKeys.Count == 0)
            {
                throw new ArgumentNullException("The target table is missing a primary key");
            }
            List<object> values = new List<object>(source.Count);
            List<DbDataColumnInfo> cols = new List<DbDataColumnInfo>();
            int keyCount = 0;
            foreach (var key in source.Keys)
            {
                var col= tableInfo.Colunms.Find(o=>string.Equals(key,o.Name,StringComparison.OrdinalIgnoreCase));
                if(col == null)
                {
                    throw new Exception("The " + key + " field does not exist in the target table");
                }
                if (col.IsPrimaryKey)
                {
                    keyCount++;
                }
                values.Add(source[key]);
                cols.Add(col);
            }
            if (keyCount < pKeys.Count)
            {
                throw new Exception("Input source is missing fields other than primary key fields");
            }
            if (keyCount == source.Count)
            {
                throw new Exception("Missing field that needs to be updated");
            }

            SqlInfo reval = new SqlInfo();
            StringBuilder set = new StringBuilder();
            StringBuilder where = new StringBuilder();
            for (int i = 0; i < values.Count; i++)
            {
                var col = cols[i];
                if (col.IsPrimaryKey)
                {
                    where.AppendFormat("{0}=@{1} AND ", GetColunmName(col.Name), col.Name);
                }
                else
                {
                    if (col.IsIdentity) { continue; }
                    set.AppendFormat("{0}=@{1},", GetColunmName(col.Name), col.Name);
                }
                reval.Paras.Add(col.Name, values[i]);
            }
            set.Remove(set.Length - 1, 1);
            where.Remove(where.Length - 5, 5);
            reval.Sql = String.Format("UPDATE {0} SET {1} WHERE {2}", GetTableName(tableInfo), set, where);

            return reval;
        }
        public SqlInfo UpdateFromNameValueCollection<DbEntity>(System.Collections.Specialized.NameValueCollection nvc) where DbEntity : class, new()
        {
            if (nvc == null || nvc.Count < 1) { throw new ArgumentException("nvc"); }

            var tableInfo = this.GetTable<DbEntity>();
            if (tableInfo.IsView)
            {
                throw new Exception("Target does not support update operation");
            }
            var pKeys = tableInfo.Colunms.FindAll(o => o.IsPrimaryKey);
            if (pKeys.Count < 1)
            {
                throw new Exception("Missing unique identity column");
            }

            var ps = typeof(DbEntity).GetProperties().ToList();
            var cols = new List<DbDataColumnInfo>();
            var values = new List<object>();
            int keyCount = 0;
            foreach (var key in nvc.AllKeys)
            {
                var col = tableInfo.Colunms.Find(o => string.Equals(o.Name, key));
                if (col == null)
                {
                    throw new Exception("The " + key + " field does not exist in the target table");
                }
                var p = ps.Find(o => string.Equals(o.Name, key));
                if (p == null)
                {
                    throw new Exception("Don't know the data type of field " + key);
                }
                if (col.IsPrimaryKey)
                {
                    keyCount++;
                }
                values.Add(ValueConvert.StrToTargetType(nvc[key], p.PropertyType));
                cols.Add(col);
            }
            if (keyCount < pKeys.Count)
            {
                throw new Exception("Input source is missing fields other than primary key fields");
            }
            if (keyCount == nvc.Count)
            {
                throw new Exception("Missing field that needs to be updated");
            }

            SqlInfo reval = new SqlInfo();
            StringBuilder set = new StringBuilder();
            StringBuilder where = new StringBuilder();
            for (int i = 0; i < values.Count; i++)
            {
                var col = cols[i];
                if (col.IsPrimaryKey)
                {
                    where.AppendFormat("{0}=@{1} AND ", GetColunmName(col.Name), col.Name);
                }
                else
                {
                    if (col.IsIdentity) { continue; }
                    set.AppendFormat("{0}=@{1},", GetColunmName(col.Name), col.Name);
                }
                reval.Paras.Add(col.Name, values[i]);
            }
            set.Remove(set.Length - 1, 1);
            where.Remove(where.Length - 5, 5);
            reval.Sql = String.Format("UPDATE {0} SET {1} WHERE {2}", GetTableName(tableInfo), set, where);

            return reval;
        }
        public string Update<DbEntity>() where DbEntity : class, new()
        {
            var tableInfo = this.GetTable<DbEntity>();
            if (tableInfo.IsView)
            {
                throw new Exception("Target does not support update operation");
            }
            StringBuilder set = new StringBuilder();
            StringBuilder where = new StringBuilder();
            foreach (var col in tableInfo.Colunms)
            {
                if (col.IsPrimaryKey)
                {
                    where.AppendFormat("{0}=@{1} AND ", GetColunmName(col.Name), col.Name);
                }
                else
                {
                    if (col.IsIdentity) { continue; }
                    set.AppendFormat("{0}=@{1},", GetColunmName(col.Name), col.Name);
                }
            }
            if (where.Length < 1)
            {
                throw new ArgumentNullException("The target table is missing a primary key");
            }
            set.Remove(set.Length - 1, 1);
            where.Remove(where.Length - 5, 5);

            return String.Format("UPDATE {0} SET {1} WHERE {2}", GetTableName(tableInfo), set, where); ;
        }
        public SqlInfo Update<DbEntity>(DbEntity entity) where DbEntity : class, new()
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            var tableInfo = this.GetTable<DbEntity>();
            if (tableInfo.IsView)
            {
                throw new Exception("Target does not support update operation");
            }
            var pKeys = tableInfo.Colunms.FindAll(o => o.IsPrimaryKey);
            if (pKeys.Count == 0)
            {
                throw new ArgumentNullException("The target table is missing a primary key");
            }
            var ps = entity.GetType().GetProperties().ToList();
            var dic = new Dictionary<System.Reflection.PropertyInfo, DbDataColumnInfo>();
            int keyCount = 0;
            for (int i = 0; i < ps.Count; i++)
            {
                var col = tableInfo.Colunms.Find(o => string.Equals(o.Name, ps[i].Name, StringComparison.OrdinalIgnoreCase));
                if (col == null)
                {
                    throw new Exception("The " + ps[i].Name + " field does not exist in the target table");
                }
                if (pKeys.Contains(col))
                {
                    keyCount++;
                }
                dic.Add(ps[i], col);
            }
            if (keyCount < pKeys.Count)
            {
                throw new Exception("Input source is missing fields other than primary key fields");
            }
            if (keyCount == ps.Count)
            {
                throw new Exception("Missing field that needs to be updated");
            }

            SqlInfo reval = new SqlInfo();
            StringBuilder set = new StringBuilder();
            StringBuilder where = new StringBuilder();
            foreach (var key in dic.Keys)
            {
                var col = dic[key];
                if (col.IsPrimaryKey)
                {
                    where.AppendFormat("{0}=@{1} AND ", GetColunmName(col.Name), col.Name);
                }
                else
                {
                    if (col.IsIdentity) { continue; }
                    set.AppendFormat("{0}=@{1},", GetColunmName(col.Name), col.Name);
                }
                reval.Paras.Add(col.Name, key.GetValue(entity, null));
            }
            if (set.Length < 1)
            {
                throw new Exception("Missing field that needs to be updated");
            }
            set.Remove(set.Length - 1, 1);
            where.Remove(where.Length - 5, 5);
            reval.Sql = String.Format("UPDATE {0} SET {1} WHERE {2}", GetTableName(tableInfo), set, where);

            return reval;
        }

        public virtual SqlInfo Delete<DbEntity>(string where, params object[] paras) where DbEntity : class, new()
        {
            if (string.IsNullOrEmpty(where))
            {
                where = "1>0";
            }
            where = where.Trim();
            if (where.StartsWith("WHERE ", StringComparison.OrdinalIgnoreCase))
            {
                where = where.Remove(0, 5);
            }
            var tableInfo = this.GetTable<DbEntity>();
            if (tableInfo.IsView)
            {
                throw new Exception("Does not support Delete operation on the view");
            }
            SqlInfo reval = new SqlInfo();
            reval.Sql= string.Format("DELETE FROM {0} WHERE {1}", GetTableName(tableInfo), where);
            int n = 0;
            int m=paras.Length;
            while (n < m)
            {
                reval.Paras.Add(n.ToString(), paras[n]);
                n++;
            }
            return reval;
        }



    }
}
