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
            throw new Exception("类型" + typeof(DbEntity).FullName + "没有映射到" + ZeroDb.Database.Key + "上");
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

        public virtual SqlInfo Count<DbEntity>(string where, params object[] paras) where DbEntity : class, new()
        {
            var tableInfo = this.GetTable<DbEntity>();
            SqlInfo reval = new SqlInfo();
            reval.Sql = string.Format("SELECT COUNT(1) FROM {0} WHERE {1}", GetTableName(tableInfo), string.IsNullOrEmpty(where) ? "1>0" : where);
            int n = 0;
            int m = paras.Length;
            while (n < m)
            {
                reval.Paras.Add(n.ToString(), paras[n]);
                n++;
            }
            return reval;
        }

        public virtual SqlInfo Page<DbEntity>(long page, long size, string where, string orderby, string[] fields, string uniqueField = "", params object[] paras) where DbEntity : class, new()
        {
            var tableInfo = this.ZeroDb.GetTable<DbEntity>();

            page = page < 1 ? 1 : page;
            size = size < 1 ? 1 : size;

            long startIndex = page * size - size + 1;
            long endIndex = page * size;
            StringBuilder sql = new StringBuilder();
            StringBuilder fieldStr = new StringBuilder();
            if (fields == null)
            {
                fields = tableInfo.Colunms.FindAll(o => o.MaxLength < 1000).Select(o => o.Name).ToArray();
            }
            for (int i = 0; i < fields.Length; i++)
            {
                if (!string.IsNullOrEmpty(fields[i]))
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
                var ts = GetUniqueFieldName(tableInfo);
                uniqueField = ts.Length == 1 ? ts[0] : string.Empty;
            }
            if (!string.IsNullOrEmpty(uniqueField))//具有唯一性字段
            {
                string resultFieldName = GetColunmName(uniqueField);
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
            SqlInfo reval = new SqlInfo();
            reval.Sql = sql.ToString();
            int n = 0;
            int m = paras.Length;
            while (n < m)
            {
                reval.Paras.Add(n.ToString(), paras[n]);
                n++;
            }
            return reval;
        }

        public SqlInfo Select<DbEntity>(ListQuery query) where DbEntity : class, new()
        {
            return Select<DbEntity>(query.Where, query.Orderby, query.Top, query.Fields, query.Paras);
        }
        public virtual SqlInfo Select<DbEntity>(string where, string orderby, int top, string[] fields, params object[] paras) where DbEntity : class, new()
        {
            SqlInfo reval = new SqlInfo();
            var tableInfo = this.GetTable<DbEntity>();
            string[] fieldArray = tableInfo.Colunms.Select(o => o.Name).ToArray();
            if (fields != null && fields.Length > 0)
            {
                int i = 0;
                List<string> temp = new List<string>();
                while (i < fields.Length)
                {
                    int j = 0;
                    while (j < fieldArray.Length)
                    {
                        if (string.Equals(fields[i], fieldArray[j], StringComparison.OrdinalIgnoreCase))
                        {
                            temp.Add(fieldArray[j]);
                            break;
                        }
                        j++;
                    }
                    i++;
                }
                fields = temp.Distinct().ToArray();
                if (fields.Length > 0)
                {
                    fieldArray = fields;
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
                    reval.Sql = string.Format("SELECT TOP {0} {1} FROM {2} WHERE {3}", top, field, tableName, where);
                }
                else
                {
                    reval.Sql = string.Format("SELECT TOP {0} {1} FROM {2} WHERE {3} ORDER BY {4}", top, field, tableName, where, orderby);
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
            var tableInfo = this.GetTable<DbEntity>();
            if (tableInfo.IsView)
            {
                throw new Exception("Target does not support update operation");
            }
            StringBuilder field = new StringBuilder();
            StringBuilder value = new StringBuilder();
            foreach (var col in tableInfo.Colunms)
            {
                if (col.IsIdentity) { continue; }
                field.AppendFormat("{0},", GetColunmName(col.Name));
                value.AppendFormat("@{0},", col.Name);
            }
            field.Remove(field.Length - 1, 1);
            value.Remove(value.Length - 1, 1);

            return String.Format("INSERT INTO {0}({1}) VALUES({2})", GetTableName(tableInfo), field, value);
        }
        public SqlInfo Insert<DbEntity>(DbEntity source) where DbEntity : class, new()
        {
            var tableInfo = this.GetTable<DbEntity>();
            if (tableInfo.IsView)
            {
                throw new Exception("Target does not support insert operation");
            }
            var ps = source.GetType().GetProperties().ToList();
            SqlInfo reval = new SqlInfo();
            StringBuilder field = new StringBuilder();
            StringBuilder value = new StringBuilder();
            foreach (var col in tableInfo.Colunms)
            {
                if (col.IsIdentity) { continue; }
                var p = ps.Find(o => string.Equals(o.Name, col.Name, StringComparison.OrdinalIgnoreCase));
                if (p == null) { continue; }
                field.AppendFormat("{0},", GetColunmName(col.Name));
                value.AppendFormat("@{0},", col.Name);
                reval.Paras.Add(col.Name, p.GetValue(source, null));
            }
            field.Remove(field.Length - 1, 1);
            value.Remove(value.Length - 1, 1);
            reval.Sql = String.Format("INSERT INTO {0}({1}) VALUES({2})", GetTableName(tableInfo), field, value);
            return reval;
        }
        public SqlInfo InsertFromCustomEntity<DbEntity>(object source) where DbEntity : class, new()
        {
            if (source == null)
            {
                throw new ArgumentNullException("entity");
            }
            var tableInfo = this.GetTable<DbEntity>();
            if (tableInfo.IsView)
            {
                throw new Exception("Target does not support insert operation");
            }
            var ps = source.GetType().GetProperties().ToList();
            var dic = new Dictionary<System.Reflection.PropertyInfo, DbDataColumnInfo>();
            for (int i = 0; i < ps.Count; i++)
            {
                var col = tableInfo.Colunms.Find(o => string.Equals(o.Name, ps[i].Name, StringComparison.OrdinalIgnoreCase));
                if (col == null)
                {
                    throw new Exception("The " + ps[i].Name + " field does not exist in the target table");
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
            SqlInfo reval = new SqlInfo();
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
            reval.Sql = String.Format("INSERT INTO {0}({1}) VALUES({2})", GetTableName(tableInfo), field, value);

            return reval;
        }
        public SqlInfo InsertFromDictionary<DbEntity>(Dictionary<string, object> source) where DbEntity : class, new()
        {
            if (source == null || source.Count < 1)
            {
                throw new ArgumentNullException("source");
            }
            var tableInfo = this.GetTable<DbEntity>();
            if (tableInfo.IsView)
            {
                throw new Exception("Target does not support insert operation");
            }
            var cols = new List<DbDataColumnInfo>();
            var values = new List<object>();
            foreach (var key in source.Keys)
            {
                var col = tableInfo.Colunms.Find(o => string.Equals(o.Name, key));
                if (col == null)
                {
                    throw new Exception("The " + key + " field does not exist in the target table");
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
            SqlInfo reval = new SqlInfo();
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
            reval.Sql = String.Format("INSERT INTO {0}({1}) VALUES({2})", GetTableName(tableInfo), field, value);

            return reval;
        }
        public SqlInfo InsertFromNameValueCollection<DbEntity>(System.Collections.Specialized.NameValueCollection source) where DbEntity : class, new()
        {
            if (source == null || source.Count < 1) { throw new ArgumentException("nvc"); }
            var tableInfo = this.GetTable<DbEntity>();
            if (tableInfo.IsView)
            {
                throw new Exception("Target does not support insert operation");
            }
            var ps = typeof(DbEntity).GetProperties().ToList();
            var cols = new List<DbDataColumnInfo>();
            var values = new List<object>();
            foreach (var key in source.AllKeys)
            {
                var col = tableInfo.Colunms.Find(o => string.Equals(o.Name, key));
                if (col == null)
                {
                    throw new Exception("The " + key + " field does not exist in the target table");
                }
                if (col.IsIdentity)
                {
                    continue;
                }
                var p = ps.Find(o => string.Equals(o.Name, key));
                if (p == null)
                {
                    throw new Exception("Don't know the data type of field " + key);
                }
                values.Add(ValueConvert.StrToTargetType(source[key], p.PropertyType));
                cols.Add(col);
            }
            if (cols.Count < 0)
            {
                throw new Exception("Input source is missing available insert field");
            }
            SqlInfo reval = new SqlInfo();
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
            reval.Sql = String.Format("INSERT INTO {0}({1}) VALUES({2})", GetTableName(tableInfo), field, value);

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

            return String.Format("UPDATE {0} SET {1} WHERE {2}", GetTableName(tableInfo), set, where);
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
        public SqlInfo UpdateFromCustomEntity<DbEntity>(object source) where DbEntity : class, new()
        {
            if (source == null)
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
            var ps = source.GetType().GetProperties().ToList();
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
                reval.Paras.Add(col.Name, key.GetValue(source, null));
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
        public SqlInfo UpdateFromNameValueCollection<DbEntity>(System.Collections.Specialized.NameValueCollection source) where DbEntity : class, new()
        {
            if (source == null || source.Count < 1) { throw new ArgumentException("source"); }

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
            foreach (var key in source.AllKeys)
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
                values.Add(ValueConvert.StrToTargetType(source[key], p.PropertyType));
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

        public SqlInfo Delete<DbEntity>(string where, params object[] paras) where DbEntity : class, new()
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

        public SqlInfo RawSql(string sql, params object[] paras)
        {
            SqlInfo reval = new SqlInfo();
            reval.Sql = sql;
            int n = 0;
            int m = paras.Length;
            while (n < m)
            {
                reval.Paras.Add(n.ToString(), paras[n]);
                n++;
            }
            return reval;
        }

    }
}
