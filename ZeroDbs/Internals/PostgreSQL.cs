using NpgsqlTypes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ZeroDbs
{
    internal class PostgreSQL : AbsDatabase, IDatabase
    {
        Type _NpgsqlRangeType = typeof(NpgsqlTypes.NpgsqlRange<>);
        public override string DbType { get { return "PostgreSQL"; } }
        public PostgreSQL(string dbKey, string connectionString, ISnowflakeIdGenerator snowflake) : base(dbKey, connectionString, snowflake)
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
            var param = new Npgsql.NpgsqlParameter();
            param.ParameterName = Param(name);
            if (value == null)
            {
                param.Value = DBNull.Value;
                return param;
            }
            if (value is ISqlParameter v)
            {
                param.Value = v.Value != null ? v.Value : DBNull.Value;
                if (!string.IsNullOrEmpty(v.DbDataTypeName))
                {
                    param.DataTypeName = v.DbDataTypeName;
                }
                param.Direction = v.Direction;
                return param;
            }
            param.Value = value;
            return param;
        }
        public override IDbDataParameter CreateParameter(int index, object value)
        {
            var param = new Npgsql.NpgsqlParameter();
            param.ParameterName = Param(index);
            if (value == null)
            {
                param.Value = DBNull.Value;
                return param;
            }
            if (value is ISqlParameter v)
            {
                param.Value = v.Value != null ? v.Value : DBNull.Value;
                if (!string.IsNullOrEmpty(v.DbDataTypeName))
                {
                    param.DataTypeName = v.DbDataTypeName;
                }
                param.Direction = v.Direction;
                return param;
            }
            param.Value = value;
            return param;
        }
        public override void UseDbConnection(DbConnectionHandler callback)
        {
            using (var conn = new Npgsql.NpgsqlConnection(ConnectionString))
            {
                conn.Open();
                callback(conn);
            }
        }
        public override ITable GetTable(string tableName)
        {
            ITable table = null;
            UseDbCommand((cmd) => {
                var dbName = cmd.Connection.Database;
                Dictionary<string, string> identityDict = new Dictionary<string, string>();
                string sql = "SELECT A.relname,A.relkind,B.description as comment"
                    + " FROM pg_class AS A"
                    + " LEFT JOIN pg_description AS B ON B.objoid=A.oid AND B.objsubid=0"
                    + " WHERE A.relkind IN('r','v')"
                    + " AND A.relname='" + tableName + "'";
                cmd.CommandText = sql;
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        table = DataReader2TableInfo(reader);
                        break;
                    }
                }
                if (table == null) { return; }
                sql = "SELECT a.table_schema,a.table_name,a.ordinal_position,a.column_name,pg_type.typname AS data_type,"
                + "COALESCE(a.character_maximum_length, a.numeric_precision, 0) AS max_length,"
                + "a.character_octet_length,a.numeric_scale,"
                + "CASE WHEN a.is_nullable='NO' THEN 0 ELSE 1 END AS can_null,"
                + "a.column_default AS default_value,"
                + "CASE WHEN a.is_identity='YES' OR a.column_default LIKE 'nextval(%' THEN 1 ELSE 0 END AS is_identity,"
                + "CASE WHEN EXISTS (SELECT 1 FROM pg_constraint pc WHERE pc.conrelid=c.oid AND pc.contype='p' AND pg_attr.attnum=ANY(pc.conkey)) THEN 1 ELSE 0 END AS is_pk,"
                + "pg_desc.description AS comment"
                + " FROM information_schema.columns a"
                + " JOIN pg_namespace n ON n.nspname=a.table_schema"
                + " JOIN pg_class c ON c.relnamespace=n.oid AND c.relname=a.table_name AND c.relkind IN ('r', 'v')"
                + " JOIN pg_attribute pg_attr ON pg_attr.attrelid=c.oid AND pg_attr.attname=a.column_name AND pg_attr.attnum>0 AND NOT pg_attr.attisdropped"
                + " JOIN pg_type ON pg_type.oid=pg_attr.atttypid"
                + " LEFT JOIN pg_description pg_desc ON pg_desc.objoid = pg_attr.attrelid AND pg_desc.objsubid=pg_attr.attnum"
                + " WHERE a.table_schema='public' AND a.table_name='" + tableName + "'"
                + " ORDER BY a.ordinal_position";
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
                string sql = "SELECT A.relname,A.relkind,B.description as comment"
                    + " FROM pg_class AS A"
                    + " LEFT JOIN pg_description AS B ON B.objoid=A.oid AND B.objsubid=0"
                    + " WHERE A.relkind IN('r','v')"
                    + " AND A.relname NOT LIKE 'pg_%' AND A.relname NOT IN('spatial_ref_sys','geography_columns','geometry_columns')"
                    + " AND A.relnamespace=(SELECT oid FROM pg_namespace WHERE nspname='public' LIMIT 1)"
                    + " ORDER BY relkind,relname";
                cmd.CommandText = sql;
                Dictionary<string, int> tableIndexesDict = new Dictionary<string, int>();
                using (var reader = cmd.ExecuteReader())
                {
                    int index = 0;
                    while (reader.Read())
                    {
                        var table = DataReader2TableInfo(reader);
                        tables.Add(table);
                        tableIndexesDict[table.Name] = index;
                        index++;
                    }
                }
                if (tables.Count < 1) { return; }
                sql = "SELECT a.table_schema,a.table_name,a.ordinal_position,a.column_name,pg_type.typname AS data_type,"
                + "COALESCE(a.character_maximum_length, a.numeric_precision, 0) AS max_length,"
                + "a.character_octet_length,a.numeric_scale,"
                + "CASE WHEN a.is_nullable='NO' THEN 0 ELSE 1 END AS can_null,"
                + "a.column_default AS default_value,"
                + "CASE WHEN a.is_identity='YES' OR a.column_default LIKE 'nextval(%' THEN 1 ELSE 0 END AS is_identity,"
                + "CASE WHEN EXISTS (SELECT 1 FROM pg_constraint pc WHERE pc.conrelid=c.oid AND pc.contype='p' AND pg_attr.attnum=ANY(pc.conkey)) THEN 1 ELSE 0 END AS is_pk,"
                + "pg_desc.description AS comment"
                + " FROM information_schema.columns a"
                + " JOIN pg_namespace n ON n.nspname=a.table_schema"
                + " JOIN pg_class c ON c.relnamespace=n.oid AND c.relname=a.table_name AND c.relkind IN ('r', 'v')"
                + " JOIN pg_attribute pg_attr ON pg_attr.attrelid=c.oid AND pg_attr.attname=a.column_name AND pg_attr.attnum>0 AND NOT pg_attr.attisdropped"
                + " JOIN pg_type ON pg_type.oid=pg_attr.atttypid"
                + " LEFT JOIN pg_description pg_desc ON pg_desc.objoid = pg_attr.attrelid AND pg_desc.objsubid=pg_attr.attnum"
                + " WHERE a.table_schema='public'"
                + " ORDER BY a.table_schema,a.table_name,a.ordinal_position";
                cmd.CommandText = sql;
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string name = reader["table_name"].ToString();
                        if (!tableIndexesDict.TryGetValue(name, out var index)) { continue; }
                        tables[index].Columns.Add(DataReader2ColumnInfo(reader));
                    }
                }
            });
            return tables;
        }
        public override string Quote(string name)
        {
            if (string.IsNullOrEmpty(name)) { throw new ArgumentException("param name is null or empty", nameof(name)); }
            return name[0] == '"' ? name : "\"" + name + "\"";
        }
        public override string Param(string name)
        {
            if (string.IsNullOrEmpty(name)) { throw new ArgumentException("param name is null or empty", nameof(name)); }
            return name[0] == '@'  ? name : "@" + name;
        }
        public override string Param(int index)
        {
            if (index < 0) { throw new ArgumentOutOfRangeException(nameof(index)); }
            return '@' + index.ToString();
        }
        public override string GetReturnIdentityColumnSqlPart(string returnIdentityColumnName)
        {
            return "RETURNING " + Quote(returnIdentityColumnName) + ";";
        }


        private Type CSharpType(string dbDataTypeName, bool isNullable)
        {
            if (string.IsNullOrWhiteSpace(dbDataTypeName)) { return typeof(object); }
            if (DbDataTypeMap.GetType(DbType, dbDataTypeName, out var type))
            {
                if (type.IsGenericType && type.GetGenericTypeDefinition() == _NpgsqlRangeType)
                {
                    // 排除 Range，不做 Nullable 包装
                    return type;
                }
                if (isNullable && type.IsValueType && Nullable.GetUnderlyingType(type) == null)
                {
                    type = typeof(Nullable<>).MakeGenericType(type);
                }
                return type;
            }
            return typeof(object);
        }
        private ITable DataReader2TableInfo(IDataReader reader)
        {
            string name = reader["relname"].ToString();
            string type = reader["relkind"].ToString();
            string description = (reader["comment"].ToString()).Trim();
            bool isView = type == "v";
            if (string.IsNullOrWhiteSpace(description))
            {
                description = (isView ? "VIEW:" : "TABLE:") + name;
            }
            return new Table { DbType = DbType, DbKey = DbKey, Columns = new List<IColumn>(), Description = description, IsView = isView, Name = name };
        }
        private IColumn DataReader2ColumnInfo(IDataReader reader)
        {
            string type = reader["data_type"].ToString();
            bool can_null = Convert.ToInt32(reader["can_null"]) == 1;
            bool is_identity = Convert.ToInt32(reader["is_identity"]) == 1;
            bool is_pk = Convert.ToInt32(reader["is_pk"]) == 1;
            long max_length = 0;
            if (reader["max_length"] != DBNull.Value)
            {
                max_length = Convert.ToInt64(reader["max_length"]);
            }
            long character_octet_length = 0;
            if (reader["character_octet_length"] != DBNull.Value)
            {
                character_octet_length = Convert.ToInt64(reader["character_octet_length"]);
            }
            int numeric_scale = 0;
            if (reader["numeric_scale"] != DBNull.Value)
            {
                numeric_scale = Convert.ToInt32(reader["numeric_scale"]);
            }
            string comment = reader["comment"] != DBNull.Value ? reader["comment"].ToString() : "";
            if (string.IsNullOrWhiteSpace(comment))
            {
                comment = "[" + type + "]";
                if (is_pk) { comment += "+[PrimaryKey]"; }
                if (is_identity) { comment += "+[Identity]"; }
            }
            string default_value = reader["default_value"] != DBNull.Value ? reader["default_value"].ToString() : "";
            return new Column
            {
                MaxLength = max_length,
                Byte = character_octet_length,
                DecimalDigits = numeric_scale,
                DefaultValue = default_value,
                Description = comment,
                IsIdentity = is_identity,
                IsNullable = can_null,
                IsPrimaryKey = is_pk,
                DbDataTypeName = type,
                Name = reader["column_name"].ToString(),
                DataType = CSharpType(type, can_null)
            };
        }

    }
}
