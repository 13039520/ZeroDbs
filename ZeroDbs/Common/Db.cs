using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;

namespace ZeroDbs.Common
{
    /// <summary>
    /// based SqlServer
    /// </summary>
    public abstract class Db : IDb
    {
        private IDataTypeMaping dataTypeMaping = null;
        private IDbInfo database = null;
        private SqlBuilder sqlBuilder = null;
        public IDbInfo Database { get { return database; } }
        public SqlBuilder SqlBuilder { get { return sqlBuilder; } }
        public IDataTypeMaping DataTypeMaping { get { return dataTypeMaping; } }

        public event DbExecuteHandler OnDbExecuteSqlEvent = null;
        public Db(IDbInfo database)
        {
            this.database = database;
            switch (this.database.Type)
            {
                case DbType.SqlServer:
                    this.sqlBuilder = new SqlServer.SqlBuilder(this);
                    this.dataTypeMaping = new SqlServer.DbDataTypeMaping();
                    break;
                case DbType.MySql:
                    this.sqlBuilder = new MySql.SqlBuilder(this);
                    this.dataTypeMaping = new MySql.DbDataTypeMaping();
                    break;
                case DbType.Sqlite:
                    this.sqlBuilder = new Sqlite.SqlBuilder(this);
                    this.dataTypeMaping = new Sqlite.DbDataTypeMaping();
                    break;
                default:
                    throw new Exception("Unsupported database type");
            }
        }
        public void FireZeroDbExecuteSqlEvent(DbExecuteArgs args)
        {
            if (this.OnDbExecuteSqlEvent != null)
            {
                this.OnDbExecuteSqlEvent(this, args);
            }
        }
        protected bool IsMappingToDbKey<DbEntity>()
        {
            var temp = DbMapping.GetDbInfo<DbEntity>();
            if (temp == null || temp.Count < 1)
            {
                return false;
            }
            return null != temp.Find(o => string.Equals(o.Key, Database.Key, StringComparison.OrdinalIgnoreCase));
        }
        protected bool IsMappingToDbKey(string entityFullName)
        {
            var temp = DbMapping.GetDbInfoByEntityFullName(entityFullName);
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
        public virtual ITableInfo GetTable<DbEntity>() where DbEntity : class, new()
        {
            throw new NotImplementedException();
        }
        public virtual ITableInfo GetTable(string entityFullName)
        {
            throw new NotImplementedException();
        }
        public virtual List<ITableInfo> GetTables()
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

        public List<Target> Select<DbEntity, Target>(ListQuery query) where DbEntity : class, new() where Target : class, new()
        {
            return Select<DbEntity, Target>(query.Where, query.Orderby, query.Top, query.Paras);
        }
        public List<DbEntity> Select<DbEntity>(ListQuery query) where DbEntity : class, new()
        {
            return Select<DbEntity>(query.Where, query.Orderby, query.Top, query.Fields, query.Paras);
        }
        public List<OutType> Select<DbEntity, OutType>() where DbEntity : class, new() where OutType : class, new()
        {
            return Select<DbEntity, OutType>("");
        }
        public List<OutType> Select<DbEntity, OutType>(string where) where DbEntity : class, new() where OutType : class, new()
        {
            return Select<DbEntity, OutType>(where, "");
        }
        public List<OutType> Select<DbEntity, OutType>(string where, string orderby) where DbEntity : class, new() where OutType : class, new()
        {
            return Select<DbEntity, OutType>(where, orderby, 0);
        }
        public List<OutType> Select<DbEntity, OutType>(string where, string orderby, int top) where DbEntity : class, new() where OutType : class, new()
        {
            return Select<DbEntity, OutType>(where, orderby, top, new object[0]);
        }
        public List<OutType> Select<DbEntity, OutType>(string where, string orderby, int top, params object[] paras) where DbEntity : class, new() where OutType : class, new()
        {
            string[] fields = PropertyInfoCache.GetPropertyInfoList<OutType>().Select(o => o.Name).ToArray();
            var info = SqlBuilder.Select<DbEntity>(where, orderby, top, fields, paras);
            using (var cmd = GetDbCommand())
            {
                return cmd.ExecuteQuery<OutType>(info);
            }
        }
        public List<DbEntity> Select<DbEntity>() where DbEntity : class, new()
        {
            return Select<DbEntity>("");
        }
        public List<DbEntity> Select<DbEntity>(string where) where DbEntity : class, new()
        {
            return Select<DbEntity>(where, "");
        }
        public List<DbEntity> Select<DbEntity>(string where, string orderby) where DbEntity : class, new()
        {
            return Select<DbEntity>(where, orderby, 0);
        }
        public List<DbEntity> Select<DbEntity>(string where, string orderby, int top) where DbEntity : class, new()
        {
            return Select<DbEntity>(where, orderby, top, new string[0]);
        }
        public List<DbEntity> Select<DbEntity>(string where, string orderby, int top, string[] fields) where DbEntity : class, new()
        {
            return Select<DbEntity>(where, orderby, top, fields, new object[0]);
        }
        public List<DbEntity> Select<DbEntity>(string where, string orderby, int top, string[] fields, params object[] paras) where DbEntity : class, new()
        {
            var info = SqlBuilder.Select<DbEntity>(where, orderby, top, fields, paras);
            using (var cmd = GetDbCommand())
            {
                return cmd.ExecuteQuery<DbEntity>(info);
            }
        }
        public DbEntity SelectByPrimaryKey<DbEntity>(object key) where DbEntity : class, new()
        {
            var info = SqlBuilder.SelectByPrimaryKey<DbEntity>(key);
            var cmd = GetDbCommand();
            try
            {
                DbEntity reval = null;
                cmd.CommandText = info.Sql;
                cmd.ParametersFromDictionary(info.Paras);
                cmd.ExecuteReader<DbEntity>((e) => {
                    reval = e.RowData;
                    e.Next = false;
                }, true);
                cmd.Dispose();
                return reval;
            }
            catch (Exception ex)
            {
                cmd.Dispose();
                throw ex;
            }
        }

        public PageData<DbEntity> Page<DbEntity>(PageQuery query) where DbEntity : class, new()
        {
            return Page<DbEntity>(query.Page, query.Size, query.Where, query.Orderby, query.Fields, query.Unique, query.Paras);
        }
        public PageData<OutType> Page<DbEntity, OutType>(PageQuery query) where DbEntity : class, new() where OutType : class, new()
        {
            return Page<DbEntity, OutType>(query.Page, query.Size, query.Where, query.Orderby, query.Unique, query.Paras);
        }
        public PageData<OutType> Page<DbEntity, OutType>(long page, long size) where DbEntity : class, new() where OutType : class, new()
        {
            return Page<DbEntity, OutType>(page, size, "");
        }
        public PageData<OutType> Page<DbEntity, OutType>(long page, long size, string where) where DbEntity : class, new() where OutType : class, new()
        {
            return Page<DbEntity, OutType>(page, size, where, "");
        }
        public PageData<OutType> Page<DbEntity, OutType>(long page, long size, string where, string orderby) where DbEntity : class, new() where OutType : class, new()
        {
            return Page<DbEntity, OutType>(page, size, where, orderby, "");
        }
        public PageData<OutType> Page<DbEntity, OutType>(long page, long size, string where, string orderby, string uniqueField) where DbEntity : class, new() where OutType : class, new()
        {
            return Page<DbEntity, OutType>(page, size, where, orderby, uniqueField, new object[0]);
        }
        public PageData<OutType> Page<DbEntity, OutType>(long page, long size, string where, string orderby, string uniqueField, params object[] paras) where DbEntity : class, new() where OutType : class, new()
        {
            string[] fields = PropertyInfoCache.GetPropertyInfoList<OutType>().Select(o => o.Name).ToArray();
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
                    return new PageData<OutType> { Total = total, Items = new List<OutType>() };
                }
                long pages = total % size == 0 ? total / size : (total / size + 1);
                if (page > pages)
                {
                    cmd.Dispose();
                    return new PageData<OutType> { Total = total, Items = new List<OutType>() };
                }
                var reval = cmd.ExecuteQuery<OutType>(info);
                cmd.Dispose();

                return new PageData<OutType> { Total = total, Items = reval };
            }
            catch (Exception ex)
            {
                cmd.Dispose();

                throw ex;
            }
        }
        public PageData<DbEntity> Page<DbEntity>(long page, long size) where DbEntity : class, new()
        {
            return Page<DbEntity>(page, size, "");
        }
        public PageData<DbEntity> Page<DbEntity>(long page, long size, string where) where DbEntity : class, new()
        {
            return Page<DbEntity>(page, size, where, "");
        }
        public PageData<DbEntity> Page<DbEntity>(long page, long size, string where, string orderby) where DbEntity : class, new()
        {
            return Page<DbEntity>(page, size, where, orderby, new string[0]);
        }
        public PageData<DbEntity> Page<DbEntity>(long page, long size, string where, string orderby, string[] fields) where DbEntity : class, new()
        {
            return Page<DbEntity>(page, size, where, orderby, fields, "");
        }
        public PageData<DbEntity> Page<DbEntity>(long page, long size, string where, string orderby, string[] fields, string uniqueField) where DbEntity : class, new()
        {
            return Page<DbEntity>(page, size, where, orderby, fields, uniqueField, new object[0]);
        }
        public PageData<DbEntity> Page<DbEntity>(long page, long size, string where, string orderby, string[] fields, string uniqueField, params object[] paras) where DbEntity : class, new()
        {
            var countSql = SqlBuilder.Count<DbEntity>(where,paras);
            var info = SqlBuilder.Page<DbEntity>(page, size, where, orderby, fields, uniqueField,paras);
            PageData<DbEntity> reval = new PageData<DbEntity>();
            using (var cmd = this.GetDbCommand())
            {
                cmd.CommandText = countSql.Sql;
                cmd.ParametersFromDictionary(countSql.Paras);
                var obj = cmd.ExecuteScalar();
                long total = Convert.ToInt64(obj);
                reval.Total = total;
                if (total < 1)
                {
                    reval.Items = new List<DbEntity>();
                }
                else
                {
                    long pages = total % size == 0 ? total / size : (total / size + 1);
                    if (page > pages)
                    {
                        reval.Items = new List<DbEntity>();
                    }
                    else
                    {
                        reval.Items = cmd.ExecuteQuery<DbEntity>(info);
                    }
                }
            }
            return reval;
        }


        public long Count<DbEntity>(string where, params object[] paras) where DbEntity : class, new()
        {
            return Count(typeof(DbEntity), where, paras);
        }
        public long Count(Type entityType, string where, params object[] paras)
        {
            var info = SqlBuilder.Count(entityType, where, paras);
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
        public long MaxIdentityPrimaryKeyValue<DbEntity>() where DbEntity : class, new()
        {
            return MaxIdentityPrimaryKeyValue(typeof(DbEntity));
        }
        public long MaxIdentityPrimaryKeyValue(Type entityType)
        {
            return MaxIdentityPrimaryKeyValue(entityType,"");
        }
        public long MaxIdentityPrimaryKeyValue<DbEntity>(string where, params object[] paras) where DbEntity : class, new()
        {
            return MaxIdentityPrimaryKeyValue(typeof(DbEntity), where, paras);
        }
        public long MaxIdentityPrimaryKeyValue(Type entityType, string where, params object[] paras)
        {
            long reval = 0;
            var info = this.SqlBuilder.MaxIdentityPrimaryKeyValue(entityType, where, paras);
            using (var cmd = this.GetDbCommand())
            {
                cmd.CommandText = info.Sql;
                cmd.ParametersFromDictionary(info.Paras);
                var obj = cmd.ExecuteScalar();
                if (obj is null || obj is DBNull)
                {
                    reval = 0;
                }
                reval = Convert.ToInt64(obj);
            }
            return reval;
        }


        protected int _NonQuery(SqlInfo info)
        {
            int reval = 0;
            using (var cmd = this.GetDbCommand())
            {
                reval = cmd.ExecuteNonQuery(info);
            }
            return reval;
        }

        public int Insert<DbEntity>(DbEntity entity) where DbEntity : class, new()
        {
            return _NonQuery(this.SqlBuilder.Insert<DbEntity>(entity));
        }
        public int Insert<DbEntity>(List<DbEntity> entities) where DbEntity : class, new()
        {
            return Insert<DbEntity>(entities, 0);
        }
        public int Insert<DbEntity>(List<DbEntity> entities, int mergeLimit) where DbEntity : class, new()
        {
            int reval = 0;
            if (mergeLimit < 1)
            {
                var sql = this.SqlBuilder.Insert<DbEntity>();
                using (var ts = this.GetDbTransactionScope(System.Data.IsolationLevel.ReadUncommitted))
                {
                    ts.Execute((cmd) =>
                    {
                        cmd.CommandText = sql;
                        foreach (var entity in entities)
                        {
                            cmd.ParametersFromEntity(entity);
                            reval += cmd.ExecuteNonQuery();
                        }
                    });
                    ts.Complete(true);
                }
            }
            else
            {
                var infos = this.SqlBuilder.Insert<DbEntity>(entities, mergeLimit);
                using (var ts = this.GetDbTransactionScope(System.Data.IsolationLevel.ReadUncommitted))
                {
                    ts.Execute((cmd) =>
                    {
                        foreach (var info in infos)
                        {
                            reval += cmd.ExecuteNonQuery(info);
                        }
                    });
                    ts.Complete(true);
                }
            }
            return reval;
        }
        public int InsertByNameValueCollection<DbEntity>(NameValueCollection source) where DbEntity : class, new()
        {
            return InsertByNameValueCollection(typeof(DbEntity), source);
        }
        public int InsertByNameValueCollection(Type entityType, NameValueCollection source)
        {
            return _NonQuery(this.SqlBuilder.InsertByNameValueCollection(entityType, source));
        }
        public int InsertByCustomEntity<DbEntity>(object source) where DbEntity : class, new()
        {
            return InsertByCustomEntity(typeof(DbEntity), source);
        }
        public int InsertByCustomEntity(Type entityType, object source)
        {
            return _NonQuery(this.SqlBuilder.InsertByCustomEntity(entityType, source));
        }
        public int InsertByDictionary<DbEntity>(Dictionary<string, object> source) where DbEntity : class, new()
        {
            return InsertByDictionary(typeof(DbEntity), source);
        }
        public int InsertByDictionary(Type entityType, Dictionary<string, object> source)
        {
            return _NonQuery(this.SqlBuilder.InsertByDictionary(entityType, source));
        }

        public int Update<DbEntity>(DbEntity entity) where DbEntity : class, new()
        {
            return Update<DbEntity>(entity, "");
        }
        public int Update<DbEntity>(DbEntity entity, string appendWhere, params object[] paras) where DbEntity : class, new()
        {
            return _NonQuery(this.SqlBuilder.Update<DbEntity>(entity, appendWhere, paras));
        }
        public int Update<DbEntity>(List<DbEntity> entities) where DbEntity : class, new()
        {
            return Update<DbEntity>(entities, "");
        }
        public int Update<DbEntity>(List<DbEntity> entities, string appendWhere, params object[] paras) where DbEntity : class, new()
        {
            if(entities == null || entities.Count < 1) { return 0; }
            var info = this.SqlBuilder.Update<DbEntity>(entities[0], appendWhere, paras);
            using (var ts = this.GetDbTransactionScope(System.Data.IsolationLevel.ReadUncommitted))
            {
                int reval = 0;
                ts.Execute((cmd) =>
                {
                    reval += cmd.ExecuteNonQuery(info);
                    for (int i = 1; i < entities.Count; i++)
                    {
                        reval += cmd.ExecuteNonQuery(this.SqlBuilder.Update<DbEntity>(entities[i], appendWhere, paras));
                    }
                });
                ts.Complete(true);
                return reval;
            }
        }
        public int UpdateByNameValueCollection<DbEntity>(NameValueCollection source) where DbEntity : class, new()
        {
            return UpdateByNameValueCollection<DbEntity>(source, "");
        }
        public int UpdateByNameValueCollection<DbEntity>(NameValueCollection source, string appendWhere, params object[] paras) where DbEntity : class, new()
        {
            return UpdateByNameValueCollection(typeof(DbEntity), source, appendWhere, paras);
        }
        public int UpdateByNameValueCollection(Type entityType, NameValueCollection source, string appendWhere, params object[] paras)
        {
            return _NonQuery(this.SqlBuilder.UpdateByNameValueCollection(entityType, source, appendWhere, paras));
        }
        public int UpdateByCustomEntity<DbEntity>(object source) where DbEntity : class, new()
        {
            return UpdateByCustomEntity<DbEntity>(source, "");
        }
        public int UpdateByCustomEntity<DbEntity>(object source, string appendWhere, params object[] paras) where DbEntity : class, new()
        {
            return UpdateByCustomEntity(typeof(DbEntity), source, appendWhere, paras);
        }
        public int UpdateByCustomEntity(Type entityType, object source, string appendWhere, params object[] paras)
        {
            return _NonQuery(this.SqlBuilder.UpdateByCustomEntity(entityType, source, appendWhere, paras));
        }
        public int UpdateByDictionary<DbEntity>(Dictionary<string, object> source) where DbEntity : class, new()
        {
            return UpdateByDictionary<DbEntity>(source, "");
        }
        public int UpdateByDictionary<DbEntity>(Dictionary<string, object> source, string appendWhere, params object[] paras) where DbEntity : class, new()
        {
            return UpdateByDictionary(typeof(DbEntity), source, appendWhere, paras);
        }
        public int UpdateByDictionary(Type entityType, Dictionary<string, object> source, string appendWhere, params object[] paras)
        {
            return _NonQuery(this.SqlBuilder.UpdateByDictionary(entityType, source, appendWhere, paras));
        }


        public int Delete<DbEntity>(string where, params object[] paras) where DbEntity : class, new()
        {
            return _NonQuery(this.SqlBuilder.Delete<DbEntity>(where, paras));
        }
        public int Delete<DbEntity>(DbEntity source) where DbEntity : class, new()
        {
            return _NonQuery(this.SqlBuilder.Delete<DbEntity>(source));
        }
        public int Delete(Type entityType, string where, params object[] paras)
        {
            return _NonQuery(this.SqlBuilder.Delete(entityType, where, paras));
        }
        public int DeleteByPrimaryKey<DbEntity>(object key) where DbEntity : class, new()
        {
            return DeleteByPrimaryKey(typeof(DbEntity), key);
        }
        public int DeleteByPrimaryKey(Type entityType, object key)
        {
            return _NonQuery(this.SqlBuilder.DeleteByPrimaryKey(entityType, key));
        }
        public int DeleteByCustomEntity<DbEntity>(object source) where DbEntity : class, new()
        {
            return DeleteByCustomEntity(typeof(DbEntity), source);
        }
        public int DeleteByCustomEntity(Type entityType, object source)
        {
            return _NonQuery(this.SqlBuilder.DeleteByCustomEntity(entityType, source));
        }
        public int DeleteByDictionary<DbEntity>(Dictionary<string, object> source) where DbEntity : class, new()
        {
            return DeleteByDictionary(typeof(DbEntity), source);
        }
        public int DeleteByDictionary(Type entityType, Dictionary<string, object> source)
        {
            return _NonQuery(this.SqlBuilder.DeleteByDictionary(entityType, source));
        }
        public int DeleteByNameValueCollection<DbEntity>(NameValueCollection source) where DbEntity : class, new()
        {
            return DeleteByNameValueCollection(typeof(DbEntity), source);
        }
        public int DeleteByNameValueCollection(Type entityType, NameValueCollection source)
        {
            return _NonQuery(this.SqlBuilder.DeleteByNameValueCollection(entityType, source));
        }



    }
}
