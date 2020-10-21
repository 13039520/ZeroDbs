using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Data.SQLite;

namespace ZeroDbs.Sqlite
{
    internal class Db: IDb
    {
        private IDataTypeMaping dbDataTypeMaping = null;
        private Common.DbConfigDatabaseInfo dbConfigDatabaseInfo = null;
        private IDbSqlBuilder dbSqlBuilder = null;
        public Common.DbConfigDatabaseInfo DbConfigDatabaseInfo { get { return dbConfigDatabaseInfo; } }
        public IDbSqlBuilder DbSqlBuilder { get { return dbSqlBuilder; } }
        public IDataTypeMaping DbDataTypeMaping { get { return dbDataTypeMaping; } }

        public event Common.DbExecuteSqlEvent OnDbExecuteSqlEvent = null;
        public Db(Common.DbConfigDatabaseInfo dbConfigDatabaseInfo)
        {
            this.dbConfigDatabaseInfo = dbConfigDatabaseInfo;
            this.dbDataTypeMaping = new DbDataTypeMaping();
            this.dbSqlBuilder = new DbSqlBuilder(this);
        }
        public void FireZeroDbExecuteSqlEvent(ZeroDbs.Common.DbExecuteSqlEventArgs args)
        {
            if (this.OnDbExecuteSqlEvent != null)
            {
                this.OnDbExecuteSqlEvent(this, args);
            }
        }
        private bool IsMappingToDbKey<T>()
        {
            var temp = Common.DbMapping.GetZeroDbConfigDatabaseInfo<T>();
            if (temp == null || temp.Count < 1)
            {
                return false;
            }
            return null != temp.Find(o => string.Equals(o.dbKey, DbConfigDatabaseInfo.dbKey, StringComparison.OrdinalIgnoreCase));
        }

        public System.Data.Common.DbConnection GetDbConnection()
        {
            return new SQLiteConnection(DbConfigDatabaseInfo.dbConnectionString);
        }
        public ZeroDbs.IDbCommand GetDbCommand()
        {
            var cmd = new SQLiteCommand();
            cmd.Connection = new SQLiteConnection(DbConfigDatabaseInfo.dbConnectionString);
            cmd.Connection.Open();
            return new ZeroDbs.Common.DbCommand(DbConfigDatabaseInfo.dbKey, cmd, this.OnDbExecuteSqlEvent, this.DbSqlBuilder);
        }
        public ZeroDbs.IDbCommand GetDbCommand(System.Data.Common.DbConnection dbConnection)
        {
            var cmd = new SQLiteCommand();
            cmd.Connection = (SQLiteConnection)dbConnection;
            if (cmd.Connection.State != System.Data.ConnectionState.Open)
            {
                cmd.Connection.Open();
            }
            return new ZeroDbs.Common.DbCommand(DbConfigDatabaseInfo.dbKey, cmd, this.OnDbExecuteSqlEvent, this.DbSqlBuilder);
        }
        public ZeroDbs.IDbCommand GetDbCommand(System.Data.Common.DbTransaction dbTransaction)
        {
            var cmd = new SQLiteCommand();
            cmd.Connection = (SQLiteConnection)dbTransaction.Connection;
            if (cmd.Connection.State != System.Data.ConnectionState.Open)
            {
                cmd.Connection.Open();
            }
            cmd.Transaction = (SQLiteTransaction)dbTransaction;
            return new ZeroDbs.Common.DbCommand(DbConfigDatabaseInfo.dbKey, cmd, this.OnDbExecuteSqlEvent, this.DbSqlBuilder);
        }
        public ZeroDbs.IDbTransactionScope GetDbTransactionScope(System.Data.IsolationLevel level, string identification="", string groupId="")
        {
            var conn = this.GetDbConnection();
            conn.Open();
            var trans = conn.BeginTransaction(level);
            return new ZeroDbs.Common.DbTransactionScope(this, identification, groupId);
        }
        public ZeroDbs.IDbTransactionScopeCollection GetDbTransactionScopeCollection()
        {
            return new ZeroDbs.Common.DbTransactionScopeCollection();
        }
        public ZeroDbs.Common.DbDataTableInfo GetDbDataTableInfo<T>() where T : class, new()
        {
            if (!IsMappingToDbKey<T>())
            {
                throw new Exception("类型" + typeof(T).FullName + "没有映射到" + DbConfigDatabaseInfo.dbKey + "上");
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
                var dv = Common.DbMapping.GetDbConfigDataViewInfo<T>().Find(o => string.Equals(o.dbKey, DbConfigDatabaseInfo.dbKey, StringComparison.OrdinalIgnoreCase));
                string getTableOrViewSql = "select * from sqlite_master where name='"+dv.tableName + "' and type IN('table','view')";

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

                    System.Text.RegularExpressions.Match temp = System.Text.RegularExpressions.Regex.Match(sql, @"\b(?<column>\w+) INTEGER PRIMARY KEY AUTOINCREMENT\b", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
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
                    throw new Exception("查询" + dv.tableName + "的表信息不成功");
                }

                string getColumnInfoSql = "PRAGMA table_info(" + dv.tableName + ")";
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
        public List<ZeroDbs.Common.DbDataTableInfo> GetDbDataTableInfoAll()
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
                    System.Text.RegularExpressions.Match temp = System.Text.RegularExpressions.Regex.Match(sql, @"\b(?<column>\w+) INTEGER PRIMARY KEY AUTOINCREMENT\b", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
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
        public bool DbConnectionTest()
        {
            try
            {
                var conn = this.GetDbConnection();
                conn.Open();
                conn.Close();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ZeroDbs.IDbCommand GetDbCommand<T>() where T : class, new()
        {
            if (!IsMappingToDbKey<T>())
            {
                throw new Exception("类型" + typeof(T).FullName + "没有映射到" + DbConfigDatabaseInfo.dbKey + "上");
            }
            return GetDbCommand();
        }
        public ZeroDbs.IDbTransactionScope GetDbTransactionScope<T>(System.Data.IsolationLevel level, string identification = "", string groupId = "") where T : class, new()
        {
            if (!IsMappingToDbKey<T>())
            {
                throw new Exception("类型" + typeof(T).FullName + "没有映射到" + DbConfigDatabaseInfo.dbKey + "上");
            }
            return GetDbTransactionScope(level, identification, groupId);
        }
        public T Get<T>(object key) where T : class, new()
        {
            if (!IsMappingToDbKey<T>())
            {
                throw new Exception("类型" + typeof(T).FullName + "没有映射到" + DbConfigDatabaseInfo.dbKey + "上");
            }
            var cmd = GetDbCommand();
            try
            {
                var sql = DbSqlBuilder.SelectByKey<T>(key);

                cmd.CommandText = sql;
                List<T> reval = cmd.ExecuteReader<T>();
                cmd.Dispose();

                return reval != null && reval.Count > 0 ? reval[0] : null;
            }
            catch (Exception ex)
            {
                cmd.Dispose();
                throw ex;
            }
        }
        public List<T> Select<T>(string where) where T : class, new()
        {
            if (!IsMappingToDbKey<T>())
            {
                throw new Exception("类型" + typeof(T).FullName + "没有映射到" + DbConfigDatabaseInfo.dbKey + "上");
            }
            var cmd = GetDbCommand();
            try
            {
                var sql = DbSqlBuilder.Select<T>(where, "");

                cmd.CommandText = sql;
                List<T> reval = cmd.ExecuteReader<T>();
                cmd.Dispose();

                return reval;
            }
            catch (Exception ex)
            {
                cmd.Dispose();
                throw ex;
            }
        }
        public List<T> Select<T>(string where, string orderby) where T : class, new()
        {
            if (!IsMappingToDbKey<T>())
            {
                throw new Exception("类型" + typeof(T).FullName + "没有映射到" + DbConfigDatabaseInfo.dbKey + "上");
            }
            var cmd = GetDbCommand();
            try
            {
                var sql = DbSqlBuilder.Select<T>(where, orderby);

                cmd.CommandText = sql;
                List<T> reval = cmd.ExecuteReader<T>();
                cmd.Dispose();

                return reval;
            }
            catch (Exception ex)
            {
                cmd.Dispose();
                throw ex;
            }
        }
        public List<T> Select<T>(string where, string orderby, int top) where T : class, new()
        {
            if (!IsMappingToDbKey<T>())
            {
                throw new Exception("类型" + typeof(T).FullName + "没有映射到" + DbConfigDatabaseInfo.dbKey + "上");
            }
            var cmd = GetDbCommand();
            try
            {
                var sql = DbSqlBuilder.Select<T>(where, orderby, top);

                cmd.CommandText = sql;
                List<T> reval = cmd.ExecuteReader<T>();
                cmd.Dispose();

                return reval;
            }
            catch (Exception ex)
            {
                cmd.Dispose();
                throw ex;
            }
        }
        public List<T> Select<T>(string where, string orderby, int top, int threshold) where T : class, new()
        {
            if (!IsMappingToDbKey<T>())
            {
                throw new Exception("类型" + typeof(T).FullName + "没有映射到" + DbConfigDatabaseInfo.dbKey + "上");
            }
            var cmd = GetDbCommand();
            try
            {
                var sql = DbSqlBuilder.Select<T>(where, orderby, top, threshold);

                cmd.CommandText = sql;
                List<T> reval = cmd.ExecuteReader<T>();
                cmd.Dispose();

                return reval;
            }
            catch (Exception ex)
            {
                cmd.Dispose();
                throw ex;
            }
        }
        public List<T> Select<T>(string where, string orderby, int top, string[] fieldNames) where T : class, new()
        {
            if (!IsMappingToDbKey<T>())
            {
                throw new Exception("类型" + typeof(T).FullName + "没有映射到" + DbConfigDatabaseInfo.dbKey + "上");
            }
            var cmd = GetDbCommand();
            try
            {
                var sql = DbSqlBuilder.Select<T>(where, orderby, top, fieldNames);

                cmd.CommandText = sql;
                List<T> reval = cmd.ExecuteReader<T>();
                cmd.Dispose();

                return reval;
            }
            catch (Exception ex)
            {
                cmd.Dispose();
                throw ex;
            }
        }
        
        public ZeroDbs.Common.PageData<T> Page<T>(long page, long size, string where) where T : class, new()
        {
            return Page<T>(page, size, where, "");
        }
        public ZeroDbs.Common.PageData<T> Page<T>(long page, long size, string where, string orderby) where T : class, new()
        {
            return Page<T>(page, size, where, orderby, new string[] { });
        }
        public ZeroDbs.Common.PageData<T> Page<T>(long page, long size, string where, string orderby, int threshold) where T : class, new()
        {
            return Page<T>(page, size, where, orderby, threshold, "");
        }
        public ZeroDbs.Common.PageData<T> Page<T>(long page, long size, string where, string orderby, int threshold, string uniqueFieldName) where T : class, new()
        {
            if (!IsMappingToDbKey<T>())
            {
                throw new Exception("类型" + typeof(T).FullName + "没有映射到" + DbConfigDatabaseInfo.dbKey + "上");
            }
            var cmd = this.GetDbCommand();
            try
            {
                var countSql = this.DbSqlBuilder.Count<T>(where);
                var sql = this.DbSqlBuilder.Page<T>(page, size, where, orderby, threshold, uniqueFieldName);
                var key = System.Text.RegularExpressions.Regex.Replace((typeof(T).FullName + where), @"[^\w]", "");
                long total = Common.ZeroDbPageCountCache.Get(key);
                if (total < 0)
                {
                    cmd.CommandText = countSql;
                    var obj = cmd.ExecuteScalar();
                    total = Convert.ToInt64(obj);
                    Common.ZeroDbPageCountCache.Set(key, total);
                }
                if (total < 1)
                {
                    cmd.Dispose();
                    return new Common.PageData<T> { Total = total, Items = new List<T>() };
                }
                long pages = total % size == 0 ? total / size : (total / size + 1);
                if (page > pages)
                {
                    cmd.Dispose();
                    return new Common.PageData<T >{ Total = total, Items = new List<T>() };
                }
                cmd.CommandText = sql;
                var reval = cmd.ExecuteReader<T>();
                cmd.Dispose();

                return new Common.PageData<T> { Total = total, Items = reval };
            }
            catch (Exception ex)
            {
                cmd.Dispose();
                throw ex;
            }
        }
        public ZeroDbs.Common.PageData<T> Page<T>(long page, long size, string where, string orderby, string[] fieldNames) where T : class, new()
        {
            return Page<T>(page, size, where, orderby, fieldNames, "");
        }
        public ZeroDbs.Common.PageData<T> Page<T>(long page, long size, string where, string orderby, string[] fieldNames, string uniqueFieldName) where T : class, new()
        {
            if (!IsMappingToDbKey<T>())
            {
                throw new Exception("类型" + typeof(T).FullName + "没有映射到" + DbConfigDatabaseInfo.dbKey + "上");
            }
            var cmd = this.GetDbCommand();
            try
            {
                var countSql = this.DbSqlBuilder.Count<T>(where);
                var sql = this.DbSqlBuilder.Page<T>(page, size, where, orderby, fieldNames, uniqueFieldName);
                var key = System.Text.RegularExpressions.Regex.Replace((typeof(T).FullName + where), @"[^\w]", "");
                long total = Common.ZeroDbPageCountCache.Get(key);
                if (total < 0)
                {
                    cmd.CommandText = countSql;
                    var obj = cmd.ExecuteScalar();
                    total = Convert.ToInt64(obj);
                    Common.ZeroDbPageCountCache.Set(key, total);
                }
                if (total < 1)
                {
                    cmd.Dispose();
                    return new Common.PageData<T> { Total = total, Items = new List<T>() };
                }
                long pages = total % size == 0 ? total / size : (total / size + 1);
                if (page > pages)
                {
                    cmd.Dispose();
                    return new Common.PageData<T> { Total = total, Items = new List<T>() };
                }
                cmd.CommandText = sql;
                var reval = cmd.ExecuteReader<T>();
                cmd.Dispose();

                return new Common.PageData<T> { Total = total, Items = reval };
            }
            catch (Exception ex)
            {
                cmd.Dispose();

                throw ex;
            }
        }

        public long Count<T>(string where) where T : class, new()
        {
            if (!IsMappingToDbKey<T>())
            {
                throw new Exception("类型" + typeof(T).FullName + "没有映射到" + DbConfigDatabaseInfo.dbKey + "上");
            }
            var cmd = this.GetDbCommand();
            try
            {
                var sql = this.DbSqlBuilder.Count<T>(where);

                cmd.CommandText = sql;
                var obj = cmd.ExecuteScalar();
                var reval = Convert.ToInt64(obj);
                cmd.Dispose();
                return reval;
            }
            catch (Exception ex)
            {
                cmd.Dispose();
                throw ex;
            }
        }

        public int Insert<T>(T entity) where T : class, new()
        {
            if (!IsMappingToDbKey<T>())
            {
                throw new Exception("类型" + typeof(T).FullName + "没有映射到" + DbConfigDatabaseInfo.dbKey + "上");
            }
            var cmd = this.GetDbCommand();
            try
            {
                var sql = this.DbSqlBuilder.Insert<T>(entity, new string[] { });

                cmd.CommandText = sql;
                var reval = cmd.ExecuteNonQuery();
                cmd.Dispose();
                return reval;
            }
            catch (Exception ex)
            {
                cmd.Dispose();
                throw ex;
            }
        }
        public int Insert<T>(List<T> entityList) where T : class, new()
        {
            if (!IsMappingToDbKey<T>())
            {
                throw new Exception("类型" + typeof(T).FullName + "没有映射到" + DbConfigDatabaseInfo.dbKey + "上");
            }

            var ts = this.GetDbTransactionScope(System.Data.IsolationLevel.ReadUncommitted);

            try
            {
                int reval = 0;
                var sqlList = this.DbSqlBuilder.Insert<T>(entityList, new string[] { });
                ts.Execute((cmd) =>
                {
                    foreach (var sql in sqlList)
                    {
                        cmd.CommandText = sql;
                        reval += cmd.ExecuteNonQuery();
                    }
                });
                ts.Dispose();
                return reval;
            }
            catch (Exception ex)
            {
                ts.Dispose();
                throw ex;
            }
        }
        public int Insert<T>(System.Collections.Specialized.NameValueCollection nvc) where T : class, new()
        {
            if (!IsMappingToDbKey<T>())
            {
                throw new Exception("类型" + typeof(T).FullName + "没有映射到" + DbConfigDatabaseInfo.dbKey + "上");
            }
            var cmd = this.GetDbCommand();
            try
            {
                var sql = this.DbSqlBuilder.Insert<T>(nvc);

                cmd.CommandText = sql;
                var reval = cmd.ExecuteNonQuery();
                cmd.Dispose();
                return reval;
            }
            catch (Exception ex)
            {
                cmd.Dispose();
                throw ex;
            }
        }
        public int Insert<T>(List<System.Collections.Specialized.NameValueCollection> nvcList) where T : class, new()
        {
            if (!IsMappingToDbKey<T>())
            {
                throw new Exception("类型" + typeof(T).FullName + "没有映射到" + DbConfigDatabaseInfo.dbKey + "上");
            }

            var ts = this.GetDbTransactionScope(System.Data.IsolationLevel.ReadUncommitted);

            try
            {
                int reval = 0;
                var sqlList = this.DbSqlBuilder.Insert<T>(nvcList);
                ts.Execute((cmd) =>
                {
                    foreach (var sql in sqlList)
                    {
                        cmd.CommandText = sql;
                        reval += cmd.ExecuteNonQuery();
                    }
                });
                ts.Dispose();
                return reval;
            }
            catch (Exception ex)
            {
                ts.Dispose();
                throw ex;
            }
        }

        public int Update<T>(T entity) where T : class, new()
        {
            if (!IsMappingToDbKey<T>())
            {
                throw new Exception("类型" + typeof(T).FullName + "没有映射到" + DbConfigDatabaseInfo.dbKey + "上");
            }
            var cmd = this.GetDbCommand();
            try
            {
                var sql = this.DbSqlBuilder.Update<T>(entity, new string[] { }, new string[] { });

                cmd.CommandText = sql;
                var reval = cmd.ExecuteNonQuery();
                cmd.Dispose();
                return reval;
            }
            catch (Exception ex)
            {
                cmd.Dispose();
                throw ex;
            }
        }
        public int Update<T>(List<T> entityList) where T : class, new()
        {
            if (!IsMappingToDbKey<T>())
            {
                throw new Exception("类型" + typeof(T).FullName + "没有映射到" + DbConfigDatabaseInfo.dbKey + "上");
            }

            var ts = this.GetDbTransactionScope(System.Data.IsolationLevel.ReadUncommitted);

            try
            {
                int reval = 0;
                var sqlList = this.DbSqlBuilder.Update<T>(entityList, new string[] { }, new string[] { });
                ts.Execute((cmd) =>
                {
                    foreach (var sql in sqlList)
                    {
                        cmd.CommandText = sql;
                        reval += cmd.ExecuteNonQuery();
                    }
                });
                ts.Dispose();
                return reval;
            }
            catch (Exception ex)
            {
                ts.Dispose();
                throw ex;
            }
        }
        public int Update<T>(System.Collections.Specialized.NameValueCollection nvc) where T : class, new()
        {
            if (!IsMappingToDbKey<T>())
            {
                throw new Exception("类型" + typeof(T).FullName + "没有映射到" + DbConfigDatabaseInfo.dbKey + "上");
            }
            var cmd = this.GetDbCommand();
            try
            {
                var sql = this.DbSqlBuilder.Update<T>(nvc);

                cmd.CommandText = sql;
                var reval = cmd.ExecuteNonQuery();
                cmd.Dispose();
                return reval;
            }
            catch (Exception ex)
            {
                cmd.Dispose();
                throw ex;
            }
        }
        public int Update<T>(List<System.Collections.Specialized.NameValueCollection> nvcList) where T : class, new()
        {
            if (!IsMappingToDbKey<T>())
            {
                throw new Exception("类型" + typeof(T).FullName + "没有映射到" + DbConfigDatabaseInfo.dbKey + "上");
            }

            var ts = this.GetDbTransactionScope(System.Data.IsolationLevel.ReadUncommitted);

            try
            {
                int reval = 0;
                var sqlList = this.DbSqlBuilder.Update<T>(nvcList);
                ts.Execute((cmd) =>
                {
                    foreach (var sql in sqlList)
                    {
                        cmd.CommandText = sql;
                        reval += cmd.ExecuteNonQuery();
                    }
                });
                ts.Dispose();
                return reval;
            }
            catch (Exception ex)
            {
                ts.Dispose();
                throw ex;
            }
        }

        public int Delete<T>(T entity) where T : class, new()
        {
            if (!IsMappingToDbKey<T>())
            {
                throw new Exception("类型" + typeof(T).FullName + "没有映射到" + DbConfigDatabaseInfo.dbKey + "上");
            }
            var cmd = this.GetDbCommand();
            try
            {
                var sql = this.DbSqlBuilder.Delete<T>(entity, new string[] { });

                cmd.CommandText = sql;
                var reval = cmd.ExecuteNonQuery();
                cmd.Dispose();
                return reval;
            }
            catch (Exception ex)
            {
                cmd.Dispose();
                throw ex;
            }
        }
        public int Delete<T>(List<T> entityList) where T : class, new()
        {
            if (!IsMappingToDbKey<T>())
            {
                throw new Exception("类型" + typeof(T).FullName + "没有映射到" + DbConfigDatabaseInfo.dbKey + "上");
            }

            var ts = this.GetDbTransactionScope(System.Data.IsolationLevel.ReadUncommitted);

            try
            {
                int reval = 0;
                var sqlList = this.DbSqlBuilder.Delete<T>(entityList, new string[] { });
                ts.Execute((cmd) =>
                {
                    foreach (var sql in sqlList)
                    {
                        cmd.CommandText = sql;
                        reval += cmd.ExecuteNonQuery();
                    }
                });
                ts.Dispose();
                return reval;
            }
            catch (Exception ex)
            {
                ts.Dispose();
                throw ex;
            }
        }
        public int Delete<T>(System.Collections.Specialized.NameValueCollection nvc) where T : class, new()
        {
            if (!IsMappingToDbKey<T>())
            {
                throw new Exception("类型" + typeof(T).FullName + "没有映射到" + DbConfigDatabaseInfo.dbKey + "上");
            }
            var cmd = this.GetDbCommand();
            try
            {
                var sql = this.DbSqlBuilder.Delete<T>(nvc);

                cmd.CommandText = sql;
                var reval = cmd.ExecuteNonQuery();
                cmd.Dispose();
                return reval;
            }
            catch (Exception ex)
            {
                cmd.Dispose();
                throw ex;
            }
        }
        public int Delete<T>(List<System.Collections.Specialized.NameValueCollection> nvcList) where T : class, new()
        {
            if (!IsMappingToDbKey<T>())
            {
                throw new Exception("类型" + typeof(T).FullName + "没有映射到" + DbConfigDatabaseInfo.dbKey + "上");
            }

            var ts = this.GetDbTransactionScope(System.Data.IsolationLevel.ReadUncommitted);

            try
            {
                int reval = 0;
                var sqlList = this.DbSqlBuilder.Delete<T>(nvcList);
                ts.Execute((cmd) =>
                {
                    foreach (var sql in sqlList)
                    {
                        cmd.CommandText = sql;
                        reval += cmd.ExecuteNonQuery();
                    }
                });
                ts.Dispose();
                return reval;
            }
            catch (Exception ex)
            {
                ts.Dispose();
                throw ex;
            }
        }


    }
}
