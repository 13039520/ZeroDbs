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
        private IDataTypeMaping dataTypeMaping = null;
        private Common.DbInfo database = null;
        private Common.SqlBuilder sqlBuilder = null;
        public Common.DbInfo Database { get { return database; } }
        public Common.SqlBuilder SqlBuilder { get { return sqlBuilder; } }
        public IDataTypeMaping DataTypeMaping { get { return dataTypeMaping; } }

        public event DbExecuteSqlEvent OnDbExecuteSqlEvent = null;
        public Db(Common.DbInfo database)
        {
            this.database = database;
            switch (this.database.Type)
            {
                case Common.DbType.SqlServer:
                    this.sqlBuilder = new SqlServer.SqlBuilder(this);
                    this.dataTypeMaping = new SqlServer.DbDataTypeMaping();
                    break;
                case Common.DbType.MySql:
                    this.sqlBuilder = new MySql.SqlBuilder(this);
                    this.dataTypeMaping = new MySql.DbDataTypeMaping();
                    break;
                case Common.DbType.Sqlite:
                    this.sqlBuilder = new Sqlite.SqlBuilder(this);
                    this.dataTypeMaping = new Sqlite.DbDataTypeMaping();
                    break;
                default:
                    throw new Exception("Unsupported database type");
            }
        }
        public void FireZeroDbExecuteSqlEvent(DbExecuteSqlEventArgs args)
        {
            if (this.OnDbExecuteSqlEvent != null)
            {
                this.OnDbExecuteSqlEvent(this, args);
            }
        }
        protected bool IsMappingToDbKey<DbEntity>()
        {
            var temp = Common.DbMapping.GetDbInfo<DbEntity>();
            if (temp == null || temp.Count < 1)
            {
                return false;
            }
            return null != temp.Find(o => string.Equals(o.Key, Database.Key, StringComparison.OrdinalIgnoreCase));
        }

        public virtual System.Data.Common.DbConnection GetDbConnection()
        {
            throw new NotImplementedException();
        }
        public virtual DbDataTableInfo GetTable<DbEntity>() where DbEntity : class, new()
        {
            throw new NotImplementedException();
        }
        public virtual List<DbDataTableInfo> GetTables()
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
            return new DbCommand(Database.Key, cmd, this.OnDbExecuteSqlEvent, this.SqlBuilder);
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

            return new DbCommand(Database.Key, cmd, this.OnDbExecuteSqlEvent, this.SqlBuilder);
        }
        public IDbTransactionScope GetDbTransactionScope(System.Data.IsolationLevel level, string identification = "", string groupId = "")
        {
            return new DbTransactionScope(this, level, identification, groupId);
        }
        public IDbTransactionScopeCollection GetDbTransactionScopeCollection()
        {
            return new DbTransactionScopeCollection();
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
                throw new Exception("类型" + typeof(DbEntity).FullName + "没有映射到" + Database.Key + "上");
            }
            return GetDbCommand();
        }
        public IDbTransactionScope GetDbTransactionScope<DbEntity>(System.Data.IsolationLevel level, string identification = "", string groupId = "") where DbEntity : class, new()
        {
            if (!IsMappingToDbKey<DbEntity>())
            {
                throw new Exception("类型" + typeof(DbEntity).FullName + "没有映射到" + Database.Key + "上");
            }
            return GetDbTransactionScope(level, identification, groupId);
        }

        public List<Target> Select<DbEntity, Target>(Common.ListQuery query) where DbEntity : class, new() where Target : class, new()
        {
            return Select<DbEntity, Target>(query.Where, query.Orderby, query.Top, query.Paras);
        }
        public List<DbEntity> Select<DbEntity>(Common.ListQuery query) where DbEntity : class, new()
        {
            return Select<DbEntity>(query.Where, query.Orderby, query.Top, query.Fields, query.Paras);
        }
        public List<IntoEntity> Select<DbEntity, IntoEntity>(string where, string orderby, int top, params object[] paras) where DbEntity : class, new() where IntoEntity : class, new()
        {
            string[] fields = typeof(IntoEntity).GetProperties().Select(o => o.Name).ToArray();
            var info = SqlBuilder.Select<DbEntity>(where, orderby, top, fields, paras);
            var cmd = GetDbCommand();
            try
            {
                List<IntoEntity> reval = cmd.ExecuteQuery<IntoEntity>(info);
                cmd.Dispose();
                return reval;
            }
            catch (Exception ex)
            {
                cmd.Dispose();
                throw ex;
            }
        }
        public List<DbEntity> Select<DbEntity>(string where, string orderby, int top, string[] fields, params object[] paras) where DbEntity : class, new()
        {
            var info = SqlBuilder.Select<DbEntity>(where, orderby, top, fields, paras);
            var cmd = GetDbCommand();
            try
            {
                List<DbEntity> reval = cmd.ExecuteQuery<DbEntity>(info);
                cmd.Dispose();
                return reval;
            }
            catch (Exception ex)
            {
                cmd.Dispose();
                throw ex;
            }
        }

        public Common.PageData<DbEntity> Page<DbEntity>(Common.PageQuery query) where DbEntity : class, new()
        {
            return Page<DbEntity>(query.Page, query.Size, query.Where, query.Orderby, query.Fields, query.UniqueField, query.Paras);
        }
        public Common.PageData<IntoEntity> Page<DbEntity, IntoEntity>(Common.PageQuery query) where DbEntity : class, new() where IntoEntity : class, new()
        {
            return Page<DbEntity, IntoEntity>(query.Page, query.Size, query.Where, query.Orderby, query.UniqueField, query.Paras);
        }
        public Common.PageData<IntoEntity> Page<DbEntity, IntoEntity>(long page, long size, string where, string orderby, string uniqueField, params object[] paras) where DbEntity : class, new() where IntoEntity : class, new()
        {
            string[] fields = typeof(IntoEntity).GetProperties().Select(o => o.Name).ToArray();
            var countSql = SqlBuilder.Count<DbEntity>(where, paras);
            var info = SqlBuilder.Page<DbEntity>(page, size, where, orderby, fields, uniqueField, paras);
            var cmd = this.GetDbCommand();
            try
            {
                cmd.CommandText = countSql.Sql;
                cmd.ParametersFromDictionary(countSql.Paras);
                var obj = cmd.ExecuteScalar();
                long total = Convert.ToInt64(obj);
                if (total < 1)
                {
                    cmd.Dispose();
                    return new Common.PageData<IntoEntity> { Total = total, Items = new List<IntoEntity>() };
                }
                long pages = total % size == 0 ? total / size : (total / size + 1);
                if (page > pages)
                {
                    cmd.Dispose();
                    return new Common.PageData<IntoEntity> { Total = total, Items = new List<IntoEntity>() };
                }
                var reval = cmd.ExecuteQuery<IntoEntity>(info);
                cmd.Dispose();

                return new Common.PageData<IntoEntity> { Total = total, Items = reval };
            }
            catch (Exception ex)
            {
                cmd.Dispose();

                throw ex;
            }
        }
        public PageData<DbEntity> Page<DbEntity>(long page, long size, string where, string orderby, string[] fields, string uniqueField, params object[] paras) where DbEntity : class, new()
        {
            var countSql = SqlBuilder.Count<DbEntity>(where,paras);
            var info = SqlBuilder.Page<DbEntity>(page, size, where, orderby, fields, uniqueField,paras);
            var cmd = this.GetDbCommand();
            try
            {
                cmd.CommandText = countSql.Sql;
                cmd.ParametersFromDictionary(countSql.Paras);
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
                var reval = cmd.ExecuteQuery<DbEntity>(info);
                cmd.Dispose();

                return new Common.PageData<DbEntity> { Total = total, Items = reval };
            }
            catch (Exception ex)
            {
                cmd.Dispose();
                throw ex;
            }
        }

        public long Count<DbEntity>(string where, params object[] paras) where DbEntity : class, new()
        {
            var info = SqlBuilder.Count<DbEntity>(where);
            var cmd = this.GetDbCommand();
            try
            {
                cmd.CommandText = info.Sql;
                cmd.ParametersFromDictionary(info.Paras);
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
            var info = this.SqlBuilder.Insert<DbEntity>(entity);
            var cmd = this.GetDbCommand();
            try
            {
                var reval = cmd.ExecuteNonQuery(info);
                cmd.Dispose();
                return reval;
            }
            catch (Exception ex)
            {
                cmd.Dispose();
                throw ex;
            }
        }
        public int Insert<DbEntity>(List<DbEntity> entities) where DbEntity : class, new()
        {
            var info = this.SqlBuilder.Insert<DbEntity>();
            var ts = this.GetDbTransactionScope(System.Data.IsolationLevel.ReadUncommitted);
            try
            {
                int reval = 0;
                ts.Execute((cmd) =>
                {
                    cmd.CommandText = info;
                    foreach (var entity in entities)
                    {
                        cmd.ParametersFromEntity(entity);
                        reval += cmd.ExecuteNonQuery();
                    }
                });
                ts.Complete(true);
                return reval;
            }
            catch (Exception ex)
            {
                ts.Dispose();
                throw ex;
            }
        }
        public int InsertFromNameValueCollection<DbEntity>(System.Collections.Specialized.NameValueCollection source) where DbEntity : class, new()
        {
            var info = this.SqlBuilder.InsertFromNameValueCollection<DbEntity>(source);
            var cmd = this.GetDbCommand();
            try
            {
                var reval = cmd.ExecuteNonQuery(info);
                cmd.Dispose();
                return reval;
            }
            catch (Exception ex)
            {
                cmd.Dispose();
                throw ex;
            }
        }
        public int InsertFromCustomEntity<DbEntity>(object source) where DbEntity : class, new()
        {
            var info = this.SqlBuilder.InsertFromCustomEntity<DbEntity>(source);
            var cmd = this.GetDbCommand();
            try
            {
                var reval = cmd.ExecuteNonQuery(info);
                cmd.Dispose();
                return reval;
            }
            catch (Exception ex)
            {
                cmd.Dispose();
                throw ex;
            }
        }
        public int InsertFromDictionary<DbEntity>(Dictionary<string, object> source) where DbEntity : class, new()
        {
            var info = this.SqlBuilder.InsertFromDictionary<DbEntity>(source);
            var cmd = this.GetDbCommand();
            try
            {
                var reval = cmd.ExecuteNonQuery(info);
                cmd.Dispose();
                return reval;
            }
            catch (Exception ex)
            {
                cmd.Dispose();
                throw ex;
            }
        }

        public int Update<DbEntity>(DbEntity entity) where DbEntity : class, new()
        {
            var info = this.SqlBuilder.Update<DbEntity>(entity);
            var cmd = this.GetDbCommand();
            try
            {
                var reval = cmd.ExecuteNonQuery(info);
                cmd.Dispose();
                return reval;
            }
            catch (Exception ex)
            {
                cmd.Dispose();
                throw ex;
            }
        }
        public int Update<DbEntity>(List<DbEntity> entities) where DbEntity : class, new()
        {
            var info = this.SqlBuilder.Update<DbEntity>();
            var ts = this.GetDbTransactionScope(System.Data.IsolationLevel.ReadUncommitted);
            try
            {
                int reval = 0;
                ts.Execute((cmd) =>
                {
                    cmd.CommandText = info;
                    foreach (var entity in entities)
                    {
                        cmd.ParametersFromEntity(entity);
                        reval += cmd.ExecuteNonQuery();
                    }
                });
                ts.Complete(true);
                return reval;
            }
            catch (Exception ex)
            {
                ts.Dispose();
                throw ex;
            }
        }
        public int UpdateFromNameValueCollection<DbEntity>(System.Collections.Specialized.NameValueCollection source) where DbEntity : class, new()
        {
            var info = this.SqlBuilder.UpdateFromNameValueCollection<DbEntity>(source);
            var cmd = this.GetDbCommand();
            try
            {
                var reval = cmd.ExecuteNonQuery(info);
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
            var info = this.SqlBuilder.UpdateFromCustomEntity<DbEntity>(source);
            var cmd = this.GetDbCommand();
            try
            {
                var reval = cmd.ExecuteNonQuery(info);
                cmd.Dispose();
                return reval;
            }
            catch (Exception ex)
            {
                cmd.Dispose();
                throw ex;
            }
        }
        public int UpdateFromDictionary<DbEntity>(Dictionary<string, object> source) where DbEntity : class, new()
        {
            var info = this.SqlBuilder.UpdateFromDictionary<DbEntity>(source);
            var cmd = this.GetDbCommand();
            try
            {
                var reval = cmd.ExecuteNonQuery(info);
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
            var info = this.SqlBuilder.Delete<DbEntity>(where, paras);
            var cmd = this.GetDbCommand();
            try
            {
                var reval = cmd.ExecuteNonQuery(info);
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
