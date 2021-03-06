﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;

namespace ZeroDbs.MySql
{
    internal class Db: ZeroDbs.IDb
    {
        private IDataTypeMaping dbDataTypeMaping = null;
        private Common.DbConfigDatabaseInfo dbConfigDatabaseInfo = null;
        private IDbSqlBuilder dbSqlBuilder = null;
        public Common.DbConfigDatabaseInfo DbConfigDatabaseInfo { get { return dbConfigDatabaseInfo; } }
        public IDbSqlBuilder DbSqlBuilder { get { return dbSqlBuilder; } }
        public IDataTypeMaping DbDataTypeMaping { get { return dbDataTypeMaping; } }

        public event ZeroDbs.Common.DbExecuteSqlEvent OnDbExecuteSqlEvent = null;
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
            return new MySqlConnection(DbConfigDatabaseInfo.dbConnectionString);
        }
        public IDbCommand GetDbCommand()
        {
            var cmd = new MySqlCommand();
            cmd.Connection = new MySqlConnection(DbConfigDatabaseInfo.dbConnectionString);
            cmd.Connection.Open();
            return new ZeroDbs.Common.DbCommand(DbConfigDatabaseInfo.dbKey, cmd, this.OnDbExecuteSqlEvent, this.DbSqlBuilder);
        }
        public IDbCommand GetDbCommand(System.Data.Common.DbConnection dbConnection)
        {
            var cmd = new MySqlCommand();
            cmd.Connection = (MySqlConnection)dbConnection;
            if (cmd.Connection.State != System.Data.ConnectionState.Open)
            {
                cmd.Connection.Open();
            }
            return new ZeroDbs.Common.DbCommand(DbConfigDatabaseInfo.dbKey, cmd, this.OnDbExecuteSqlEvent, this.DbSqlBuilder);
        }
        public IDbCommand GetDbCommand(System.Data.Common.DbTransaction dbTransaction)
        {
            var cmd = new MySqlCommand();
            cmd.Connection = (MySqlConnection)dbTransaction.Connection;
            if (cmd.Connection.State != System.Data.ConnectionState.Open)
            {
                cmd.Connection.Open();
            }
            cmd.Transaction = (MySqlTransaction)dbTransaction;
            return new ZeroDbs.Common.DbCommand(DbConfigDatabaseInfo.dbKey, cmd, this.OnDbExecuteSqlEvent, this.DbSqlBuilder);
        }
        public IDbTransactionScope GetDbTransactionScope(System.Data.IsolationLevel level, string identification="", string groupId="")
        {
            var conn = this.GetDbConnection();
            conn.Open();
            var trans = conn.BeginTransaction(level);
            return new ZeroDbs.Common.DbTransactionScope(this, identification, groupId);
        }
        public IDbTransactionScopeCollection GetDbTransactionScopeCollection()
        {
            return new ZeroDbs.Common.DbTransactionScopeCollection();
        }
        public ZeroDbs.Common.DbDataTableInfo GetTable<T>() where T : class, new()
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
                var dbName = cmd.DbConnection.Database;
                var dv = Common.DbMapping.GetDbConfigDataViewInfo<T>().Find(o => string.Equals(o.dbKey, DbConfigDatabaseInfo.dbKey, StringComparison.OrdinalIgnoreCase));
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
        public List<ZeroDbs.Common.DbDataTableInfo> GetTables()
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

        public IDbCommand GetDbCommand<T>() where T : class, new()
        {
            if (!IsMappingToDbKey<T>())
            {
                throw new Exception("类型" + typeof(T).FullName + "没有映射到" + DbConfigDatabaseInfo.dbKey + "上");
            }
            return GetDbCommand();
        }
        public IDbTransactionScope GetDbTransactionScope<T>(System.Data.IsolationLevel level, string identification = "", string groupId = "") where T : class, new()
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
            catch(Exception ex)
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
