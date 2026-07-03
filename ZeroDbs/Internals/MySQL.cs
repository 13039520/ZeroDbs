using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlTypes;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ZeroDbs
{
    internal class MySQL : AbsDatabase, IDatabase
    {
        public override string DbType { get { return "MySql"; } }
        public MySQL(string dbKey, string connectionString, ISnowflakeIdGenerator snowflake) : base(dbKey, connectionString, snowflake)
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
            var param = new MySql.Data.MySqlClient.MySqlParameter
            {
                ParameterName = Param(name)
            };
            if (value == null)
            {
                param.Value = DBNull.Value;
                return param;
            }
            if (value is Guid guid)
            {
                param.Value = guid.ToString();
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
        }
        public override IDbDataParameter CreateParameter(int index, object value)
        {
            var param = new MySql.Data.MySqlClient.MySqlParameter
            {
                ParameterName = Param(index)
            };
            if (value == null)
            {
                param.Value = DBNull.Value;
                return param;
            }
            if (value is Guid guid)
            {
                param.Value = guid.ToString();
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
        }
        public override void UseDbConnection(DbConnectionHandler callback)
        {
            using (var conn = new MySql.Data.MySqlClient.MySqlConnection(ConnectionString))
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
                string sql = $"SELECT * FROM information_schema.TABLES WHERE TABLE_SCHEMA='{dbName}' AND TABLE_NAME='{tableName}'";
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
                sql = $"SELECT * FROM information_schema.COLUMNS WHERE TABLE_SCHEMA='{dbName}' AND TABLE_NAME='{tableName}' ORDER BY ORDINAL_POSITION";
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
                var dbName = cmd.Connection.Database;
                Dictionary<string,string> identityDict = new Dictionary<string, string>();
                string sql = $"SELECT * FROM information_schema.TABLES WHERE TABLE_SCHEMA='{dbName}' ORDER BY TABLE_TYPE,TABLE_NAME";
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
                sql = $"SELECT * FROM information_schema.COLUMNS WHERE TABLE_SCHEMA='{dbName}' ORDER BY TABLE_NAME,ORDINAL_POSITION";
                cmd.CommandText = sql;
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string tbname = reader["TABLE_NAME"].ToString();
                        if (!tableIndexesDict.TryGetValue(tbname, out var index)) { continue; }
                        tables[index].Columns.Add(DataReader2ColumnInfo(reader));
                    }
                }
            });
            return tables;
        }
        public override string Quote(string name)
        {
            if (string.IsNullOrEmpty(name)) { throw new ArgumentException("param name is null or empty", nameof(name)); }
            return name[0] == '`' ? name : "`" + name + "`";
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


        private Type CSharpType(string dbDataTypeName, bool isNullable, long maxLength)
        {
            if (string.IsNullOrWhiteSpace(dbDataTypeName)) { return typeof(object); }
            dbDataTypeName = dbDataTypeName.ToUpperInvariant();
            Type type;
            // Guid 特殊约定
            if (maxLength == 36 && (dbDataTypeName == "CHAR" || dbDataTypeName == "VARCHAR"))
            {
                type = typeof(Guid);
            }
            else if (maxLength == 16 && dbDataTypeName == "BINARY")
            {
                type = typeof(Guid);
            }
            else if (maxLength == 1 && dbDataTypeName == "BIT")
            {
                type = typeof(bool);
            }
            else
            {
                if (!DbDataTypeMap.GetType(DbType, dbDataTypeName, out type))
                {
                    type = typeof(object);
                }
            }
            if (isNullable && type.IsValueType && Nullable.GetUnderlyingType(type) == null)
            {
                type = typeof(Nullable<>).MakeGenericType(type);
            }
            return type;
        }
        private ITable DataReader2TableInfo(IDataReader reader)
        {
            bool isView = (reader["TABLE_TYPE"].ToString() == "VIEW");
            string name = reader["TABLE_NAME"].ToString();
            string description = reader["TABLE_COMMENT"].ToString();
            if (string.IsNullOrWhiteSpace(description))
            {
                if (isView)
                {
                    if (description == "VIEW")
                    {
                        description = "VIEW:" + name;
                    }
                }
                else
                {
                    if (description == "TABLE")
                    {
                        description = "TABLE:" + name;
                    }
                }
            }
            return new Table { DbType = DbType, DbKey = DbKey, Columns = new List<IColumn>(), Description = description, IsView = isView, Name = name };
        }
        private IColumn DataReader2ColumnInfo(IDataReader reader)
        {
            long maxLength = reader["CHARACTER_MAXIMUM_LENGTH"] != DBNull.Value ? Convert.ToInt64(reader["CHARACTER_MAXIMUM_LENGTH"]) : -1;
            long _byte = reader["CHARACTER_OCTET_LENGTH"] != DBNull.Value ? Convert.ToInt64(reader["CHARACTER_OCTET_LENGTH"]) : -1;
            int decimalDigits = reader["NUMERIC_SCALE"] != DBNull.Value ? Convert.ToInt32(reader["NUMERIC_SCALE"]) : -1;
            string description = reader["COLUMN_COMMENT"] != DBNull.Value ? reader["COLUMN_COMMENT"].ToString() : "";
            bool isIdentity = reader["EXTRA"] != DBNull.Value ? (reader["EXTRA"].ToString().ToLower() == "auto_increment") : false;
            bool isNullable = reader["IS_NULLABLE"] != DBNull.Value ? (reader["IS_NULLABLE"].ToString().ToLower() == "yes") : false;
            bool isPrimaryKey = reader["COLUMN_KEY"] != DBNull.Value ? (reader["COLUMN_KEY"].ToString().ToLower() == "pri") : false;
            string name = reader["COLUMN_NAME"].ToString();
            string type = reader["DATA_TYPE"].ToString();
            string defaultValue = reader["COLUMN_DEFAULT"] != DBNull.Value ? reader["COLUMN_DEFAULT"].ToString() : "";
            return new Column
            {
                MaxLength = maxLength,
                Byte = _byte,
                DecimalDigits = decimalDigits,
                DefaultValue = defaultValue,
                Description = description,
                IsIdentity = isIdentity,
                IsNullable = isNullable,
                IsPrimaryKey = isPrimaryKey,
                DbDataTypeName = type,
                Name = name,
                DataType = CSharpType(type, isNullable, maxLength)
            };
        }

    }
}
