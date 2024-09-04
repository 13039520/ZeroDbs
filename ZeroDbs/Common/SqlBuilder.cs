using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;

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

        protected List<System.Reflection.PropertyInfo> GetPropertyInfos<DbEntity>() where DbEntity : class, new()
        {
            return PropertyInfoCache.GetPropertyInfoList<DbEntity>();
        }
        protected List<System.Reflection.PropertyInfo> GetPropertyInfos(Type entityType)
        {
            return PropertyInfoCache.GetPropertyInfoList(entityType);
        }
        protected void SqlInfoUseParas(ref SqlInfo info, params object[] paras)
        {
            int n = 0;
            int m = paras.Length;
            while (n < m)
            {
                info.Paras.Add(n.ToString(), paras[n]);
                n++;
            }
        }

        public ITableInfo GetTable<DbEntity>() where DbEntity : class, new()
        {
            var reval = this.db.GetTable<DbEntity>();
            if(reval!= null)
            {
                return reval;
            }
            throw new Exception("类型" + typeof(DbEntity).FullName + "没有映射到" + ZeroDb.DbInfo.Key + "上");
        }
        public ITableInfo GetTable(string entityFullName)
        {
            var reval = this.db.GetTable(entityFullName);
            if (reval != null)
            {
                return reval;
            }
            throw new Exception("类型" + entityFullName + "没有映射到" + ZeroDb.DbInfo.Key + "上");
        }
        public virtual string[] GetUniqueFieldName(ITableInfo tableInfo)
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
        public virtual string GetTableName(ITableInfo tableInfo)
        {
            return tableInfo.Name;
        }
        public virtual string GetColunmName(string colName)
        {
            return colName;
        }

        public SqlInfo Count<DbEntity>(string where, params object[] paras) where DbEntity : class, new()
        {
            return Count(GetTable<DbEntity>(), where, paras);
        }
        public SqlInfo Count(Type entityType, string where, params object[] paras)
        {
            return Count(GetTable(entityType.FullName), where, paras);
        }
        public virtual SqlInfo Count(ITableInfo table, string where, params object[] paras)
        {
            SqlInfo reval = new SqlInfo();
            reval.Sql = string.Format("SELECT COUNT(1) FROM {0} WHERE {1}", GetTableName(table), string.IsNullOrEmpty(where) ? "1>0" : where);
            SqlInfoUseParas(ref reval, paras);
            return reval;
        }

        public SqlInfo MaxIdentityPrimaryKeyValue<DbEntity>() where DbEntity : class, new()
        {
            return MaxIdentityPrimaryKeyValue<DbEntity>("");
        }
        public SqlInfo MaxIdentityPrimaryKeyValue<DbEntity>(string where, params object[] paras) where DbEntity : class, new()
        {
            return MaxIdentityPrimaryKeyValue(GetTable<DbEntity>(), where, paras);
        }
        public SqlInfo MaxIdentityPrimaryKeyValue(Type entityType, string where, params object[] paras)
        {
            return MaxIdentityPrimaryKeyValue(GetTable(entityType.FullName), where, paras);
        }
        public virtual SqlInfo MaxIdentityPrimaryKeyValue(ITableInfo table, string where, params object[] paras)
        {
            var col = table.Colunms.Find(o => o.IsPrimaryKey && o.IsIdentity);
            if (col == null)
            {
                throw new Exception("The target is missing an identity primary key field");
            }
            if (!string.IsNullOrEmpty(where))
            {
                where = string.Format(" WHERE {0}", where);
            }
            SqlInfo reval = new SqlInfo(paras.Length) { Sql = string.Format("SELECT MAX({0}) FROM {1}{2}", GetColunmName(col.Name), GetTableName(table), where) };
            SqlInfoUseParas(ref reval, paras);
            return reval;
        }

        public SqlInfo Page<DbEntity>(long page, long size) where DbEntity : class, new()
        {
            return Page<DbEntity>(page, size, "");
        }
        public SqlInfo Page<DbEntity>(long page, long size, string where) where DbEntity : class, new()
        {
            return Page<DbEntity>(page, size, where, "");
        }
        public SqlInfo Page<DbEntity>(long page, long size, string where, string orderby) where DbEntity : class, new()
        {
            return Page<DbEntity>(page, size, where, orderby, new string[0]);
        }
        public SqlInfo Page<DbEntity>(long page, long size, string where, string orderby, string[] fields) where DbEntity : class, new()
        {
            return Page<DbEntity>(page, size, where, orderby, fields, "");
        }
        public SqlInfo Page<DbEntity>(long page, long size, string where, string orderby, string[] fields, string uniqueField = "") where DbEntity : class, new()
        {
            return Page<DbEntity>(page, size, where, orderby, fields, uniqueField, new object[0]);
        }
        public SqlInfo Page<DbEntity>(long page, long size, string where, string orderby, string[] fields, string uniqueField = "", params object[] paras) where DbEntity : class, new()
        {
            var tableInfo = this.ZeroDb.GetTable<DbEntity>();
            return Page(tableInfo, page, size, where, orderby, fields, uniqueField, paras);
        }
        public SqlInfo Page(Type entityType, PageQuery query)
        {
            return Page(GetTable(entityType.FullName), query.Page, query.Size, query.Where, query.Orderby, query.Fields, query.Unique,query.Paras);
        }
        public virtual SqlInfo Page(ITableInfo table, long page, long size, string where, string orderby, string[] fields, string uniqueField = "", params object[] paras)
        {
            page = page < 1 ? 1 : page;
            size = size < 1 ? 1 : size;

            long startIndex = page * size - size + 1;
            long endIndex = page * size;
            StringBuilder sql = new StringBuilder();
            StringBuilder fieldStr = new StringBuilder();
            bool needCheck = true;
            if (fields == null || fields.Length < 1)
            {
                needCheck = false;
                fields = table.Colunms.FindAll(o => o.MaxLength < 1000).Select(o => o.Name).ToArray();
            }
            for (int i = 0; i < fields.Length; i++)
            {
                if (needCheck)
                {
                    var col = table.Colunms.Find(o => string.Equals(o.Name, fields[i], StringComparison.OrdinalIgnoreCase));
                    if (col != null)
                    {
                        fieldStr.AppendFormat("{0},", GetColunmName(col.Name));
                    }
                }
                else
                {
                    fieldStr.AppendFormat("{0},", GetColunmName(fields[i]));
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
            if (string.IsNullOrEmpty(uniqueField))
            {
                var ts = GetUniqueFieldName(table);
                uniqueField = ts.Length == 1 ? ts[0] : string.Empty;
            }
            if (!string.IsNullOrEmpty(uniqueField))//具有唯一性字段
            {
                string resultFieldName = GetColunmName(uniqueField);
                sql.AppendFormat("SELECT {0} FROM {1}", fieldStr, GetTableName(table));
                //获取唯一性字段集合
                sql.AppendFormat(" WHERE {0} IN(SELECT {0} FROM(", resultFieldName);
                sql.AppendFormat("SELECT ROW_NUMBER() OVER (ORDER BY {0})AS Row,{1} FROM {2}", orderby, resultFieldName, GetTableName(table));
                if (!string.IsNullOrEmpty(where))
                {
                    sql.AppendFormat(" WHERE {0}", where);
                }
                sql.AppendFormat(")TT WHERE TT.Row BETWEEN {0} AND {1}) ORDER BY {2}", startIndex, endIndex, orderby);
            }
            else//没有唯一性字段
            {
                sql.AppendFormat("SELECT {0} FROM (", fieldStr);
                sql.AppendFormat("SELECT ROW_NUMBER() OVER (ORDER BY {0})AS Row,{1} FROM {2}", orderby, fieldStr, GetTableName(table));
                if (!string.IsNullOrEmpty(where))
                {
                    sql.AppendFormat(" WHERE {0}", where);
                }
                sql.Append(") TT");
                sql.AppendFormat(" WHERE TT.Row BETWEEN {0} AND {1}", startIndex, endIndex);
            }
            SqlInfo reval = new SqlInfo(paras.Length);
            reval.Sql = sql.ToString();
            SqlInfoUseParas(ref reval, paras);
            return reval;
        }

        public SqlInfo Select<DbEntity>(ListQuery query) where DbEntity : class, new()
        {
            return Select<DbEntity>(query.Where, query.Orderby, query.Top, query.Fields, query.Paras);
        }
        public SqlInfo Select<DbEntity>(string where) where DbEntity : class, new()
        {
            return Select<DbEntity>(where, "");
        }
        public SqlInfo Select<DbEntity>(string where, string orderby) where DbEntity : class, new()
        {
            return Select<DbEntity>(where, orderby, 0);
        }
        public SqlInfo Select<DbEntity>(string where, string orderby, int top) where DbEntity : class, new()
        {
            return Select<DbEntity>(where, orderby, top, new string[0]);
        }
        public SqlInfo Select<DbEntity>(string where, string orderby, int top, string[] fields) where DbEntity : class, new()
        {
            return Select<DbEntity>(where, orderby, top, fields, new object[0]);
        }
        public SqlInfo Select<DbEntity>(string where, string orderby, int top, string[] fields, params object[] paras) where DbEntity : class, new()
        {
            return Select(GetTable<DbEntity>(),where, orderby, top, fields, paras);
        }
        public SqlInfo Select(Type entityType, ListQuery query)
        {
            return Select(GetTable(entityType.FullName), query.Where, query.Orderby, query.Top, query.Fields, query.Paras);
        }
        public virtual SqlInfo Select(ITableInfo table, string where, string orderby, int top, string[] fields, params object[] paras)
        {
            SqlInfo reval = new SqlInfo();
            StringBuilder field = new StringBuilder();
            bool needCheck = true;
            if (fields == null || fields.Length < 1)
            {
                needCheck = false;
                fields = table.Colunms.FindAll(o => o.MaxLength < 1000).Select(o => o.Name).ToArray();
            }
            for (int i = 0; i < fields.Length; i++)
            {
                if (needCheck)
                {
                    var col = table.Colunms.Find(o => string.Equals(o.Name, fields[i], StringComparison.OrdinalIgnoreCase));
                    if (col != null)
                    {
                        field.AppendFormat("{0},", GetColunmName(col.Name));
                    }
                }
                else
                {
                    field.AppendFormat("{0},", GetColunmName(fields[i]));
                }
            }
            if (field.Length > 0)
            {
                field.Remove(field.Length - 1, 1);
            }
            else
            {
                field.Append("*");
            }
            if (string.IsNullOrEmpty(where))
            {
                where = "1>0";
            }
            var tableName = GetTableName(table);
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
                    reval.Sql = string.Format("SELECT TOP {0} {1} FROM {2} WHERE {3}", top, field, tableName, where);
                }
                else
                {
                    reval.Sql = string.Format("SELECT TOP {0} {1} FROM {2} WHERE {3} ORDER BY {4}", top, field, tableName, where, orderby);
                }
            }
            SqlInfoUseParas(ref reval, paras);
            return reval;
        }
        public SqlInfo SelectByPrimaryKey<DbEntity>(object key) where DbEntity : class, new()
        {
            return SelectByPrimaryKey(GetTable<DbEntity>(), key);
        }
        public SqlInfo SelectByPrimaryKey(Type entityType, object key)
        {
            return SelectByPrimaryKey(GetTable(entityType.FullName), key);
        }
        public SqlInfo SelectByPrimaryKey(ITableInfo table, object key)
        {
            if (key == null) { throw new ArgumentNullException("key"); }

            var pKeys = table.Colunms.FindAll(o => o.IsPrimaryKey);
            if (pKeys.Count < 1)
            {
                throw new ArgumentNullException("The target table is missing a primary key");
            }
            var dic = new Dictionary<IColumnInfo, object>(pKeys.Count);
            var type = key.GetType();
            if (pKeys.Count > 1)
            {
                var ps = GetPropertyInfos(type);
                foreach(var k in pKeys)
                {
                    var p = ps.Find(o => string.Equals(o.Name, k.Name, StringComparison.OrdinalIgnoreCase));
                    if(p == null)
                    {
                        throw new ArgumentNullException("Parameter '" + key + "' must have property '" + k.Name + "'");
                    }
                    var val = p.GetValue(key, null);
                    dic.Add(k, val);
                }
            }
            else
            {
                dic.Add(pKeys[0], key);
            }
            SqlInfo reval = new SqlInfo(dic.Count);
            StringBuilder where = new StringBuilder();
            foreach(var item in dic.Keys)
            {
                where.AppendFormat("{0}=@{1} AND ", GetColunmName(item.Name), item.Name);
                reval.Paras.Add(item.Name, dic[item]);
            }
            where.Remove(where.Length - 5, 5);
            string[] fields = table.Colunms.Select(o => o.Name).ToArray();
            for(int i = 0; i < fields.Length; i++)
            {
                fields[i] = GetColunmName(fields[i]);
            }
            reval.Sql = string.Format("SELECT {0} FROM {1} WHERE {2}", string.Join(",", fields), GetTableName(table), where);
            return reval;
        }

        public string Insert<DbEntity>() where DbEntity : class, new()
        {
            return Insert(GetTable<DbEntity>());
        }
        public string Insert(ITableInfo table)
        {
            if (table.IsView)
            {
                throw new Exception("Target does not support update operation");
            }
            StringBuilder field = new StringBuilder();
            StringBuilder value = new StringBuilder();
            foreach (var col in table.Colunms)
            {
                if (col.IsIdentity) { continue; }
                field.AppendFormat("{0},", GetColunmName(col.Name));
                value.AppendFormat("@{0},", col.Name);
            }
            field.Remove(field.Length - 1, 1);
            value.Remove(value.Length - 1, 1);

            return String.Format("INSERT INTO {0}({1}) VALUES({2})", GetTableName(table), field, value);
        }
        public SqlInfo Insert<DbEntity>(DbEntity source) where DbEntity : class, new()
        {
            var tableInfo = this.GetTable<DbEntity>();
            if (tableInfo.IsView)
            {
                throw new Exception("Target does not support insert operation");
            }
            var ps = GetPropertyInfos<DbEntity>();
            SqlInfo reval = new SqlInfo(tableInfo.Colunms.Count);
            StringBuilder field = new StringBuilder();
            StringBuilder value = new StringBuilder();
            int num = 0;
            foreach (var col in tableInfo.Colunms)
            {
                if (col.IsIdentity) { continue; }
                var p = ps.Find(o => string.Equals(o.Name, col.Name, StringComparison.OrdinalIgnoreCase));
                if (p == null) { continue; }
                field.AppendFormat("{0},", GetColunmName(col.Name));
                string pName = string.Format("@{0}", num);
                value.AppendFormat("{0},", pName);
                reval.Paras.Add(pName, p.GetValue(source, null));
                num++;
            }
            field.Remove(field.Length - 1, 1);
            value.Remove(value.Length - 1, 1);
            reval.Sql = String.Format("INSERT INTO {0}({1}) VALUES({2})", GetTableName(tableInfo), field, value);
            return reval;
        }
        public List<SqlInfo> Insert<DbEntity>(List<DbEntity> entities, int mergeLimit = 10) where DbEntity : class, new()
        {
            var tableInfo = this.GetTable<DbEntity>();
            if (tableInfo.IsView)
            {
                throw new Exception("Target does not support insert operation");
            }
            if (mergeLimit < 1) { mergeLimit = 1; }
            int lisCount = entities.Count;
            List<SqlInfo> reval = new List<SqlInfo>();
            if (lisCount < 1) { return reval; }
            var properties = GetPropertyInfos<DbEntity>();
            
            StringBuilder field = new StringBuilder();
            List<IColumnInfo> cols = tableInfo.Colunms.FindAll(o=>o.IsIdentity == false);
            List<System.Reflection.PropertyInfo> ps= new List<System.Reflection.PropertyInfo>();
            for(int i = cols.Count - 1; i > -1; i--)
            {
                var p = properties.Find(o => string.Equals(o.Name, cols[i].Name, StringComparison.OrdinalIgnoreCase));
                if (p == null) {
                    cols.RemoveAt(i);
                    continue;
                }
                ps.Add(p);
                field.AppendFormat("{0},", GetColunmName(cols[i].Name));
            }
            if (cols.Count < 1)
            {
                throw new Exception("It is not mapped to any column.");
            }
            field.Remove(field.Length - 1, 1);
            int colCount = cols.Count;
            int rowIndex = 0;
            string insertPart = string.Format("INSERT INTO {0}({1}) VALUES", GetTableName(tableInfo), field);
            StringBuilder valuePart = new StringBuilder();
            SqlInfo sql = new SqlInfo(mergeLimit * colCount);
            int colIndex = 0;
            for (int i = 0; i < lisCount; i++)
            {
                valuePart.Append("(");
                for (int j = 0; j < colCount; j++)
                {
                    valuePart.AppendFormat("@{0},", colIndex);
                    sql.Paras.Add(string.Format("@{0}", colIndex), ps[j].GetValue(entities[i], null));
                    colIndex++;
                }
                valuePart.Remove(valuePart.Length - 1, 1);
                valuePart.Append("),");

                rowIndex++;
                if (rowIndex < mergeLimit)
                {
                    if (i + 1 < lisCount)
                    {
                        continue;
                    }
                }
                valuePart.Remove(valuePart.Length - 1, 1);
                valuePart.Append(";");
                sql.Sql=string.Format("{0}{1}", insertPart, valuePart);
                reval.Add(sql);
                valuePart.Clear();
                rowIndex = 0;
                colIndex = 0;
                sql = new SqlInfo(mergeLimit * colCount);
            }
            return reval;
        }
        public SqlInfo InsertByCustomEntity<DbEntity>(object source) where DbEntity : class, new()
        {
            Type type = typeof(DbEntity);
            return InsertByCustomEntity(GetTable(type.FullName), type, source);
        }
        public SqlInfo InsertByCustomEntity(Type entityType, object source)
        {
            return InsertByCustomEntity(GetTable(entityType.FullName), entityType, source);
        }
        public SqlInfo InsertByCustomEntity(ITableInfo table, Type entityType, object source)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            if (table.IsView)
            {
                throw new Exception("Target does not support insert operation");
            }
            var ps = GetPropertyInfos(entityType);
            var dic = new Dictionary<System.Reflection.PropertyInfo, IColumnInfo>();
            for (int i = 0; i < ps.Count; i++)
            {
                var col = table.Colunms.Find(o => string.Equals(o.Name, ps[i].Name, StringComparison.OrdinalIgnoreCase));
                if (col == null)
                {
                    //throw new Exception("The " + ps[i].Name + " field does not exist in the target table");
                    continue;
                }
                if (col.IsIdentity)
                {
                    continue;
                }
                dic.Add(ps[i], col);
            }
            if (dic.Count < 0)
            {
                throw new Exception("Input source is missing available insert field");
            }
            SqlInfo reval = new SqlInfo(dic.Count);
            StringBuilder field = new StringBuilder();
            StringBuilder value = new StringBuilder();
            foreach (var key in dic.Keys)
            {
                var col = dic[key];
                field.AppendFormat("{0},", GetColunmName(col.Name));
                value.AppendFormat("@{0},", col.Name);
                reval.Paras.Add(col.Name, key.GetValue(source, null));
            }
            field.Remove(field.Length - 1, 1);
            value.Remove(value.Length - 1, 1);
            reval.Sql = String.Format("INSERT INTO {0}({1}) VALUES({2})", GetTableName(table), field, value);

            return reval;
        }
        public SqlInfo InsertByDictionary<DbEntity>(Dictionary<string, object> source) where DbEntity : class, new()
        {
            return InsertByDictionary(typeof(DbEntity), source);
        }
        public SqlInfo InsertByDictionary(Type entityType, Dictionary<string, object> source)
        {
            return InsertByDictionary(GetTable(entityType.FullName), source);
        }
        public SqlInfo InsertByDictionary(ITableInfo table, Dictionary<string, object> source)
        {
            if (source == null || source.Count < 1)
            {
                throw new ArgumentNullException("source");
            }
            if (table.IsView)
            {
                throw new Exception("Target does not support insert operation");
            }
            var cols = new List<IColumnInfo>();
            var values = new List<object>();
            foreach (var key in source.Keys)
            {
                var col = table.Colunms.Find(o => string.Equals(o.Name, key));
                if (col == null)
                {
                    //throw new Exception("The " + key + " field does not exist in the target table");
                    continue;
                }
                if (col.IsIdentity)
                {
                    continue;
                }
                values.Add(source[key]);
                cols.Add(col);
            }
            if (cols.Count < 0)
            {
                throw new Exception("Input source is missing available insert field");
            }
            SqlInfo reval = new SqlInfo(values.Count);
            StringBuilder field = new StringBuilder();
            StringBuilder value = new StringBuilder();
            for (int i = 0; i < values.Count; i++)
            {
                var col = cols[i];
                field.AppendFormat("{0},", GetColunmName(col.Name));
                value.AppendFormat("@{0},", col.Name);
                reval.Paras.Add(col.Name, values[i]);
            }
            field.Remove(field.Length - 1, 1);
            value.Remove(value.Length - 1, 1);
            reval.Sql = String.Format("INSERT INTO {0}({1}) VALUES({2})", GetTableName(table), field, value);

            return reval;
        }
        public SqlInfo InsertByNameValueCollection<DbEntity>(NameValueCollection source) where DbEntity : class, new()
        {
            Type type = typeof(DbEntity);
            return InsertByNameValueCollection(GetTable(type.FullName), source);
        }
        public SqlInfo InsertByNameValueCollection(Type entityType, NameValueCollection source)
        {
            return InsertByNameValueCollection(GetTable(entityType.FullName), source);
        }
        public SqlInfo InsertByNameValueCollection(ITableInfo table, NameValueCollection source)
        {
            if (source == null || source.Count < 1) { throw new ArgumentException("nvc"); }
            if (table.IsView)
            {
                throw new Exception("Target does not support insert operation");
            }
            var cols = new List<IColumnInfo>();
            var values = new List<object>();
            foreach (var key in source.AllKeys)
            {
                var col = table.Colunms.Find(o => string.Equals(o.Name, key));
                if (col == null)
                {
                    //throw new Exception("The " + key + " field does not exist in the target table");
                    continue;
                }
                if (col.IsIdentity)
                {
                    continue;
                }
                object val;
                if(!ValueConvert.StrToTargetType(source[key], col.Type, out val))
                {
                    throw new Exception("Cannot convert to target type");
                }
                values.Add(val);
                cols.Add(col);
            }
            if (cols.Count < 0)
            {
                throw new Exception("Input source is missing available insert field");
            }
            SqlInfo reval = new SqlInfo(values.Count);
            StringBuilder field = new StringBuilder();
            StringBuilder value = new StringBuilder();
            for (int i = 0; i < values.Count; i++)
            {
                var col = cols[i];
                field.AppendFormat("{0},", GetColunmName(col.Name));
                value.AppendFormat("@{0},", col.Name);
                reval.Paras.Add(col.Name, values[i]);
            }
            field.Remove(field.Length - 1, 1);
            value.Remove(value.Length - 1, 1);
            reval.Sql = String.Format("INSERT INTO {0}({1}) VALUES({2})", GetTableName(table), field, value);

            return reval;
        }

        public string Update<DbEntity>() where DbEntity : class, new()
        {
            return Update(GetTable<DbEntity>());
        }
        public string Update(Type entityType)
        {
            return Update(GetTable(entityType.FullName));
        }
        public string Update(ITableInfo table)
        {
            if (table.IsView)
            {
                throw new Exception("Target does not support update operation");
            }
            StringBuilder set = new StringBuilder();
            StringBuilder where = new StringBuilder();
            foreach (var col in table.Colunms)
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

            return String.Format("UPDATE {0} SET {1} WHERE {2}", GetTableName(table), set, where);
        }
        public SqlInfo Update<DbEntity>(DbEntity entity) where DbEntity : class, new()
        {
            return Update<DbEntity>(entity, "");
        }
        public SqlInfo Update<DbEntity>(DbEntity entity, string appendWhere, params object[] paras) where DbEntity : class, new()
        {
            return UpdateByCustomEntity(typeof(DbEntity), entity, appendWhere, paras);
        }
        public SqlInfo UpdateByCustomEntity<DbEntity>(object source) where DbEntity : class, new()
        {
            return UpdateByCustomEntity<DbEntity>(source, "");
        }
        public SqlInfo UpdateByCustomEntity<DbEntity>(object source, string appendWhere, params object[] paras) where DbEntity : class, new()
        {
            return UpdateByCustomEntity(GetTable<DbEntity>(), source, appendWhere, paras);
        }
        public SqlInfo UpdateByCustomEntity(Type entityType, object source, string appendWhere, params object[] paras)
        {
            return UpdateByCustomEntity(GetTable(entityType.FullName), source, appendWhere, paras);
        }
        public SqlInfo UpdateByCustomEntity(ITableInfo table, object source, string appendWhere, params object[] paras)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            if (table.IsView)
            {
                throw new Exception("Target does not support update operation");
            }
            var pKeys = table.Colunms.FindAll(o => o.IsPrimaryKey);
            bool hasAppendWhere = !string.IsNullOrEmpty(appendWhere);
            if (pKeys.Count < 0)
            {
                if (!hasAppendWhere)
                {
                    throw new ArgumentNullException("The target table is missing a primary key");
                }
            }
            var ps = GetPropertyInfos(source.GetType());
            var dic = new Dictionary<System.Reflection.PropertyInfo, IColumnInfo>(ps.Count);
            int keyCount = 0;
            for (int i = 0; i < ps.Count; i++)
            {
                var col = table.Colunms.Find(o => string.Equals(o.Name, ps[i].Name, StringComparison.OrdinalIgnoreCase));
                if (col == null)
                {
                    //throw new Exception("The " + ps[i].Name + " field does not exist in the target table");
                    continue;
                }
                if (pKeys.Contains(col))
                {
                    keyCount++;
                }
                dic.Add(ps[i], col);
            }
            if (!hasAppendWhere)
            {
                if (keyCount < 1)
                {
                    throw new Exception("No primary key field provided");
                }
                if (keyCount == ps.Count)
                {
                    throw new Exception("Missing field that needs to be updated");
                }
            }

            SqlInfo reval = new SqlInfo(dic.Count + paras.Length);
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
                reval.Paras.Add(col.Name, key.GetValue(source, null));
            }
            if (set.Length < 1)
            {
                throw new Exception("Missing field that needs to be updated");
            }
            set.Remove(set.Length - 1, 1);
            bool hasPrimaryKeyWhere = where.Length > 0;
            if (hasPrimaryKeyWhere)
            {
                where.Remove(where.Length - 5, 5);
            }
            if (hasAppendWhere)
            {
                if (hasPrimaryKeyWhere)
                {
                    where.AppendFormat(" AND {0}", appendWhere);
                }
                else
                {
                    where.Append(appendWhere);
                }
                SqlInfoUseParas(ref reval, paras);
            }
            reval.Sql = String.Format("UPDATE {0} SET {1} WHERE {2}", GetTableName(table), set, where);

            return reval;
        }
        public SqlInfo UpdateByDictionary<DbEntity>(Dictionary<string,object> source) where DbEntity : class, new()
        {
            return UpdateByDictionary<DbEntity>(source, "");
        }
        public SqlInfo UpdateByDictionary<DbEntity>(Dictionary<string, object> source, string appendWhere, params object[] paras) where DbEntity : class, new()
        {
            return UpdateByDictionary(GetTable<DbEntity>(), source, appendWhere, paras);
        }
        public SqlInfo UpdateByDictionary(Type entityType, Dictionary<string, object> source, string appendWhere, params object[] paras)
        {
            return UpdateByDictionary(GetTable(entityType.FullName), source, appendWhere, paras);
        }
        public SqlInfo UpdateByDictionary(ITableInfo table, Dictionary<string, object> source, string appendWhere, params object[] paras)
        {
            if (source == null || source.Count < 1)
            {
                throw new ArgumentNullException("source");
            }
            if (table.IsView)
            {
                throw new Exception("Target does not support update operation");
            }
            var pKeys = table.Colunms.FindAll(o => o.IsPrimaryKey);
            bool hasAppendWhere = !string.IsNullOrEmpty(appendWhere);
            if (pKeys.Count < 0)
            {
                if (!hasAppendWhere)
                {
                    throw new ArgumentNullException("The target table is missing a primary key");
                }
            }
            List<object> values = new List<object>(source.Count);
            List<IColumnInfo> cols = new List<IColumnInfo>();
            int keyCount = 0;
            foreach (var key in source.Keys)
            {
                var col = table.Colunms.Find(o => string.Equals(key, o.Name, StringComparison.OrdinalIgnoreCase));
                if (col == null)
                {
                    //throw new Exception("The " + key + " field does not exist in the target table");
                    continue;
                }
                if (col.IsPrimaryKey)
                {
                    keyCount++;
                }
                values.Add(source[key]);
                cols.Add(col);
            }
            if (!hasAppendWhere)
            {
                if (keyCount < 1)
                {
                    throw new Exception("No primary key field provided");
                }
                if (keyCount == source.Count)
                {
                    throw new Exception("Missing field that needs to be updated");
                }
            }

            SqlInfo reval = new SqlInfo(values.Count + paras.Length);
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
            if (set.Length < 1)
            {
                throw new Exception("Missing field that needs to be updated");
            }
            set.Remove(set.Length - 1, 1);
            bool hasPrimaryKeyWhere = where.Length > 0;
            if (hasPrimaryKeyWhere)
            {
                where.Remove(where.Length - 5, 5);
            }
            if (hasAppendWhere)
            {
                if (hasPrimaryKeyWhere)
                {
                    where.AppendFormat(" AND {0}", appendWhere);
                }
                else
                {
                    where.Append(appendWhere);
                }
                SqlInfoUseParas(ref reval, paras);
            }
            reval.Sql = String.Format("UPDATE {0} SET {1} WHERE {2}", GetTableName(table), set, where);

            return reval;
        }
        public SqlInfo UpdateByNameValueCollection<DbEntity>(NameValueCollection source) where DbEntity : class, new()
        {
            return UpdateByNameValueCollection<DbEntity>(source, "");
        }
        public SqlInfo UpdateByNameValueCollection<DbEntity>(NameValueCollection source, string appendWhere, params object[] paras) where DbEntity : class, new()
        {
            Type type = typeof(DbEntity);
            return UpdateByNameValueCollection(GetTable(type.FullName), source, appendWhere, paras);
        }
        public SqlInfo UpdateByNameValueCollection(Type entityType, NameValueCollection source, string appendWhere, params object[] paras)
        {
            return UpdateByNameValueCollection(GetTable(entityType.FullName), source, appendWhere, paras);
        }
        public SqlInfo UpdateByNameValueCollection(ITableInfo table, NameValueCollection source, string appendWhere, params object[] paras)
        {
            if (source == null || source.Count < 1) { throw new ArgumentException("source"); }

            if (table.IsView)
            {
                throw new Exception("Target does not support update operation");
            }
            var pKeys = table.Colunms.FindAll(o => o.IsPrimaryKey);
            bool hasAppendWhere = !string.IsNullOrEmpty(appendWhere);
            if (pKeys.Count < 0)
            {
                if (!hasAppendWhere)
                {
                    throw new ArgumentNullException("The target table is missing a primary key");
                }
            }

            var cols = new List<IColumnInfo>();
            var values = new List<object>();
            int keyCount = 0;
            foreach (var key in source.AllKeys)
            {
                var col = table.Colunms.Find(o => string.Equals(o.Name, key, StringComparison.OrdinalIgnoreCase));
                if (col == null)
                {
                    //throw new Exception("The " + key + " field does not exist in the target table");
                    continue;
                }
                if (col.IsPrimaryKey)
                {
                    keyCount++;
                }
                object val;
                if(!ValueConvert.StrToTargetType(source[key], col.Type, out val))
                {
                    throw new Exception("Cannot convert to target type(" + col.Type + ")");
                }
                values.Add(val);
                cols.Add(col);
            }
            if (!hasAppendWhere)
            {
                if (keyCount < 1)
                {
                    throw new Exception("No primary key field provided");
                }
                if (keyCount == source.Count)
                {
                    throw new Exception("Missing field that needs to be updated");
                }
            }

            SqlInfo reval = new SqlInfo(values.Count + paras.Length);
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
            if (set.Length < 1)
            {
                throw new Exception("Missing field that needs to be updated");
            }
            set.Remove(set.Length - 1, 1);
            bool hasPrimaryKeyWhere = where.Length > 0;
            if (hasPrimaryKeyWhere)
            {
                where.Remove(where.Length - 5, 5);
            }
            if (hasAppendWhere)
            {
                if (hasPrimaryKeyWhere)
                {
                    where.AppendFormat(" AND {0}", appendWhere);
                }
                else
                {
                    where.Append(appendWhere);
                }
                SqlInfoUseParas(ref reval, paras);
            }
            reval.Sql = String.Format("UPDATE {0} SET {1} WHERE {2}", GetTableName(table), set, where);

            return reval;
        }

        public SqlInfo Delete<DbEntity>(DbEntity source) where DbEntity : class, new()
        {
            if (source == null) { throw new ArgumentException("source"); }

            var type = typeof(DbEntity);
            var table = GetTable(type.FullName);
            if (table.IsView)
            {
                throw new Exception("Target does not support update operation");
            }
            var pKeys = table.Colunms.FindAll(o => o.IsPrimaryKey);
            if (pKeys.Count < 1)
            {
                throw new ArgumentNullException("The target table is missing a primary key");
            }
            var ps = GetPropertyInfos(type);
            var dic = new Dictionary<System.Reflection.PropertyInfo, IColumnInfo>(pKeys.Count);
            foreach(var p in pKeys)
            {
                var t = ps.Find(o => string.Equals(o.Name, p.Name, StringComparison.OrdinalIgnoreCase));
                if (t == null)
                {
                    throw new Exception("No primary key field entered");
                }
                dic.Add(t, p);
            }
            SqlInfo reval = new SqlInfo(dic.Count);
            StringBuilder where = new StringBuilder();
            foreach (var p in dic.Keys)
            {
                var col = dic[p];
                where.AppendFormat("{0}=@{1} AND ", GetColunmName(col.Name), col.Name);
                reval.Paras.Add(col.Name, p.GetValue(source, null));
            }
            where.Remove(where.Length - 5, 5);
            reval.Sql = string.Format("DELETE FROM {0} WHERE {1}", GetTableName(table), where);

            return reval;
        }
        public SqlInfo Delete<DbEntity>(string where, params object[] paras) where DbEntity : class, new()
        {
            return Delete(typeof(DbEntity), where, paras);
        }
        public SqlInfo Delete(Type entityType, string where, params object[] paras)
        {
            return Delete(GetTable(entityType.FullName), where, paras);
        }
        public SqlInfo Delete(ITableInfo table, string where, params object[] paras)
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
            if (table.IsView)
            {
                throw new Exception("Does not support Delete operation on the view");
            }
            SqlInfo reval = new SqlInfo(paras.Length);
            reval.Sql = string.Format("DELETE FROM {0} WHERE {1}", GetTableName(table), where);
            SqlInfoUseParas(ref reval, paras);
            return reval;
        }
        public SqlInfo DeleteByPrimaryKey<DbEntity>(object key) where DbEntity : class, new()
        {
            return DeleteByPrimaryKey(typeof(DbEntity), key);
        }
        public SqlInfo DeleteByPrimaryKey(Type entityType, object key)
        {
            return DeleteByPrimaryKey(GetTable(entityType.FullName), key);
        }
        public SqlInfo DeleteByPrimaryKey(ITableInfo table, object key)
        {
            if (key == null) { throw new ArgumentNullException("key"); }

            var pKeys = table.Colunms.FindAll(o => o.IsPrimaryKey);
            if (pKeys.Count < 1)
            {
                throw new ArgumentNullException("The target table is missing a primary key");
            }
            var dic = new Dictionary<IColumnInfo, object>(pKeys.Count);
            var type = key.GetType();
            if (pKeys.Count > 1)
            {
                var ps = GetPropertyInfos(type);
                foreach (var k in pKeys)
                {
                    var p = ps.Find(o => string.Equals(o.Name, k.Name, StringComparison.OrdinalIgnoreCase));
                    if (p == null)
                    {
                        throw new ArgumentNullException("Parameter '" + key + "' must have property '" + k.Name + "'");
                    }
                    if (!string.Equals(p.PropertyType.Name, k.Type))
                    {
                        throw new ArgumentException("Property '" + p.Name + "' type error");
                    }
                    var val = p.GetValue(key, null);
                    dic.Add(k, val);
                }
            }
            else
            {
                if (!string.Equals(type.Name, pKeys[0].Type, StringComparison.OrdinalIgnoreCase))
                {
                    throw new ArgumentNullException("Wrong data type for parameter");
                }
                dic.Add(pKeys[0], key);
            }
            SqlInfo reval = new SqlInfo(dic.Count);
            StringBuilder where = new StringBuilder();
            foreach (var item in dic.Keys)
            {
                where.AppendFormat("{0}=@{1} AND ", GetColunmName(item.Name), item.Name);
                reval.Paras.Add(item.Name, dic[item]);
            }
            where.Remove(where.Length - 5, 5);
            reval.Sql = string.Format("DELETE FROM {0} WHERE {1}", GetTableName(table), where);
            return reval;
        }
        public SqlInfo DeleteByCustomEntity<DbEntity>(object source) where DbEntity : class, new()
        {
            return DeleteByCustomEntity(typeof(DbEntity), source);
        }
        public SqlInfo DeleteByCustomEntity(Type entityType, object source)
        {
            return DeleteByCustomEntity(GetTable(entityType.FullName), source);
        }
        public SqlInfo DeleteByCustomEntity(ITableInfo table, object source)
        {
            if (source == null) { throw new ArgumentException("source"); }

            if (table.IsView)
            {
                throw new Exception("Target does not support update operation");
            }
            var ps = GetPropertyInfos(source.GetType());
            var dic = new Dictionary<System.Reflection.PropertyInfo, IColumnInfo>(ps.Count);
            for (int i = 0; i < ps.Count; i++)
            {
                var col = table.Colunms.Find(o => string.Equals(o.Name, ps[i].Name, StringComparison.OrdinalIgnoreCase));
                if (col == null)
                {
                    //throw new Exception("The " + ps[i].Name + " field does not exist in the target table");
                    continue;
                }
                dic.Add(ps[i], col);
            }
            if (dic.Count < 1)
            {
                throw new Exception("Missing condition field");
            }
            SqlInfo reval = new SqlInfo(dic.Count);
            StringBuilder where = new StringBuilder();
            foreach (var p in dic.Keys)
            {
                var col = dic[p];
                where.AppendFormat("{0}=@{1} AND ", GetColunmName(col.Name), col.Name);
                reval.Paras.Add(col.Name, p.GetValue(source,null));
            }
            where.Remove(where.Length - 5, 5);
            reval.Sql = string.Format("DELETE FROM {0} WHERE {1}", GetTableName(table), where);

            return reval;
        }
        public SqlInfo DeleteByDictionary<DbEntity>(Dictionary<string, object> source) where DbEntity : class, new()
        {
            return DeleteByDictionary(typeof(DbEntity), source);
        }
        public SqlInfo DeleteByDictionary(Type entityType, Dictionary<string, object> source)
        {
            return DeleteByDictionary(GetTable(entityType.FullName), source);
        }
        public SqlInfo DeleteByDictionary(ITableInfo table, Dictionary<string, object> source)
        {
            if (source == null || source.Count < 1) { throw new ArgumentException("source"); }

            if (table.IsView)
            {
                throw new Exception("Target does not support update operation");
            }

            var cols = new List<IColumnInfo>();
            var values = new List<object>();
            foreach (var key in source.Keys)
            {
                var col = table.Colunms.Find(o => string.Equals(o.Name, key, StringComparison.OrdinalIgnoreCase));
                if (col == null)
                {
                    //throw new Exception("The " + key + " field does not exist in the target table");
                    continue;
                }
                values.Add(source[key]);
                cols.Add(col);
            }
            if (cols.Count < 0)
            {
                throw new Exception("Missing condition field");
            }
            SqlInfo reval = new SqlInfo(values.Count);
            StringBuilder where = new StringBuilder();
            for (int i = 0; i < cols.Count; i++)
            {
                where.AppendFormat("{0}=@{1} AND ", GetColunmName(cols[i].Name), cols[i].Name);
                reval.Paras.Add(cols[i].Name, values[i]);
            }
            where.Remove(where.Length - 5, 5);
            reval.Sql = string.Format("DELETE FROM {0} WHERE {1}", GetTableName(table), where);

            return reval;
        }
        public SqlInfo DeleteByNameValueCollection<DbEntity>(NameValueCollection source) where DbEntity : class, new()
        {
            return DeleteByNameValueCollection(typeof(DbEntity), source);
        }
        public SqlInfo DeleteByNameValueCollection(Type entityType, NameValueCollection source)
        {
            return DeleteByNameValueCollection(GetTable(entityType.FullName), source);
        }
        public SqlInfo DeleteByNameValueCollection(ITableInfo table, NameValueCollection source)
        {
            if (source == null || source.Count < 1) { throw new ArgumentException("source"); }

            if (table.IsView)
            {
                throw new Exception("Target does not support update operation");
            }

            var cols = new List<IColumnInfo>();
            var values = new List<object>();
            foreach (var key in source.AllKeys)
            {
                var col = table.Colunms.Find(o => string.Equals(o.Name, key, StringComparison.OrdinalIgnoreCase));
                if(col== null)
                {
                    //throw new Exception("The " + key + " field does not exist in the target table");
                    continue;
                }
                object val;
                if (!ValueConvert.StrToTargetType(source[key], col.Type, out val))
                {
                    throw new Exception("Cannot convert to target type");
                }
                values.Add(val);
                cols.Add(col);
            }
            if(cols.Count < 0)
            {
                throw new Exception("Missing condition field");
            }
            SqlInfo reval = new SqlInfo(values.Count);
            StringBuilder where = new StringBuilder();
            for (int i = 0; i < cols.Count; i++)
            {
                where.AppendFormat("{0}=@{1} AND ", GetColunmName(cols[i].Name), cols[i].Name);
                reval.Paras.Add(cols[i].Name, values[i]);
            }
            where.Remove(where.Length - 5, 5);
            reval.Sql = string.Format("DELETE FROM {0} WHERE {1}", GetTableName(table), where);

            return reval;
        }

        public SqlInfo RawSql(string sql, params object[] paras)
        {
            SqlInfo reval = new SqlInfo(paras.Length);
            reval.Sql = sql;
            SqlInfoUseParas(ref reval, paras);
            return reval;
        }

        public PageQuery PageQuery()
        {
            return new PageQuery();
        }
        public ListQuery ListQuery()
        {
            return new ListQuery();
        }
        public PageQuery PageQuery(NameValueCollection queryNVC)
        {
            PageQuery query = new PageQuery();
            string page = queryNVC["page"];
            if (!string.IsNullOrEmpty(page))
            {
                int _page;
                if (int.TryParse(page, out _page) && _page > 0)
                {
                    query.Page = _page;
                }
            }
            string size = queryNVC["size"];
            if (!string.IsNullOrEmpty(page))
            {
                int _size;
                if (int.TryParse(size, out _size) && _size > 0)
                {
                    query.Size = _size;
                }
            }
            string where = queryNVC["where"];
            if (!string.IsNullOrEmpty(where))
            {
                query.Where = where;
            }
            string orderby = queryNVC["orderby"];
            if (!string.IsNullOrEmpty(orderby))
            {
                query.Orderby = orderby;
            }
            string unique = queryNVC["unique"];
            if (!string.IsNullOrEmpty(unique))
            {
                if (!System.Text.RegularExpressions.Regex.IsMatch(unique, @"^[_a-zA-Z]{1,64}$"))
                {
                    throw new Exception("unique parameter error");
                }
                query.Unique = unique;
            }
            if (query.Size < 1)
            {
                query.Size = 10;
            }
            if (query.Size > 1000)
            {
                query.Size = 1000;
            }
            query.UseFields(GetFields(queryNVC));
            if (!string.IsNullOrEmpty(query.Where))
            {
                object[] paras = GetParas(queryNVC);
                query.UseParas(paras);
            }
            return query;
        }
        public ListQuery ListQuery(NameValueCollection queryNVC)
        {
            ListQuery query = new ListQuery();
            string where = queryNVC["where"];
            if (!string.IsNullOrEmpty(where))
            {
                query.Where = where;
            }
            string orderby = queryNVC["orderby"];
            if (!string.IsNullOrEmpty(orderby))
            {
                query.Orderby = orderby;
            }
            string top = queryNVC["top"];
            if (!string.IsNullOrEmpty(top))
            {
                int _top;
                if (int.TryParse(top, out _top) && _top > 0)
                {
                    query.Top = _top;
                }
            }
            if (query.Top < 1)
            {
                query.Top = 10;
            }
            if (query.Top > 1000)
            {
                query.Top = 1000;
            }
            query.UseFields(GetFields(queryNVC));
            if (!string.IsNullOrEmpty(query.Where))
            {
                object[] paras = GetParas(queryNVC);
                query.UseParas(paras);
            }
            return query;
        }
        public object SqlParaParse(string value)
        {
            if (value.Length < 1) { return value; }
            if (value[0] != '(') { return value; }
            int index = value.IndexOf(')');
            if (index < 1 || index == value.Length - 1) { return value; }
            string s1 = value.Substring(0 + 1, index - 1).ToLower();
            string s2 = value.Substring(index + 1);
            object obj;
            if (ValueConvert.StrToTargetType(s2, s1, out obj))
            {
                return obj;
            }
            throw new Exception("failed to parsing \"" + value+ "\"");
        }
        public object[] SqlParaParse(string[] values)
        {
            if(values == null || values.Length < 1)
            {
                return new object[0];
            }
            List<object> reval = new List<object>();
            foreach(string s in values)
            {
                reval.Add(SqlParaParse(s));
            }
            return reval.ToArray();
        }
        private string[] GetFields(NameValueCollection nvc)
        {
            string fields = nvc["fields"];
            if (string.IsNullOrEmpty(fields))
            {
                return new string[0];
            }
            return fields.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Distinct().ToArray();
        }
        private object[] GetParas(NameValueCollection nvc)
        {
            string paras = nvc["paras"];
            if (string.IsNullOrEmpty(paras))
            {
                return new object[0];
            }
            return SqlParaParse(paras.Split(','));
        }

    }
}
