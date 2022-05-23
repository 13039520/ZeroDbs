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
            throw new Exception("类型" + typeof(DbEntity).FullName + "没有映射到" + ZeroDb.Database.Key + "上");
        }
        public ITableInfo GetTable(string entityFullName)
        {
            var reval = this.db.GetTable(entityFullName);
            if (reval != null)
            {
                return reval;
            }
            throw new Exception("类型" + entityFullName + "没有映射到" + ZeroDb.Database.Key + "上");
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
            Type type = typeof(DbEntity);
            return InsertFromCustomEntity(GetTable(type.FullName), type, source);
        }
        public SqlInfo InsertFromCustomEntity(Type entityType, object source)
        {
            return InsertFromCustomEntity(GetTable(entityType.FullName), entityType, source);
        }
        public SqlInfo InsertFromCustomEntity(ITableInfo table, Type entityType, object source)
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
        public SqlInfo InsertFromDictionary<DbEntity>(Dictionary<string, object> source) where DbEntity : class, new()
        {
            return InsertFromDictionary(GetTable<DbEntity>(), source);
        }
        public SqlInfo InsertFromDictionary(ITableInfo table, Dictionary<string, object> source)
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
        public SqlInfo InsertFromNameValueCollection<DbEntity>(System.Collections.Specialized.NameValueCollection source) where DbEntity : class, new()
        {
            Type type = typeof(DbEntity);
            return InsertFromNameValueCollection(GetTable(type.FullName), type, source);
        }
        public SqlInfo InsertFromNameValueCollection(Type entityType, System.Collections.Specialized.NameValueCollection source)
        {
            return InsertFromNameValueCollection(GetTable(entityType.FullName), entityType, source);
        }
        public SqlInfo InsertFromNameValueCollection(ITableInfo table, Type entityType, System.Collections.Specialized.NameValueCollection source)
        {
            if (source == null || source.Count < 1) { throw new ArgumentException("nvc"); }
            if (table.IsView)
            {
                throw new Exception("Target does not support insert operation");
            }
            var ps = GetPropertyInfos(entityType);
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
            bool hasAppendWhere = !string.IsNullOrEmpty(appendWhere);
            if (pKeys.Count < 0)
            {
                if (!hasAppendWhere)
                {
                    throw new ArgumentNullException("The target table is missing a primary key");
                }
            }
            var ps = GetPropertyInfos<DbEntity>();
            var dic = new Dictionary<System.Reflection.PropertyInfo, IColumnInfo>();
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
            if (!hasAppendWhere)
            {
                if (keyCount < pKeys.Count)
                {
                    throw new Exception("Input source is missing fields other than primary key fields");
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
                reval.Paras.Add(col.Name, key.GetValue(entity, null));
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
            reval.Sql = String.Format("UPDATE {0} SET {1} WHERE {2}", GetTableName(tableInfo), set, where);

            return reval;
        }
        public SqlInfo UpdateFromCustomEntity<DbEntity>(object source) where DbEntity : class, new()
        {
            return UpdateFromCustomEntity<DbEntity>(source, "");
        }
        public SqlInfo UpdateFromCustomEntity<DbEntity>(object source, string appendWhere, params object[] paras) where DbEntity : class, new()
        {
            return UpdateFromCustomEntity(GetTable<DbEntity>(), source, appendWhere, paras);
        }
        public SqlInfo UpdateFromCustomEntity(Type entityType, object source, string appendWhere, params object[] paras)
        {
            return UpdateFromCustomEntity(GetTable(entityType.FullName), source, appendWhere, paras);
        }
        public SqlInfo UpdateFromCustomEntity(ITableInfo table, object source, string appendWhere, params object[] paras)
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
            var dic = new Dictionary<System.Reflection.PropertyInfo, IColumnInfo>();
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
        public SqlInfo UpdateFromDictionary<DbEntity>(Dictionary<string,object> source) where DbEntity : class, new()
        {
            return UpdateFromDictionary<DbEntity>(source, "");
        }
        public SqlInfo UpdateFromDictionary<DbEntity>(Dictionary<string, object> source, string appendWhere, params object[] paras) where DbEntity : class, new()
        {
            return UpdateFromDictionary(GetTable<DbEntity>(), source, appendWhere, paras);
        }
        public SqlInfo UpdateFromDictionary(Type entityType, Dictionary<string, object> source, string appendWhere, params object[] paras)
        {
            return UpdateFromDictionary(GetTable(entityType.FullName), source, appendWhere, paras);
        }
        public SqlInfo UpdateFromDictionary(ITableInfo table, Dictionary<string, object> source, string appendWhere, params object[] paras)
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

            var ps = GetPropertyInfos<DbEntity>();
            var cols = new List<IColumnInfo>();
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

            SqlInfo reval = new SqlInfo(values.Count);
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
        public SqlInfo UpdateFromNameValueCollection<DbEntity>(System.Collections.Specialized.NameValueCollection source, string appendWhere, params object[] paras) where DbEntity : class, new()
        {
            Type type = typeof(DbEntity);
            return UpdateFromNameValueCollection(GetTable(type.FullName), type, source, appendWhere, paras);
        }
        public SqlInfo UpdateFromNameValueCollection(Type entityType, System.Collections.Specialized.NameValueCollection source, string appendWhere, params object[] paras)
        {
            return UpdateFromNameValueCollection(GetTable(entityType.FullName), entityType, source, appendWhere, paras);
        }
        public SqlInfo UpdateFromNameValueCollection(ITableInfo table, Type entityType, System.Collections.Specialized.NameValueCollection source, string appendWhere, params object[] paras)
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

            var ps = GetPropertyInfos(entityType);
            var cols = new List<IColumnInfo>();
            var values = new List<object>();
            int keyCount = 0;
            foreach (var key in source.AllKeys)
            {
                var col = table.Colunms.Find(o => string.Equals(o.Name, key));
                if (col == null)
                {
                    //throw new Exception("The " + key + " field does not exist in the target table");
                    continue;
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

        public SqlInfo Delete<DbEntity>(string where, params object[] paras) where DbEntity : class, new()
        {
            return Delete(GetTable<DbEntity>(), where, paras);
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
        public PageQuery PageQuery(System.Collections.Specialized.NameValueCollection queryNVC)
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
        public ListQuery ListQuery(System.Collections.Specialized.NameValueCollection queryNVC)
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
            string error = "TryParse error of \"" + value.Replace("\"", "\\\"") + "\"";
            if (s1 == "int16" || s1 == "short")
            {
                short v1;
                if (!short.TryParse(s2, out v1))
                {
                    throw new Exception(error);
                }
                return v1;
            }
            if (s1 == "uint16" || s1 == "ushort")
            {
                ushort v1;
                if (!ushort.TryParse(s2, out v1))
                {
                    throw new Exception(error);
                }
                return v1;
            }
            if (s1 == "int" || s1 == "int32")
            {
                int v1;
                if (!int.TryParse(s2, out v1))
                {
                    throw new Exception(error);
                }
                return v1;
            }
            if (s1 == "u" || s1 == "uint" || s1 == "uint32")
            {
                uint v1;
                if (!uint.TryParse(s2, out v1))
                {
                    throw new Exception(error);
                }
                return v1;
            }
            if (s1 == "l" || s1 == "long" || s1 == "int64")
            {
                long v1;
                if (!long.TryParse(s2, out v1))
                {
                    throw new Exception(error);
                }
                return v1;
            }
            if (s1 == "ul" || s1 == "ulong" || s1 == "uint64")
            {
                ulong v1;
                if (!ulong.TryParse(s2, out v1))
                {
                    throw new Exception(error);
                }
                return v1;
            }
            if (s1 == "f" || s1 == "single" || s1 == "float")
            {
                float v1;
                if (!float.TryParse(s2, out v1))
                {
                    throw new Exception(error);
                }
                return v1;
            }
            if (s1 == "d" || s1 == "double")
            {
                double v1;
                if (!double.TryParse(s2, out v1))
                {
                    throw new Exception(error);
                }
                return v1;
            }
            if (s1 == "m" || s1 == "decimal")
            {
                decimal v1;
                if (!decimal.TryParse(s2, out v1))
                {
                    throw new Exception(error);
                }
                return v1;
            }
            if (s1 == "dt" || s1 == "datetime")
            {
                DateTime v1;
                if (!DateTime.TryParse(s2, out v1))
                {
                    throw new Exception(error);
                }
                return v1;
            }
            if (s1 == "guid")
            {
                Guid v1;
                if (!Guid.TryParse(s2, out v1))
                {
                    throw new Exception(error);
                }
                return v1;
            }
            if (s1 == "bool")
            {
                bool v1;
                if (!bool.TryParse(s2, out v1))
                {
                    throw new Exception(error);
                }
                return v1;
            }
            if (s1 == "byte")
            {
                byte v;
                if (!byte.TryParse(s2, out v))
                {
                    throw new Exception(error);
                }
                return v;
            }
            if (s1 == "sbyte")
            {
                sbyte v1;
                if (!sbyte.TryParse(s2, out v1))
                {
                    throw new Exception(error);
                }
                return v1;
            }

            throw new Exception("Unrecognized prefix \"" + s1 + "\"");
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
        private string[] GetFields(System.Collections.Specialized.NameValueCollection nvc)
        {
            string fields = nvc["fields"];
            if (string.IsNullOrEmpty(fields))
            {
                return new string[0];
            }
            return fields.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Distinct().ToArray();
        }
        private object[] GetParas(System.Collections.Specialized.NameValueCollection nvc)
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
