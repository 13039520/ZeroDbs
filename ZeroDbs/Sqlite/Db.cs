using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Data.SQLite;
using MySql.Data.MySqlClient;

namespace ZeroDbs.Sqlite
{
    internal class Db: Common.Db
    {
        public Db(IDbInfo dbInfo) :base()
        {
            this.dbInfo = dbInfo;
            this.sqlBuilder = new SqlBuilder(this);
            this.dataTypeMaping = new DbDataTypeMaping();
        }

        public override System.Data.Common.DbConnection GetDbConnection(bool useSecondDb = false)
        {
            if (useSecondDb && !string.IsNullOrEmpty(DbInfo.ConnectionString2))
            {
                return new SQLiteConnection(DbInfo.ConnectionString2);
            }
            return new SQLiteConnection(DbInfo.ConnectionString);
        }
        public override ITableInfo GetTable<DbEntity>()
        {
            return GetTable(typeof(DbEntity).FullName);
        }
        public override ITableInfo GetTable(string entityFullName)
        {
            if (!IsMappingToDbKey(entityFullName))
            {
                throw new Exception("类型" + entityFullName + "没有映射到" + DbInfo.Key + "上");
            }
            string key = entityFullName;
            var value = Common.DbDataviewStructCache.Get(key);
            if (value != null)
            {
                return value;
            }

            var cmd = this.GetDbCommand();
            try
            {
                var dv = Common.DbMapping.GetDbTableEntityMapByEntityFullName(entityFullName).Find(o => string.Equals(o.DbKey, DbInfo.Key, StringComparison.OrdinalIgnoreCase));
                string getTableOrViewSql = "select * from sqlite_master where name='" + dv.TableName + "' and type IN('table','view')";

                Common.TableInfo dbDataTableInfo = null;
                List<string> IdentityNames = new List<string>();
                cmd.CommandText = getTableOrViewSql;
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    string type = (reader["type"].ToString()).Trim();
                    string name = (reader["name"].ToString()).Trim();
                    string tbl_name = (reader["tbl_name"].ToString()).Trim();
                    int rootpage = Convert.ToInt32(reader["rootpage"].ToString());
                    string sql = (reader["sql"].ToString()).Trim();

                    System.Text.RegularExpressions.Match temp = System.Text.RegularExpressions.Regex.Match(sql, @"(?<column>[^\{\}\(\),]\w+)\b[a-zA-Z0-9 ]{1,}\bAUTOINCREMENT\b", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                    if (temp.Success)
                    {
                        IdentityNames.Add(temp.Groups["column"].Value);
                    }

                    dbDataTableInfo = new Common.TableInfo();
                    dbDataTableInfo.DbName = cmd.DbConnection.DataSource;//cmd.DbConnection.Database;
                    dbDataTableInfo.Name = reader["name"].ToString();
                    dbDataTableInfo.IsView = "view" == type;
                    dbDataTableInfo.Description = (dbDataTableInfo.IsView ? "VIEW:" : "TABLE:") + dbDataTableInfo.Name;
                    dbDataTableInfo.Colunms = new List<IColumnInfo>();
                }
                reader.Close();
                reader.Dispose();

                if (dbDataTableInfo == null)
                {
                    throw new Exception("查询" + dv.TableName + "的表信息不成功");
                }

                string getColumnInfoSql = "PRAGMA table_info(" + dv.TableName + ")";
                cmd.IsCheckCommandText = false;
                cmd.CommandText = getColumnInfoSql;
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    ZeroDbs.Common.ColumnInfo column = new ZeroDbs.Common.ColumnInfo();

                    string type = reader["type"].ToString();
                    var match = System.Text.RegularExpressions.Regex.Match(type, @"^(?<type>\w+)(\s*\((?<num1>\d+)(,\s*(?<num2>\d+))?\))?");
                    if (!match.Success)
                    {
                        break;
                    }
                    string typeStrOnly = match.Groups["type"].Value;
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
                    column.Name = reader["name"].ToString();
                    column.MaxLength = maxLength;
                    column.Byte = 0;
                    column.DecimalDigits = decimalDigits;
                    column.DefaultValue = this.DataTypeMaping.GetDotNetDefaultValue(reader["dflt_value"].ToString(), reader["type"].ToString(), column.MaxLength);
                    column.Description = type;
                    column.IsIdentity = IdentityNames.Contains(column.Name);
                    column.IsNullable = "0" == reader["notnull"].ToString();
                    column.IsPrimaryKey = "0" != reader["pk"].ToString();
                    column.Type = this.DataTypeMaping.GetDotNetTypeString(typeStrOnly, column.MaxLength);

                    dbDataTableInfo.Colunms.Add(column);
                }
                reader.Close();
                reader.Dispose();

                cmd.Dispose();

                Common.DbDataviewStructCache.Set(key, dbDataTableInfo);

                return dbDataTableInfo;

            }
            catch (Exception ex)
            {
                cmd.Dispose();
                throw ex;
            }
        }
        public override List<ITableInfo> GetTables()
        {
            var cmd = this.GetDbCommand();
            try
            {
                var dbName = cmd.DbConnection.DataSource;//cmd.DbConnection.Database;
                List<string> sqlList = new List<string>();
                string getAllTableAndViewSql = "select * from sqlite_master where type IN('table','view') order by type";
               
                List<ITableInfo> List = new List<ITableInfo>();
                List<string> IdentityNames = new List<string>();
                cmd.CommandText = getAllTableAndViewSql;
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    string type = (reader["type"].ToString()).Trim();
                    string name = (reader["name"].ToString()).Trim();
                    string tbl_name = (reader["tbl_name"].ToString()).Trim();
                    int rootpage = Convert.ToInt32(reader["rootpage"].ToString());
                    string sql = (reader["sql"].ToString()).Trim();

                    if ("sqlite_sequence"== name) { continue; }
                    ZeroDbs.Common.TableInfo m = new Common.TableInfo();
                    
                    m.DbName = dbName;
                    m.Name = name;
                    m.IsView = "view" == type;
                    m.Description = (m.IsView ? "VIEW:": "TABLE:") + m.Name;
                    m.Colunms = new List<IColumnInfo>();
                    List.Add(m);
                    System.Text.RegularExpressions.Match temp = System.Text.RegularExpressions.Regex.Match(sql, @"(?<column>[^\{\}\(\),]\w+)\b[a-zA-Z0-9 ]{1,}\bAUTOINCREMENT\b", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                    if (temp.Success)
                    {
                        IdentityNames.Add(temp.Groups["column"].Value);
                    }
                }
                reader.Close();
                reader.Dispose();

                cmd.IsCheckCommandText = false;

                foreach (ZeroDbs.Common.TableInfo m in List)
                {
                    string sql = "PRAGMA table_info("+m.Name+")";
                    cmd.CommandText = sql;
                    reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        ZeroDbs.Common.ColumnInfo column = new ZeroDbs.Common.ColumnInfo();
                        
                        string type = reader["type"].ToString();
                        var match = System.Text.RegularExpressions.Regex.Match(type, @"^(?<type>\w+)(\s*\((?<num1>\d+)(,\s*(?<num2>\d+))?\))?");
                        if (!match.Success)
                        {
                            break;
                        }
                        string typeStrOnly = match.Groups["type"].Value;
                        string num1 = match.Groups["num1"].Value;
                        string num2 = match.Groups["num2"].Value;
                        int decimalDigits = 0;
                        long maxLength = 0;
                        if(!string.IsNullOrEmpty(num1))
                        {
                            maxLength=Convert.ToInt64(num1);
                        }
                        if(!string.IsNullOrEmpty(num2))
                        {
                            decimalDigits = Convert.ToInt32(num2);
                        }
                        column.Name = reader["name"].ToString();
                        column.MaxLength = maxLength;
                        column.Byte = 0;
                        column.DecimalDigits = decimalDigits;
                        column.DefaultValue = this.DataTypeMaping.GetDotNetDefaultValue(reader["dflt_value"].ToString(), reader["type"].ToString(), column.MaxLength);
                        column.Description = type;
                        column.IsIdentity = IdentityNames.Contains(column.Name);
                        column.IsNullable = "0" == reader["notnull"].ToString();
                        column.IsPrimaryKey = "0" != reader["pk"].ToString();
                        column.Type = this.DataTypeMaping.GetDotNetTypeString(typeStrOnly, column.MaxLength);
                        

                        m.Colunms.Add(column);
                    }/**/
                    reader.Close();
                    reader.Dispose();
                }

                cmd.Dispose();

                return List;
            }
            catch (Exception ex)
            {
                cmd.Dispose();
                throw ex;
            }
        }
        
    }
}
