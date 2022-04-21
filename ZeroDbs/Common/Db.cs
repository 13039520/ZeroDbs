using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZeroDbs.Common
{
    /// <summary>
    /// based SqlServer
    /// </summary>
    public abstract class Db : IDb
    {
        private IDataTypeMaping dbDataTypeMaping = null;
        private Common.DatabaseInfo database = null;
        private Common.SqlBuilder dbSqlBuilder = null;
        public Common.DatabaseInfo Database { get { return database; } }
        public Common.SqlBuilder DbSqlBuilder { get { return dbSqlBuilder; } }
        public IDataTypeMaping DbDataTypeMaping { get { return dbDataTypeMaping; } }

        public event ZeroDbs.Common.DbExecuteSqlEvent OnDbExecuteSqlEvent = null;
        public Db(Common.DatabaseInfo database)
        {
            this.database = database;
            if (string.Equals("SqlServer", database.dbType, StringComparison.OrdinalIgnoreCase))
            {
                this.dbSqlBuilder = new SqlServer.SqlBuilder(this);
                this.dbDataTypeMaping=new SqlServer.DbDataTypeMaping();
            }else if (string.Equals("MySql", database.dbType, StringComparison.OrdinalIgnoreCase))
            {
                this.dbSqlBuilder = new MySql.SqlBuilder(this);
                this.dbDataTypeMaping = new MySql.DbDataTypeMaping();
            }
            else if (string.Equals("Sqlite", database.dbType, StringComparison.OrdinalIgnoreCase))
            {
                this.dbSqlBuilder = new Sqlite.SqlBuilder(this);
                this.dbDataTypeMaping = new Sqlite.DbDataTypeMaping();
            }
            else
            {
                this.dbSqlBuilder = new SqlServer.SqlBuilder(this);
                this.dbDataTypeMaping = new SqlServer.DbDataTypeMaping();
            }
        }
        public void FireZeroDbExecuteSqlEvent(ZeroDbs.Common.DbExecuteSqlEventArgs args)
        {
            if (this.OnDbExecuteSqlEvent != null)
            {
                this.OnDbExecuteSqlEvent(this, args);
            }
        }
        protected bool IsMappingToDbKey<DbEntity>()
        {
            var temp = Common.DbMapping.GetZeroDbConfigDatabaseInfo<DbEntity>();
            if (temp == null || temp.Count < 1)
            {
                return false;
            }
            return null != temp.Find(o => string.Equals(o.dbKey, Database.dbKey, StringComparison.OrdinalIgnoreCase));
        }

        public virtual System.Data.Common.DbConnection GetDbConnection()
        {
            throw new NotImplementedException();
        }
        public virtual ZeroDbs.Common.DbDataTableInfo GetTable<DbEntity>() where DbEntity : class, new()
        {
            throw new NotImplementedException();
        }
        public virtual List<ZeroDbs.Common.DbDataTableInfo> GetTables()
        {
            throw new NotImplementedException();
        }

        public IDbCommand GetDbCommand()
        {
            var conn = this.GetDbConnection();
            if (conn.State != System.Data.ConnectionState.Open)
            {
                conn.Open();
            }
            var cmd = conn.CreateCommand();
            return new ZeroDbs.Common.DbCommand(Database.dbKey, cmd, this.OnDbExecuteSqlEvent, this.DbSqlBuilder);
        }
        public IDbCommand GetDbCommand(System.Data.Common.DbTransaction transaction)
        {
            if (transaction.Connection.State != System.Data.ConnectionState.Open)
            {
                transaction.Connection.Open();
            }
            System.Data.Common.DbCommand cmd = transaction.Connection.CreateCommand();
            cmd.Connection = transaction.Connection;
            cmd.Transaction = transaction;

            return new ZeroDbs.Common.DbCommand(Database.dbKey, cmd, this.OnDbExecuteSqlEvent, this.DbSqlBuilder);
        }
        public IDbTransactionScope GetDbTransactionScope(System.Data.IsolationLevel level, string identification = "", string groupId = "")
        {
            return new ZeroDbs.Common.DbTransactionScope(this, level, identification, groupId);
        }
        public IDbTransactionScopeCollection GetDbTransactionScopeCollection()
        {
            return new ZeroDbs.Common.DbTransactionScopeCollection();
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
            catch (Exception e)
            {
                throw e;
            }
        }

        public IDbCommand GetDbCommand<DbEntity>() where DbEntity : class, new()
        {
            if (!IsMappingToDbKey<DbEntity>())
            {
                throw new Exception("类型" + typeof(DbEntity).FullName + "没有映射到" + Database.dbKey + "上");
            }
            return GetDbCommand();
        }
        public IDbTransactionScope GetDbTransactionScope<DbEntity>(System.Data.IsolationLevel level, string identification = "", string groupId = "") where DbEntity : class, new()
        {
            if (!IsMappingToDbKey<DbEntity>())
            {
                throw new Exception("类型" + typeof(DbEntity).FullName + "没有映射到" + Database.dbKey + "上");
            }
            return GetDbTransactionScope(level, identification, groupId);
        }

        public List<IntoEntity> Select<DbEntity, IntoEntity>(string where, string orderby, int top, params object[] paras) where DbEntity : class, new() where IntoEntity : class, new()
        {
            string[] fieldNames = typeof(IntoEntity).GetProperties().Select(o => o.Name).ToArray();
            var sql = DbSqlBuilder.Select<DbEntity>(where, orderby, top, fieldNames, paras);
            var cmd = GetDbCommand();
            try
            {
                cmd.CommandText = sql.Sql;
                cmd.ParametersFromDictionary(sql.Paras);
                List<IntoEntity> reval = cmd.ExecuteReader<IntoEntity>();
                cmd.Dispose();
                return reval;
            }
            catch (Exception ex)
            {
                cmd.Dispose();
                throw ex;
            }
        }
        public List<DbEntity> Select<DbEntity>(string where, string orderby, int top, string[] fieldNames, params object[] paras) where DbEntity : class, new()
        {
            var sql = DbSqlBuilder.Select<DbEntity>(where, orderby, top, fieldNames, paras);
            var cmd = GetDbCommand();
            try
            {
                cmd.CommandText = sql.Sql;
                cmd.ParametersFromDictionary(sql.Paras);
                List<DbEntity> reval = cmd.ExecuteReader<DbEntity>();
                cmd.Dispose();
                return reval;
            }
            catch (Exception ex)
            {
                cmd.Dispose();
                throw ex;
            }
        }

        public ZeroDbs.Common.PageData<DbEntity> Page<DbEntity>(long page, long size, string where, string orderby, string[] fieldNames, string uniqueFieldName) where DbEntity : class, new()
        {
            var countSql = DbSqlBuilder.Count<DbEntity>(where);
            var sql = DbSqlBuilder.Page<DbEntity>(page, size, where, orderby, fieldNames, uniqueFieldName);
            var cmd = this.GetDbCommand();
            try
            {
                cmd.CommandText = countSql;
                var obj = cmd.ExecuteScalar();
                long total = Convert.ToInt64(obj);
                if (total < 1)
                {
                    cmd.Dispose();
                    return new Common.PageData<DbEntity> { Total = total, Items = new List<DbEntity>() };
                }
                long pages = total % size == 0 ? total / size : (total / size + 1);
                if (page > pages)
                {
                    cmd.Dispose();
                    return new Common.PageData<DbEntity> { Total = total, Items = new List<DbEntity>() };
                }
                cmd.CommandText = sql;
                var reval = cmd.ExecuteReader<DbEntity>();
                cmd.Dispose();

                return new Common.PageData<DbEntity> { Total = total, Items = reval };
            }
            catch (Exception ex)
            {
                cmd.Dispose();

                throw ex;
            }
        }

        public long Count<DbEntity>(string where) where DbEntity : class, new()
        {
            var sql = DbSqlBuilder.Count<DbEntity>(where);
            var cmd = this.GetDbCommand();
            try
            {
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

        public int Insert<DbEntity>(DbEntity entity) where DbEntity : class, new()
        {
            var sql = DbSqlBuilder.Insert<DbEntity>();
            var cmd = this.GetDbCommand();
            try
            {
                cmd.CommandText = sql;
                cmd.ParametersFromEntity(entity);
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
        public int Insert<DbEntity>(List<DbEntity> entityList) where DbEntity : class, new()
        {
            var sql = DbSqlBuilder.Insert<DbEntity>();
            var ts = this.GetDbTransactionScope(System.Data.IsolationLevel.ReadUncommitted);
            try
            {
                int reval = 0;
                ts.Execute((cmd) =>
                {
                    cmd.CommandText = sql;
                    foreach (var entity in entityList)
                    {
                        cmd.ParametersFromEntity(entity);
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
        public int Insert<DbEntity>(System.Collections.Specialized.NameValueCollection nvc) where DbEntity : class, new()
        {
            var sql = this.DbSqlBuilder.Insert<DbEntity>(nvc);
            var cmd = this.GetDbCommand();
            try
            {
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
        public int Insert<DbEntity>(List<System.Collections.Specialized.NameValueCollection> nvcList) where DbEntity : class, new()
        {
            var sqlList = this.DbSqlBuilder.Insert<DbEntity>(nvcList);
            var ts = this.GetDbTransactionScope(System.Data.IsolationLevel.ReadUncommitted);
            try
            {
                int reval = 0;
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

        public int Update<DbEntity>(DbEntity entity) where DbEntity : class, new()
        {
            var sql = this.DbSqlBuilder.Update<DbEntity>();
            var cmd = this.GetDbCommand();
            try
            {
                cmd.CommandText = sql;
                cmd.ParametersFromEntity(entity);
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
        public int Update<DbEntity>(List<DbEntity> entityList) where DbEntity : class, new()
        {
            var sql = this.DbSqlBuilder.Update<DbEntity>();
            var ts = this.GetDbTransactionScope(System.Data.IsolationLevel.ReadUncommitted);
            try
            {
                int reval = 0;
                ts.Execute((cmd) =>
                {
                    cmd.CommandText = sql;
                    foreach (var entity in entityList)
                    {
                        cmd.ParametersFromEntity(entity);
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
        public int UpdateFromNameValueCollection<DbEntity>(System.Collections.Specialized.NameValueCollection nvc) where DbEntity : class, new()
        {
            var sql = this.DbSqlBuilder.UpdateFromNameValueCollection<DbEntity>(nvc);
            var cmd = this.GetDbCommand();
            try
            {
                cmd.CommandText = sql.Sql;
                cmd.ParametersFromDictionary(sql.Paras);
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
        public int UpdateFromCustomEntity<DbEntity>(object source) where DbEntity : class, new()
        {
            var sql = this.DbSqlBuilder.UpdateFromCustomEntity<DbEntity>(source);
            var cmd = this.GetDbCommand();
            try
            {
                cmd.CommandText = sql.Sql;
                cmd.ParametersFromDictionary(sql.Paras);
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
        public int UpdateFromDictionary<DbEntity>(Dictionary<string, object> dic) where DbEntity : class, new()
        {
            var sql = this.DbSqlBuilder.UpdateFromDictionary<DbEntity>(dic);
            var cmd = this.GetDbCommand();
            try
            {
                cmd.CommandText = sql.Sql;
                cmd.ParametersFromDictionary(sql.Paras);
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

        public int Delete<DbEntity>(string where, params object[] paras) where DbEntity : class, new()
        {
            var sql = this.DbSqlBuilder.Delete<DbEntity>(where, paras);
            var cmd = this.GetDbCommand();
            try
            {
                cmd.CommandText = sql.Sql;
                cmd.ParametersFromDictionary(sql.Paras);
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

    }
}
