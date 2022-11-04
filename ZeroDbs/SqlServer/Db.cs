using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
#if NET40
using System.Data.SqlClient;
#else
using Microsoft.Data.SqlClient;
#endif

namespace ZeroDbs.SqlServer
{
    internal class Db: Common.Db
    {
        public Db(IDbInfo dbInfo) : base()
        {
            this.dbInfo = dbInfo;
            this.sqlBuilder = new SqlBuilder(this);
            this.dataTypeMaping = new DbDataTypeMaping();
        }
        public override System.Data.Common.DbConnection GetDbConnection()
        {
#if NET40
            return new System.Data.SqlClient.SqlConnection(DbInfo.ConnectionString);
#else
            return new  Microsoft.Data.SqlClient.SqlConnection(DbInfo.ConnectionString);
#endif
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
                string getTableOrViewSql = "SELECT A.[id],A.[type],A.[name],"
                    + "(SELECT TOP 1 ISNULL(value, '') FROM sys.extended_properties AS E LEFT JOIN (SELECT object_id,name AS name2 FROM sys.views UNION SELECT object_id,name AS name2 FROM sys.tables) AS T1 ON T1.object_id=major_id WHERE E.minor_id=0 AND E.name='MS_Description' AND name2=A.[name])"
                    + "AS [description]"
                    + " FROM [sysobjects] AS A"
                    + " WHERE [name]='" + dv.TableName + "' AND ([type] = 'U' OR [type]= 'V')  ORDER BY [type],[name]";

                Common.TableInfo dbDataTableInfo = null;

                cmd.CommandText = getTableOrViewSql;
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    dbDataTableInfo = new Common.TableInfo();
                    dbDataTableInfo.DbName = cmd.DbConnection.Database;
                    dbDataTableInfo.Name = reader["name"].ToString();
                    string type = (reader["type"].ToString()).Trim();
                    dbDataTableInfo.IsView = "V" == type;
                    string description = reader["description"] == DBNull.Value ? "" : reader["description"].ToString();
                    if (string.IsNullOrEmpty(description))
                    {
                        if (dbDataTableInfo.IsView)
                        {
                            description = "VIEW:" + dbDataTableInfo.Name;
                        }
                        else
                        {
                            description = "TABLE:" + dbDataTableInfo.Name;
                        }
                    }
                    dbDataTableInfo.Description = description;
                    dbDataTableInfo.Colunms = new List<IColumnInfo>();
                }
                reader.Close();
                reader.Dispose();

                if (dbDataTableInfo == null)
                {
                    throw new Exception("查询" + dv.TableName + "的表信息不成功");
                }

                string getColumnInfoSql = "SELECT C.Name AS [Name],DbEntity.Name AS [Type],"
                    + "CONVERT(bit,C.IsNullable) AS [IsNullable],"
                    + "CONVERT(bit,CASE WHEN EXISTS(SELECT 1 FROM sysobjects WHERE xtype='PK' AND parent_obj=C.Id AND Name IN("
                    + "SELECT Name FROM sysindexes WHERE Indid IN("
                    + "SELECT Indid FROM sysindexkeys WHERE Id=C.Id AND ColId=C.ColId))) THEN 1 ELSE 0 END)"
                    + "AS [IsPrimaryKey],"
                    + "CONVERT(bit,COLUMNPROPERTY(C.Id,C.Name,'IsIdentity')) AS [IsIdentity],"
                    + "C.Length AS [Byte],"
                    + "COLUMNPROPERTY(C.Id,C.Name,'PRECISION') AS [MaxLength],"
                    + "ISNULL(COLUMNPROPERTY(C.Id,C.Name,'Scale'),0) AS [DecimalDigits],"
                    + "ISNULL(CM.text,'') AS [DefaultValue],"
                    + "ISNULL(ETP.value,'') AS [Description] "
                    + "FROM syscolumns C "
                    + "INNER JOIN systypes DbEntity ON C.xusertype = DbEntity.xusertype "
                    + "LEFT JOIN sys.extended_properties ETP ON ETP.major_id=C.id AND ETP.minor_id=C.colid AND ETP.name='MS_Description' "
                    + "LEFT JOIN syscomments CM ON C.cdefault=CM.id"
                    + " WHERE C.Id=object_id('" + dv.TableName + "')";

                List<string> sqlList2 = new List<string>();
                sqlList2.Add(getColumnInfoSql);

                cmd.CommandText = getColumnInfoSql;
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    ZeroDbs.Common.ColumnInfo column = new ZeroDbs.Common.ColumnInfo();
                    column.MaxLength = Convert.ToInt64(reader["MaxLength"]);
                    column.Byte = Convert.ToInt64(reader["Byte"]);
                    column.DecimalDigits = Convert.ToInt32(reader["DecimalDigits"]);
                    column.DefaultValue = this.DataTypeMaping.GetDotNetDefaultValue(reader["DefaultValue"].ToString(), reader["Type"].ToString(), column.MaxLength);
                    column.Description = reader["Description"].ToString();
                    column.IsIdentity = Convert.ToBoolean(reader["IsIdentity"]);
                    column.IsNullable = Convert.ToBoolean(reader["IsNullable"]);
                    column.IsPrimaryKey = Convert.ToBoolean(reader["IsPrimaryKey"]);
                    column.Type = this.DataTypeMaping.GetDotNetTypeString(reader["Type"].ToString(), column.MaxLength);
                    column.Name = reader["Name"].ToString();

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
                var dbName = cmd.DbConnection.Database;
                List<string> sqlList = new List<string>();
                string getAllTableAndViewSql = "SELECT A.[id],A.[type],A.[name],"
                    + "(SELECT TOP 1 ISNULL(value, '') FROM sys.extended_properties AS E LEFT JOIN (SELECT object_id,name AS name2 FROM sys.views UNION SELECT object_id,name AS name2 FROM sys.tables) AS T1 ON T1.object_id=major_id WHERE E.minor_id=0 AND E.name='MS_Description' AND name2=A.[name])"
                    + "AS [description]"
                    + " FROM[sysobjects] AS A"
                    + " WHERE([type] = 'U' OR [type]= 'V')  ORDER BY [type],[name]";
               
                List<ITableInfo> List = new List<ITableInfo>();
                
                cmd.CommandText = getAllTableAndViewSql;
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    ZeroDbs.Common.TableInfo m = new Common.TableInfo();
                    m.DbName = dbName;
                    m.Name = reader["name"].ToString();
                    string type = (reader["type"].ToString()).Trim();
                    m.IsView = "V" == type;
                    string description = reader["description"] == DBNull.Value ? "" : reader["description"].ToString();
                    if (string.IsNullOrEmpty(description))
                    {
                        if (m.IsView)
                        {
                            description = "VIEW:" + m.Name;
                        }
                        else
                        {
                            description = "TABLE:" + m.Name;
                        }
                    }
                    m.Description = description;
                    m.Colunms = new List<IColumnInfo>();
                    List.Add(m);
                }
                reader.Close();
                reader.Dispose();

                foreach (ZeroDbs.Common.TableInfo m in List)
                {
                    string sql = "SELECT C.Name AS [Name],DbEntity.Name AS [Type],"
                    + "CONVERT(bit,C.IsNullable) AS [IsNullable],"
                    + "CONVERT(bit,CASE WHEN EXISTS(SELECT 1 FROM sysobjects WHERE xtype='PK' AND parent_obj=C.Id AND Name IN("
                    + "SELECT Name FROM sysindexes WHERE Indid IN("
                    + "SELECT Indid FROM sysindexkeys WHERE Id=C.Id AND ColId=C.ColId))) THEN 1 ELSE 0 END)"
                    + "AS [IsPrimaryKey],"
                    + "CONVERT(bit,COLUMNPROPERTY(C.Id,C.Name,'IsIdentity')) AS [IsIdentity],"
                    + "C.Length AS [Byte],"
                    + "COLUMNPROPERTY(C.Id,C.Name,'PRECISION') AS [MaxLength],"
                    + "ISNULL(COLUMNPROPERTY(C.Id,C.Name,'Scale'),0) AS [DecimalDigits],"
                    + "ISNULL(CM.text,'') AS [DefaultValue],"
                    + "ISNULL(ETP.value,'') AS [Description] "
                    + "FROM syscolumns C "
                    + "INNER JOIN systypes DbEntity ON C.xusertype = DbEntity.xusertype "
                    + "LEFT JOIN sys.extended_properties ETP ON ETP.major_id=C.id AND ETP.minor_id=C.colid AND ETP.name='MS_Description' "
                    + "LEFT JOIN syscomments CM ON C.cdefault=CM.id"
                    + " WHERE C.Id=object_id('" + m.Name + "')";

                    cmd.CommandText = sql;
                    reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        ZeroDbs.Common.ColumnInfo column = new ZeroDbs.Common.ColumnInfo();
                        column.MaxLength = Convert.ToInt64(reader["MaxLength"]);
                        column.Byte = Convert.ToInt64(reader["Byte"]);
                        column.DecimalDigits = Convert.ToInt32(reader["DecimalDigits"]);
                        column.DefaultValue = this.DataTypeMaping.GetDotNetDefaultValue(reader["DefaultValue"].ToString(), reader["Type"].ToString(), column.MaxLength);
                        column.Description = reader["Description"].ToString();
                        column.IsIdentity = Convert.ToBoolean(reader["IsIdentity"]);
                        column.IsNullable = Convert.ToBoolean(reader["IsNullable"]);
                        column.IsPrimaryKey = Convert.ToBoolean(reader["IsPrimaryKey"]);
                        column.Type = this.DataTypeMaping.GetDotNetTypeString(reader["Type"].ToString(), column.MaxLength);
                        column.Name = reader["Name"].ToString();

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
