using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlTypes;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ZeroDbs
{
    internal class SqlServer : AbsDatabase, IDatabase
    {
        public override string DbType { get { return "SqlServer"; } }
        public SqlServer(string dbKey, string connectionString, ISnowflakeIdGenerator snowflake) : base(dbKey, connectionString, snowflake)
        {

        }
        
        public override void SetDbDataTypeMapping(string dbDataType, Type clrType)
        {
            if (!string.IsNullOrWhiteSpace(dbDataType) && clrType != null)
            {
                DbDataTypeMap.SetType(DbType, dbDataType, clrType);
            }
        }
        public override IDbDataParameter CreateParameter(string name, object value)
        {
#if NET45
            var param = new System.Data.SqlClient.SqlParameter();
            param.ParameterName = Param(name);
            if(value == null)
            {
                param.Value = DBNull.Value;
                return param;
            }
            if (value is ISqlParameter v)
            {
                param.Value = v.Value != null ? v.Value : DBNull.Value;
                param.Direction = v.Direction;
                return param;
            }
            param.Value = value;
            return param;
#else
            var param = new Microsoft.Data.SqlClient.SqlParameter();
            param.ParameterName = Param(name);
            if (value == null)
            {
                param.Value = DBNull.Value;
                return param;
            }
            if (value is ISqlParameter v)
            {
                param.Value = v.Value != null ? v.Value : DBNull.Value;
                param.Direction = v.Direction;
                return param;
            }
            param.Value = value;
            return param;
#endif
        }
        public override IDbDataParameter CreateParameter(int index, object value)
        {
#if NET45
            var param = new System.Data.SqlClient.SqlParameter();
            param.ParameterName = Param(index);
            if(value == null)
            {
                param.Value = DBNull.Value;
                return param;
            }
            if (value is ISqlParameter v)
            {
                param.Value = v.Value != null ? v.Value : DBNull.Value;
                param.Direction = v.Direction;
                return param;
            }
            param.Value = value;
            return param;
#else
            var param = new Microsoft.Data.SqlClient.SqlParameter();
            param.ParameterName = Param(index);
            if(value == null)
            {
                param.Value = DBNull.Value;
                return param;
            }
            if (value is ISqlParameter v)
            {
                param.Value = v.Value != null ? v.Value : DBNull.Value;
                param.Direction = v.Direction;
                return param;
            }
            param.Value = value;
            return param;
#endif
        }
        public override void UseDbConnection(DbConnectionHandler callback)
        {
#if NET45
            using (var conn = new System.Data.SqlClient.SqlConnection(ConnectionString))
            {
                conn.Open();
                callback(conn);
            }
#else
            using (var conn = new Microsoft.Data.SqlClient.SqlConnection(ConnectionString))
            {
                conn.Open();
                callback(conn);
            }
#endif
        }
        public override ITable GetTable(string tableName)
        {
            ITable table = null;
            UseDbCommand((cmd) => {
                var dbName = cmd.Connection.Database;
                Dictionary<string, string> identityDict = new Dictionary<string, string>();
                string sql = $"SELECT o.object_id,o.type,o.name,ep.value AS description FROM sys.objects o LEFT JOIN sys.extended_properties ep ON ep.major_id=o.object_id AND ep.minor_id=0 AND ep.name='MS_Description' WHERE o.type IN ('U','V') AND o.name='{tableName}'";
                cmd.CommandText = sql;
                int object_id = 0;
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        table = DataReader2TableInfo(reader, out object_id);
                        break;
                    }
                }
                if (table == null) { return; }
                sql = $"SELECT c.object_id,c.column_id,c.name AS Name,t.name AS Type,c.is_nullable AS IsNullable,c.is_identity AS IsIdentity,c.max_length AS Byte,c.precision AS MaxLength,c.scale AS DecimalDigits,dc.definition AS DefaultValue,ep.value AS Description,CASE WHEN pk.column_id IS NOT NULL THEN 1 ELSE 0 END AS IsPrimaryKey FROM sys.columns c JOIN sys.types t ON c.user_type_id=t.user_type_id JOIN sys.objects o ON c.object_id=o.object_id LEFT JOIN sys.default_constraints dc ON c.default_object_id = dc.object_id LEFT JOIN sys.extended_properties ep ON ep.major_id=c.object_id AND ep.minor_id=c.column_id AND ep.name='MS_Description' LEFT JOIN (SELECT ic.object_id, ic.column_id FROM sys.indexes i JOIN sys.index_columns ic ON i.object_id=ic.object_id AND i.index_id=ic.index_id WHERE i.is_primary_key=1) pk ON pk.object_id=c.object_id AND pk.column_id=c.column_id WHERE c.object_id={object_id} ORDER BY c.column_id";
                cmd.CommandText = sql;
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        table.Columns.Add(DataReader2ColumnInfo(reader));
                    }
                }
            });
            return table;
        }
        public override List<ITable> GetTables()
        {
            List<ITable> tables = new List<ITable>();
            UseDbCommand((cmd) => {
                string sql = "SELECT o.object_id,o.type,o.name,ep.value AS description FROM sys.objects o LEFT JOIN sys.extended_properties ep ON ep.major_id=o.object_id AND ep.minor_id=0 AND ep.name='MS_Description' WHERE o.type IN ('U','V') ORDER BY o.type,o.name";
                cmd.CommandText= sql;
                Dictionary<int, int> tableIndexesDict = new Dictionary<int, int>();
                using (var reader = cmd.ExecuteReader())
                {
                    int index = 0;
                    while (reader.Read())
                    {
                        int object_id;
                        var table= DataReader2TableInfo(reader, out object_id);
                        tables.Add(table);
                        tableIndexesDict[object_id] = index;
                        index++;
                    }
                }
                if (tableIndexesDict.Count < 1) { return; }
                sql = "SELECT c.object_id,c.column_id,c.name AS Name,t.name AS Type,c.is_nullable AS IsNullable,c.is_identity AS IsIdentity,c.max_length AS Byte,c.precision AS MaxLength,c.scale AS DecimalDigits,dc.definition AS DefaultValue,ep.value AS Description,CASE WHEN pk.column_id IS NOT NULL THEN 1 ELSE 0 END AS IsPrimaryKey FROM sys.columns c JOIN sys.types t ON c.user_type_id=t.user_type_id JOIN sys.objects o ON c.object_id=o.object_id LEFT JOIN sys.default_constraints dc ON c.default_object_id = dc.object_id LEFT JOIN sys.extended_properties ep ON ep.major_id=c.object_id AND ep.minor_id=c.column_id AND ep.name='MS_Description' LEFT JOIN (SELECT ic.object_id, ic.column_id FROM sys.indexes i JOIN sys.index_columns ic ON i.object_id=ic.object_id AND i.index_id=ic.index_id WHERE i.is_primary_key=1) pk ON pk.object_id=c.object_id AND pk.column_id=c.column_id WHERE c.object_id IN (SELECT object_id FROM sys.objects WHERE type IN ('U', 'V')) ORDER BY c.object_id,c.column_id";
                cmd.CommandText = sql;
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int object_id = Convert.ToInt32(reader["object_id"]);
                        if (!tableIndexesDict.TryGetValue(object_id, out var index)) { continue; }
                        tables[index].Columns.Add(DataReader2ColumnInfo(reader));
                    }
                }
            });
            return tables;
        }
        public override string Quote(string name)
        {
            if (string.IsNullOrEmpty(name)) { throw new ArgumentException("param name is null or empty", nameof(name)); }
            return name[0] == '[' ? name : "[" + name + "]";
        }
        public override string Param(string name)
        {
            if (string.IsNullOrEmpty(name)) { throw new ArgumentException("param name is null or empty", nameof(name)); }
            return name[0] == '@' ? name : "@" + name;
        }
        public override string Param(int index)
        {
            if (index < 0) { throw new ArgumentOutOfRangeException(nameof(index)); }
            return '@' + index.ToString();
        }
        protected override ISql CompileSelectOptions<T>(ISelectOptions<T> opts)
        {
            if (opts == null)
            {
                throw new ArgumentNullException(nameof(opts));
            }
            if (string.IsNullOrEmpty(opts.TableName))
            {
                throw new InvalidOperationException($"{nameof(opts.TableName)} is null or empty");
            }
            string fieldsPart = opts.Fields != null && opts.Fields.Count > 0 ? string.Join(",", opts.Fields.Select(f => Quote(f))) : "*";
            StringBuilder sql = new StringBuilder();
            sql.Append("SELECT ");
            if (opts.Top > 0)
            {
                sql.Append(" TOP ");
                sql.Append(opts.Top);
                sql.Append(" ");
            }
            sql.Append(fieldsPart);
            sql.Append(" FROM ");
            sql.Append(Quote(opts.TableName));
            if (!string.IsNullOrEmpty(opts.Where))
            {
                sql.Append(" WHERE ");
                sql.Append(opts.Where);
            }
            string orderbyPart = opts.Orderby != null && opts.Orderby.Count > 0 ? string.Join(",", opts.Orderby.Select(o => Quote(o.Field) + (o.IsAscending ? " ASC" : " DESC"))) : "";
            if (!string.IsNullOrEmpty(orderbyPart))
            {
                sql.Append(" ORDER BY ");
                sql.Append(orderbyPart);
            }
            return new Sql { Text = sql.ToString(), Params = opts.WhereParams };
        }
        public override IPageResult<T> Page<T>(IPageOptions<T> opts)
        {
            PageResult<T> reval = new PageResult<T> { Rows = new List<T>() };
            if (opts == null) { return reval; }
            if (string.IsNullOrWhiteSpace(opts.TableName))
            {
                throw new ArgumentNullException(nameof(opts.TableName));
            }
            if (opts.Converter == null)
            {
                throw new ArgumentNullException(nameof(opts.Converter));
            }
            int page = opts.Page;
            int size = opts.Size;
            page = page < 1 ? 1 : page;
            size = size < 1 ? 1 : size;

            reval.Page = page;
            reval.Size = size;

            string tableName = Quote(opts.TableName);

            long startIndex = page * size - size + 1;
            long endIndex = page * size;

            string fieldsPart = opts.Fields != null && opts.Fields.Count > 0 ? string.Join(",", opts.Fields.Select(f => Quote(f))) : "*";
            string orderbyPart = opts.Orderby != null && opts.Orderby.Count > 0 ? string.Join(",", opts.Orderby.Select(o => Quote(o.Field) + (o.IsAscending ? " ASC" : " DESC"))) : "";
            if (string.IsNullOrEmpty(orderbyPart))
            {
                orderbyPart = "(SELECT NULL)";
            }
            string where = opts.Where;
            if (string.IsNullOrEmpty(where))
            {
                where = "1>0";
            }
            string uniqueField = opts.UniqueField;
            StringBuilder sql = new StringBuilder();
            if (!string.IsNullOrEmpty(uniqueField))//具有唯一性字段
            {
                uniqueField = Quote(uniqueField);
                sql.AppendFormat("SELECT {0} FROM {1}", uniqueField, tableName);
                //获取唯一性字段集合
                sql.AppendFormat(" WHERE {0} IN(SELECT {0} FROM(", fieldsPart);
                sql.AppendFormat("SELECT ROW_NUMBER() OVER (ORDER BY {0})AS PageRowId,{1} FROM {2}", orderbyPart, fieldsPart, tableName);
                if (!string.IsNullOrEmpty(where))
                {
                    sql.AppendFormat(" WHERE {0}", where);
                }
                sql.AppendFormat(")TT WHERE TT.PageRowId BETWEEN {0} AND {1}) ORDER BY {2}", startIndex, endIndex, orderbyPart);
            }
            else//没有唯一性字段
            {
                sql.AppendFormat("SELECT {0} FROM (", fieldsPart);
                sql.AppendFormat("SELECT ROW_NUMBER() OVER (ORDER BY {0})AS PageRowId,{1} FROM {2}", orderbyPart, fieldsPart, tableName);
                if (!string.IsNullOrEmpty(where))
                {
                    sql.AppendFormat(" WHERE {0}", where);
                }
                sql.Append(") TT");
                sql.AppendFormat(" WHERE TT.PageRowId BETWEEN {0} AND {1}", startIndex, endIndex);
            }
            UseDbCommand((cmd) =>
            {
                cmd.CommandText = string.Format("SELECT COUNT(1) FROM {0} WHERE {1}", tableName, where);
                if (opts.WhereParams != null)
                {
                    for (int i = 0; i < opts.WhereParams.Length; i++)
                    {
                        cmd.Parameters.Add(CreateParameter(i, opts.WhereParams[i]));
                    }
                }
                object obj = cmd.ExecuteScalar();
                long total = 0;
                if (obj != null && obj != DBNull.Value)
                {
                    total = Convert.ToInt64(obj);
                }
                reval.Total = total;
                long pages = total % size == 0 ? total / size : (total / size + 1);
                if (page > pages)
                {
                    return;
                }
                cmd.CommandText = sql.ToString();
                using (var reader = cmd.ExecuteReader())
                {
                    DataReaderWrapper dr = new DataReaderWrapper(reader);
                    while (reader.Read())
                    {
                        reval.Rows.Add(opts.Converter(dr));
                    }
                }
            });
            return reval;
        }


        private Type CSharpType(string dbDataTypeName, bool isNullable)
        {
            if (string.IsNullOrWhiteSpace(dbDataTypeName)) { return typeof(object); }
            if (!DbDataTypeMap.GetType(DbType, dbDataTypeName, out var type)) { return typeof(object); }
            if (isNullable && type.IsValueType && Nullable.GetUnderlyingType(type) == null) { type = typeof(Nullable<>).MakeGenericType(type); }
            return type;
        }
        private ITable DataReader2TableInfo(IDataReader reader, out int object_id)
        {
            string name = reader["name"].ToString().Trim();
            string type = reader["type"].ToString().Trim();
            bool isView = "V" == type;
            string description = reader["description"] == DBNull.Value ? "" : reader["description"].ToString().Trim();
            if (string.IsNullOrEmpty(description))
            {
                description = (isView ? "VIEW:" : "TABLE:") + name;
            }
            object_id = Convert.ToInt32(reader["object_id"]);
            return new Table { DbType = DbType, Columns = new List<IColumn>(), DbKey = this.DbKey, Description = description, IsView = isView, Name = name };
        }
        private IColumn DataReader2ColumnInfo(IDataReader reader)
        {
            string type = reader["Type"].ToString();
            bool isNullable = Convert.ToBoolean(reader["IsNullable"]);
            bool isIdentity = Convert.ToBoolean(reader["IsIdentity"]);
            bool isPrimaryKey = Convert.ToBoolean(reader["IsPrimaryKey"]);
            string description = reader["Description"] != DBNull.Value ? reader["Description"].ToString() : "";
            if (string.IsNullOrEmpty(description))
            {
                description = "[" + type + "]";
                if (isIdentity) { description += "+[Identity]"; }
                if (isPrimaryKey) { description += "+[PrimaryKey]"; }
            }

            return new Column
            {
                MaxLength = Convert.ToInt64(reader["MaxLength"]),
                Byte = Convert.ToInt64(reader["Byte"]),
                DecimalDigits = Convert.ToInt32(reader["DecimalDigits"]),
                DefaultValue = reader["DefaultValue"] != DBNull.Value ? reader["DefaultValue"].ToString() : "",
                Description = description,
                IsIdentity = isIdentity,
                IsNullable = isNullable,
                IsPrimaryKey = isPrimaryKey,
                DbDataTypeName = type,
                Name = reader["Name"].ToString(),
                DataType = CSharpType(type, isNullable)
            };
        }
    }
}
