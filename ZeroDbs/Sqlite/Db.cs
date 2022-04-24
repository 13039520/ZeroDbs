using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Data.SQLite;

namespace ZeroDbs.Sqlite
{
    internal class Db: Common.Db
    {
        public Db(Common.DbInfo database):base(database)
        {

        }

        public override System.Data.Common.DbConnection GetDbConnection()
        {
            return new SQLiteConnection(Database.ConnectionString);
        }
        public override ZeroDbs.Common.DbDataTableInfo GetTable<T>()
        {
            if (!IsMappingToDbKey<T>())
            {
                throw new Exception("类型" + typeof(T).FullName + "没有映射到" + Database.Key + "上");
            }

            var key = typeof(T).FullName;
            var value = Common.DbDataviewStructCache.Get(key);
            if (value != null)
            {
                return value;
            }

            var cmd = this.GetDbCommand();
            try
            {
                var dv = Common.DbMapping.GetDbConfigDataViewInfo<T>().Find(o => string.Equals(o.DbKey, Database.Key, StringComparison.OrdinalIgnoreCase));
                string getTableOrViewSql = "select * from sqlite_master where name='"+dv.TableName + "' and type IN('table','view')";

                Common.DbDataTableInfo dbDataTableInfo = null;
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

                    dbDataTableInfo = new Common.DbDataTableInfo();
                    dbDataTableInfo.DbName = cmd.DbConnection.DataSource;//cmd.DbConnection.Database;
                    dbDataTableInfo.Name = reader["name"].ToString();
                    dbDataTableInfo.IsView = "view" == type;
                    dbDataTableInfo.Description = (dbDataTableInfo.IsView ? "VIEW:" : "TABLE:") + dbDataTableInfo.Name;
                    dbDataTableInfo.Colunms = new List<Common.DbDataColumnInfo>();
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
                    ZeroDbs.Common.DbDataColumnInfo column = new ZeroDbs.Common.DbDataColumnInfo();
                    column.Name = reader["name"].ToString();
                    column.MaxLength = 0;
                    column.Byte = 0;
                    column.DecimalDigits = 0;
                    column.DefaultValue = this.DbDataTypeMaping.GetDotNetDefaultValue(reader["dflt_value"].ToString(), reader["type"].ToString(), column.MaxLength);
                    column.Description = reader["type"].ToString();
                    column.IsIdentity = IdentityNames.Contains(column.Name);
                    column.IsNullable = "0" == reader["notnull"].ToString();
                    column.IsPrimaryKey = "0" != reader["pk"].ToString();
                    column.Type = this.DbDataTypeMaping.GetDotNetTypeString(reader["Type"].ToString(), column.MaxLength);

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
        public override List<ZeroDbs.Common.DbDataTableInfo> GetTables()
        {
            var cmd = this.GetDbCommand();
            try
            {
                var dbName = cmd.DbConnection.DataSource;//cmd.DbConnection.Database;
                List<string> sqlList = new List<string>();
                string getAllTableAndViewSql = "select * from sqlite_master where type IN('table','view') order by type";
               
                List<ZeroDbs.Common.DbDataTableInfo> List = new List<ZeroDbs.Common.DbDataTableInfo>();
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
                    ZeroDbs.Common.DbDataTableInfo m = new Common.DbDataTableInfo();
                    
                    m.DbName = dbName;
                    m.Name = name;
                    m.IsView = "view" == type;
                    m.Description = (m.IsView ? "VIEW:": "TABLE:") + m.Name;
                    m.Colunms = new List<Common.DbDataColumnInfo>();
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

                foreach (ZeroDbs.Common.DbDataTableInfo m in List)
                {
                    string sql = "PRAGMA table_info("+m.Name+")";
                    cmd.CommandText = sql;
                    reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        ZeroDbs.Common.DbDataColumnInfo column = new ZeroDbs.Common.DbDataColumnInfo();
                        column.Name = reader["name"].ToString();
                        column.MaxLength = 0;
                        column.Byte = 0;
                        column.DecimalDigits = 0;
                        column.DefaultValue = this.DbDataTypeMaping.GetDotNetDefaultValue(reader["dflt_value"].ToString(), reader["type"].ToString(), column.MaxLength);
                        column.Description = reader["type"].ToString();
                        column.IsIdentity = IdentityNames.Contains(column.Name);
                        column.IsNullable = "0" == reader["notnull"].ToString();
                        column.IsPrimaryKey = "0" != reader["pk"].ToString();
                        column.Type = this.DbDataTypeMaping.GetDotNetTypeString(reader["Type"].ToString(), column.MaxLength);
                        

                        m.Colunms.Add(column);
                    }
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
