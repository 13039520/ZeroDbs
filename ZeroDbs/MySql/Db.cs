using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;

namespace ZeroDbs.MySql
{
    internal class Db: Common.Db
    {
        public Db(Common.DbConfigDatabaseInfo database): base(database)
        {

        }

        public override System.Data.Common.DbConnection GetDbConnection()
        {
            return new MySqlConnection(Database.dbConnectionString);
        }
        public override ZeroDbs.Common.DbDataTableInfo GetTable<DbEntity>()
        {
            if (!IsMappingToDbKey<DbEntity>())
            {
                throw new Exception("类型" + typeof(DbEntity).FullName + "没有映射到" + Database.dbKey + "上");
            }

            var key = typeof(DbEntity).FullName;
            var value = Common.DbDataviewStructCache.Get(key);
            if (value != null)
            {
                return value;
            }

            var cmd = this.GetDbCommand();
            try
            {
                var dbName = cmd.DbConnection.Database;
                var dv = Common.DbMapping.GetDbConfigDataViewInfo<DbEntity>().Find(o => string.Equals(o.dbKey, Database.dbKey, StringComparison.OrdinalIgnoreCase));
                string getTableOrViewSql = "SELECT * FROM information_schema.TABLES WHERE TABLE_SCHEMA='" + dbName + "' AND TABLE_NAME='" + dv.tableName + "'";

                Common.DbDataTableInfo dbDataTableInfo = null;

                cmd.CommandText = getTableOrViewSql;
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    dbDataTableInfo = new Common.DbDataTableInfo();
                    ZeroDbs.Common.DbDataTableInfo m = new Common.DbDataTableInfo();
                    bool isTable = (reader["TABLE_TYPE"].ToString() != "VIEW");
                    string tableName = reader["TABLE_NAME"].ToString();
                    string description = reader["TABLE_COMMENT"].ToString();
                    if (description != "")
                    {
                        if (isTable)
                        {
                            if (description == "TABLE")
                            {
                                description = "TABLE:" + tableName;
                            }
                        }
                        else
                        {
                            if (description == "VIEW")
                            {
                                description = "VIEW:" + tableName;
                            }
                        }
                    }
                    dbDataTableInfo.Name = tableName;
                    dbDataTableInfo.IsView = isTable ? false : true;
                    dbDataTableInfo.Description = description;
                    dbDataTableInfo.DbName = dbName;
                    dbDataTableInfo.Colunms = new List<Common.DbDataColumnInfo>();
                }
                reader.Close();
                reader.Dispose();

                if (dbDataTableInfo == null)
                {
                    throw new Exception("查询" + dv.tableName + "的表信息不成功");
                }

                string getColumnInfoSql = "SELECT * FROM information_schema.COLUMNS WHERE table_schema='" + dbDataTableInfo.DbName + "' AND table_name='" + dbDataTableInfo.Name + "'"; ;

                cmd.CommandText = getColumnInfoSql;
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    ZeroDbs.Common.DbDataColumnInfo column = new ZeroDbs.Common.DbDataColumnInfo();

                    column.MaxLength = reader["CHARACTER_MAXIMUM_LENGTH"].ToString().Length > 0 ? Convert.ToInt64(reader["CHARACTER_MAXIMUM_LENGTH"]) : -1;
                    column.Byte = reader["CHARACTER_OCTET_LENGTH"].ToString().Length > 0 ? Convert.ToInt64(reader["CHARACTER_OCTET_LENGTH"]) : -1;
                    column.DecimalDigits = reader["NUMERIC_SCALE"].ToString().Length > 0 ? Convert.ToInt32(reader["NUMERIC_SCALE"]) : -1;
                    column.DefaultValue = this.DbDataTypeMaping.GetDotNetDefaultValue(reader["COLUMN_DEFAULT"].ToString(), reader["DATA_TYPE"].ToString(), column.MaxLength);
                    column.Description = reader["COLUMN_COMMENT"].ToString();
                    column.IsIdentity = reader["EXTRA"].ToString().ToLower() == "auto_increment";
                    column.IsNullable = reader["IS_NULLABLE"].ToString().ToLower() == "yes";
                    column.IsPrimaryKey = reader["COLUMN_KEY"].ToString().ToUpper() == "PRI";//COLUMN_KEY
                    column.Name = reader["COLUMN_NAME"].ToString();
                    column.Type = this.DbDataTypeMaping.GetDotNetTypeString(reader["DATA_TYPE"].ToString(), column.MaxLength);

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
                var dbName = cmd.DbConnection.Database;
                string getAllTableAndViewSql = "SELECT * FROM information_schema.TABLES WHERE TABLE_SCHEMA='" + dbName + "'";

                List<ZeroDbs.Common.DbDataTableInfo> List = new List<ZeroDbs.Common.DbDataTableInfo>();

                cmd.CommandText = getAllTableAndViewSql;
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    ZeroDbs.Common.DbDataTableInfo m = new Common.DbDataTableInfo();
                    bool isTable = (reader["TABLE_TYPE"].ToString() != "VIEW");
                    string tableName = reader["TABLE_NAME"].ToString();
                    string description = reader["TABLE_COMMENT"].ToString();
                    if (description != "")
                    {
                        if (isTable)
                        {
                            if (description == "TABLE")
                            {
                                description = "TABLE:" + tableName;
                            }
                        }
                        else
                        {
                            if (description == "VIEW")
                            {
                                description = "VIEW:" + tableName;
                            }
                        }
                    }
                    m.Name = tableName;
                    m.IsView = isTable ? false : true;
                    m.Description = description;
                    m.DbName = dbName;
                    m.Colunms = new List<Common.DbDataColumnInfo>();
                    List.Add(m);
                }
                reader.Close();
                reader.Dispose();

                foreach (ZeroDbs.Common.DbDataTableInfo m in List)
                {
                    string sql = "SELECT * FROM information_schema.COLUMNS WHERE table_schema='"+ m.DbName + "' AND table_name='"+m.Name+"'";

                    cmd.CommandText = sql;
                    reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        ZeroDbs.Common.DbDataColumnInfo column = new ZeroDbs.Common.DbDataColumnInfo();

                        column.MaxLength = reader["CHARACTER_MAXIMUM_LENGTH"].ToString().Length > 0 ? Convert.ToInt64(reader["CHARACTER_MAXIMUM_LENGTH"]) : -1;
                        column.Byte = reader["CHARACTER_OCTET_LENGTH"].ToString().Length > 0 ? Convert.ToInt64(reader["CHARACTER_OCTET_LENGTH"]) : -1;
                        column.DecimalDigits = reader["NUMERIC_SCALE"].ToString().Length > 0 ? Convert.ToInt32(reader["NUMERIC_SCALE"]) : -1;
                        column.DefaultValue = this.DbDataTypeMaping.GetDotNetDefaultValue(reader["COLUMN_DEFAULT"].ToString(), reader["DATA_TYPE"].ToString(), column.MaxLength);
                        column.Description = reader["COLUMN_COMMENT"].ToString();
                        column.IsIdentity = reader["EXTRA"].ToString().ToLower() == "auto_increment";
                        column.IsNullable = reader["IS_NULLABLE"].ToString().ToLower() == "yes";
                        column.IsPrimaryKey = reader["COLUMN_KEY"].ToString().ToUpper() == "PRI";//COLUMN_KEY
                        column.Name = reader["COLUMN_NAME"].ToString();
                        column.Type = this.DbDataTypeMaping.GetDotNetTypeString(reader["DATA_TYPE"].ToString(), column.MaxLength);

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
