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
    internal class SQLite : AbsDatabase, IDatabase
    {
        public override string DbType { get { return "SQLite"; } }
        public SQLite(string dbKey, string connectionString, ISnowflakeIdGenerator snowflake) : base(dbKey, connectionString, snowflake)
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
            var param = new System.Data.SQLite.SQLiteParameter
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
            }
            param.Value = value;
            return param;
        }
        public override IDbDataParameter CreateParameter(int index, object value)
        {
            var param = new System.Data.SQLite.SQLiteParameter
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
            }
            param.Value = value;
            return param;
        }
        public override void UseDbConnection(DbConnectionHandler callback)
        {
            using (var conn = new System.Data.SQLite.SQLiteConnection(ConnectionString))
            {
                conn.Open();
                callback(conn);
            }
        }
        public override ITable GetTable(string tableName)
        {
            ITable table = null;
            UseDbCommand((cmd) => {
                Dictionary<string, string> identityDict = new Dictionary<string, string>();
                string sql = $"select * from sqlite_master where type IN('table','view') and name='{tableName}'";
                cmd.CommandText = sql;
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        table = DataReader2TableInfo(reader, identityDict);
                        break;
                    }
                }
                if (table == null) { return; }
                cmd.CommandText = "PRAGMA table_info(" + Quote(table.Name) + ")";
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var col = DataReader2ColumnInfo(reader, table, identityDict);
                        if (col != null)
                        {
                            table.Columns.Add(col);
                        }
                    }
                }
            });
            return table;
        }
        public override List<ITable> GetTables()
        {
            List<ITable> tables = new List<ITable>();
            UseDbCommand((cmd) => {
                Dictionary<string,string> identityDict = new Dictionary<string, string>();
                string sql = "select * from sqlite_master where type IN('table','view') and name<>'sqlite_sequence' order by type";
                cmd.CommandText = sql;
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        tables.Add(DataReader2TableInfo(reader, identityDict));
                    }
                }
                if (tables.Count < 1) { return; }
                foreach (var table in tables)
                {
                    cmd.CommandText = "PRAGMA table_info(" + Quote(table.Name) + ")";
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var col= DataReader2ColumnInfo(reader,table,identityDict);
                            if (col != null)
                            {
                                table.Columns.Add(col);
                            }
                        }
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


        private Type CSharpType(string dbDataTypeName, bool isNullable, long maxLength)
        {
            if (string.IsNullOrWhiteSpace(dbDataTypeName)) { return typeof(object); }
            dbDataTypeName = dbDataTypeName.ToUpperInvariant();
            Type type;
            // Guid 特殊约定
            if ((dbDataTypeName.Equals("CHAR", StringComparison.OrdinalIgnoreCase) || dbDataTypeName.Equals("VARCHAR", StringComparison.OrdinalIgnoreCase)) && maxLength == 36)
            {
                type = typeof(Guid);
            }
            else
            {
                if (!DbDataTypeMap.GetType(DbType, dbDataTypeName, out type))
                {
                    if (dbDataTypeName.Contains("INT")) { type = typeof(long); }
                    else if (dbDataTypeName.Contains("CHAR") || dbDataTypeName.Contains("TEXT")) { type = typeof(string); }
                    else if (dbDataTypeName.Contains("BLOB")) { type = typeof(byte[]); }
                    else if (dbDataTypeName.Contains("REAL") || dbDataTypeName.Contains("DOUBLE")) { type = typeof(double); }
                    else if (dbDataTypeName.Contains("DECIMAL") || dbDataTypeName.Contains("NUMERIC")) { type = typeof(decimal); }
                    else { type = typeof(object); }
                }
            }
            if (isNullable && type.IsValueType && Nullable.GetUnderlyingType(type) == null)
            {
                type = typeof(Nullable<>).MakeGenericType(type);
            }
            return type;
        }
        private ITable DataReader2TableInfo(IDataReader reader, Dictionary<string, string> identityDict)
        {
            string name = (reader["name"].ToString()).Trim();
            string type = (reader["type"].ToString()).Trim();
            string createSql = (reader["sql"].ToString()).Trim();
            if (!string.IsNullOrEmpty(createSql) && createSql.IndexOf("AUTOINCREMENT", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                //自增列判断依据： INTEGER类型+主键+AUTOINCREMENT
                //在SQLite中：AUTOINCREMENT 只能应用在INTEGER类型的主键上
                var match = Regex.Match(createSql, @"(?i)(\w+)\s+INTEGER\s+PRIMARY\s+KEY\s+AUTOINCREMENT");
                if (match.Success)
                {
                    identityDict[name] = match.Groups[1].Value;
                }
            }
            bool isView = type == "view";
            string description = (isView ? "VIEW:" : "TABLE:") + name;
            return new Table { DbType = DbType, DbKey = DbKey, Columns = new List<IColumn>(), Description = description, IsView = isView, Name = name };
        }
        private IColumn DataReader2ColumnInfo(IDataReader reader, ITable table, Dictionary<string, string> identityDict)
        {
            string type = reader["type"].ToString();
            var match = System.Text.RegularExpressions.Regex.Match(type, @"^(?<type>\w+)(\s*\((?<num1>\d+)(,\s*(?<num2>\d+))?\))?");
            if (!match.Success) { return null; }
            string typeStrOnly = match.Groups["type"].Value.ToUpper();
            string num1 = match.Groups["num1"].Value;
            string num2 = match.Groups["num2"].Value;
            int decimalDigits = 0;
            long maxLength = 0;
            if (!string.IsNullOrEmpty(num1))
            {
                maxLength = Convert.ToInt64(num1);
            }
            if (!string.IsNullOrEmpty(num2))
            {
                decimalDigits = Convert.ToInt32(num2);
            }
            string name = reader["Name"].ToString();
            bool isPrimaryKey = "0" != reader["pk"].ToString();
            bool isIdentity = false;
            if (identityDict.TryGetValue(table.Name, out var field))
            {
                isIdentity = field == name;
            }
            bool isNullable = "0" == reader["notnull"].ToString();
            string comment = "[" + typeStrOnly + "]";
            if (isPrimaryKey) { comment += "+[PrimaryKey]"; }
            if (isIdentity) { comment += "+[Identity]"; }
            return new Column
            {
                MaxLength = maxLength,
                Byte = 0,
                DecimalDigits = decimalDigits,
                DefaultValue = reader["dflt_value"] != DBNull.Value ? reader["dflt_value"].ToString() : "",
                Description = comment,
                IsIdentity = isIdentity,
                IsNullable = isNullable,
                IsPrimaryKey = isPrimaryKey,
                DbDataTypeName = typeStrOnly,
                Name = name,
                DataType = CSharpType(typeStrOnly, isNullable, maxLength)
            };
        }

    }
}
