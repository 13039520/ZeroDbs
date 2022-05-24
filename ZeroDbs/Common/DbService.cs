using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;

namespace ZeroDbs.Common
{
    public class DbService : IDbService
    {
        IStrCommon _StrCommon = null;
        DbExecuteHandler _dbExecuteHandler = null;
        static Dictionary<string, IDb> _dbInstanceDic = new Dictionary<string, IDb>();
        static object _lock = new object();

        public IStrCommon StrCommon { get { return _StrCommon; } }


        public DbService()
        {
            _StrCommon = new StrCommon();
        }
        public DbService(DbExecuteHandler handler)
        {
            _dbExecuteHandler = handler != null ? handler : null;
            _StrCommon = new StrCommon();
        }
        private IDb GetDbByEntityFullName(string entityFullName)
        {
            IDb reval = null;
            if (string.IsNullOrEmpty(entityFullName))
            {
                return reval;
            }
            var list = DbMapping.GetDbInfoByEntityFullName(entityFullName);
            if (list == null || list.Count < 1)
            {
                return reval;
            }
            var info = list[0];
            if (!_dbInstanceDic.ContainsKey(info.Key))
            {
                lock (_lock)
                {
                    #region -- code --
                    if (!_dbInstanceDic.ContainsKey(info.Key))
                    {
                        IDb db = DbFactory.Create(info, _dbExecuteHandler);
                        if (db != null)
                        {
                            _dbInstanceDic.Add(info.Key, db);
                            reval = db;
                        }
                    }
                    else
                    {
                        reval = _dbInstanceDic[info.Key];
                    }
                    #endregion
                }
            }
            else
            {
                reval = _dbInstanceDic[info.Key];
            }
            return reval;
        }

        public Dictionary<string, IDb> GetDbs()
        {
            var dbs = new Dictionary<string, IDb>();
            var zeroConfigInfo = DbConfigReader.GetDbConfigInfo();
            if (zeroConfigInfo != null && zeroConfigInfo.Dbs != null && zeroConfigInfo.Dbs.Count > 0)
            {
                foreach (var m in zeroConfigInfo.Dbs)
                {
                    if (!_dbInstanceDic.ContainsKey(m.Key))
                    {
                        lock (_lock)
                        {
                            #region -- code --
                            if (!_dbInstanceDic.ContainsKey(m.Key))
                            {
                                IDb db = DbFactory.Create(m, _dbExecuteHandler);
                                if (db != null)
                                {
                                    _dbInstanceDic.Add(m.Key, db);
                                    dbs.Add(m.Key, db);
                                }
                            }
                            else
                            {
                                dbs.Add(m.Key, _dbInstanceDic[m.Key]);
                            }
                            #endregion
                        }
                    }
                    else
                    {
                        dbs.Add(m.Key, _dbInstanceDic[m.Key]);
                    }
                }
            }
            return dbs;
        }
        public Dictionary<string, IDb> GetDbs(List<IDbInfo> dbConfigList)
        {
            var dbs = new Dictionary<string, IDb>();
            if (dbConfigList != null && dbConfigList.Count > 0)
            {
                foreach (var m in dbConfigList)
                {
                    if (!_dbInstanceDic.ContainsKey(m.Key))
                    {
                        lock (_lock)
                        {
                            #region -- code --
                            if (!_dbInstanceDic.ContainsKey(m.Key))
                            {
                                IDb db = DbFactory.Create(m, _dbExecuteHandler);
                                if (db != null)
                                {
                                    _dbInstanceDic.Add(m.Key, db);
                                    dbs.Add(m.Key, db);
                                }
                            }
                            else
                            {
                                dbs.Add(m.Key, _dbInstanceDic[m.Key]);
                            }
                            #endregion
                        }
                    }
                    else
                    {
                        dbs.Add(m.Key, _dbInstanceDic[m.Key]);
                    }
                }
            }
            return dbs;
        }
        public IDb GetDb<DbEntity>() where DbEntity : class, new()
        {
            return GetDb(typeof(DbEntity).FullName);
        }
        public IDb GetDb(string entityFullName)
        {
            return GetDbByEntityFullName(entityFullName);
        }
        public IDb GetDbByDbKey(string dbKey)
        {
            IDb reval = null;
            var info = DbMapping.GetDbInfo(dbKey);
            if (info == null)
            {
                return null;
            }
            if (!_dbInstanceDic.ContainsKey(info.Key))
            {
                lock (_lock)
                {
                    #region -- code --
                    if (!_dbInstanceDic.ContainsKey(info.Key))
                    {
                        IDb db = DbFactory.Create(info, _dbExecuteHandler);
                        if (db != null)
                        {
                            _dbInstanceDic.Add(info.Key, db);
                            reval = db;
                        }
                    }
                    else
                    {
                        reval = _dbInstanceDic[info.Key];
                    }
                    #endregion
                }
            }
            else
            {
                reval = _dbInstanceDic[info.Key];
            }
            return reval;
        }
        public IDbCommand GetDbCommand<DbEntity>() where DbEntity : class, new()
        {
            return GetDb<DbEntity>().GetDbCommand();
        }
        public IDbCommand GetDbCommand(string entityFullName)
        {
            return GetDb(entityFullName).GetDbCommand();
        }
        public IDbCommand GetDbCommandByDbKey(string dbKey)
        {
            return GetDbByDbKey(dbKey).GetDbCommand();
        }
        public IDbTransactionScope GetDbTransactionScope<DbEntity>(System.Data.IsolationLevel level, string identification = "", string groupId = "") where DbEntity : class, new()
        {
            return this.GetDb<DbEntity>().GetDbTransactionScope(level, identification, groupId);
        }
        public IDbTransactionScope GetDbTransactionScope(string entityFullName, System.Data.IsolationLevel level, string identification = "", string groupId = "")
        {
            var db = this.GetDb(entityFullName);
            return db.GetDbTransactionScope(level, identification, groupId);
        }
        public IDbTransactionScope GetDbTransactionScopeByDbKey(string dbKey, System.Data.IsolationLevel level, string identification = "", string groupId = "")
        {
            var db = GetDbByDbKey(dbKey);
            return db.GetDbTransactionScope(level, identification, groupId);
        }
        public IDbTransactionScopeCollection GetDbTransactionScopeCollection()
        {
            return new DbTransactionScopeCollection();
        }
        public bool AddZeroDbMapping<DbEntity>(string dbKey, string tableName) where DbEntity : class
        {
            return AddZeroDbMapping(typeof(DbEntity).FullName, dbKey, tableName);
        }
        public bool AddZeroDbMapping(string entityFullName, string dbKey, string tableName)
        {
            return DbConfigReader.AddTableMapping(entityFullName, dbKey, tableName);
        }


        public List<Target> Select<DbEntity, Target>(ListQuery query) where DbEntity : class, new() where Target : class, new()
        {
            return GetDb<DbEntity>().Select<DbEntity, Target>(query);
        }
        public List<DbEntity> Select<DbEntity>(ListQuery query) where DbEntity : class, new()
        {
            return GetDb<DbEntity>().Select<DbEntity>(query);
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
            return GetDb<DbEntity>().Select<DbEntity, OutType>(where, orderby, top, paras);
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
            return GetDb<DbEntity>().Select<DbEntity>(where, orderby, top, fields, paras);
        }
        public DbEntity SelectByPrimaryKey<DbEntity>(object key) where DbEntity : class, new()
        {
            return GetDb<DbEntity>().SelectByPrimaryKey<DbEntity>(key);
        }

        public PageData<OutType> Page<DbEntity, OutType>(PageQuery query) where DbEntity : class, new() where OutType : class, new()
        {
            return GetDb<DbEntity>().Page<DbEntity, OutType>(query);
        }
        public PageData<DbEntity> Page<DbEntity>(PageQuery query) where DbEntity : class, new()
        {
            return GetDb<DbEntity>().Page<DbEntity>(query);
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
            return GetDb<DbEntity>().Page<DbEntity, OutType>(page, size, where, orderby, uniqueField, paras);
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
            return GetDb<DbEntity>().Page<DbEntity>(page, size, where, orderby, fields, uniqueField, paras);
        }
        
        public long Count<DbEntity>(string where, params object[] paras) where DbEntity : class, new()
        {
            return Count(typeof(DbEntity), where, paras);
        }
        public long Count(Type entityType, string where, params object[] paras)
        {
            return GetDb(entityType.FullName).Count(entityType, where, paras);
        }
        public long MaxIdentityPrimaryKeyValue<DbEntity>() where DbEntity : class, new()
        {
            return GetDb<DbEntity>().MaxIdentityPrimaryKeyValue<DbEntity>();
        }
        public long MaxIdentityPrimaryKeyValue<DbEntity>(string where, params object[] paras) where DbEntity : class, new()
        {
            return MaxIdentityPrimaryKeyValue(typeof(DbEntity), where, paras);
        }
        public long MaxIdentityPrimaryKeyValue(Type entityType)
        {
            return MaxIdentityPrimaryKeyValue(entityType, "");
        }
        public long MaxIdentityPrimaryKeyValue(Type entityType, string where, params object[] paras)
        {
            return GetDb(entityType.FullName).MaxIdentityPrimaryKeyValue(entityType, where, paras);
        }

        public int Insert<DbEntity>(DbEntity entity) where DbEntity : class, new()
        {
            return GetDb<DbEntity>().Insert<DbEntity>(entity);
        }
        public int Insert<DbEntity>(List<DbEntity> entities) where DbEntity : class, new()
        {
            return GetDb<DbEntity>().Insert<DbEntity>(entities);
        }
        public int InsertByNameValueCollection<DbEntity>(NameValueCollection source) where DbEntity : class, new()
        {
            return InsertByNameValueCollection(typeof(DbEntity), source);
        }
        public int InsertByNameValueCollection(Type entityType, NameValueCollection source)
        {
            return GetDb(entityType.FullName).InsertByNameValueCollection(entityType, source);
        }
        public int InsertByCustomEntity<DbEntity>(object source) where DbEntity : class, new()
        {
            return InsertByCustomEntity(typeof(DbEntity), source);
        }
        public int InsertByCustomEntity(Type entityType, object source)
        {
            return GetDb(entityType.FullName).InsertByCustomEntity(entityType, source);
        }
        public int InsertByDictionary<DbEntity>(Dictionary<string, object> source) where DbEntity : class, new()
        {
            return InsertByDictionary(typeof(DbEntity), source);
        }
        public int InsertByDictionary(Type entityType, Dictionary<string, object> source)
        {
            return GetDb(entityType.FullName).InsertByCustomEntity(entityType, source);
        }

        public int Update<DbEntity>(DbEntity entity) where DbEntity : class, new()
        {
            return Update<DbEntity>(entity, "");
        }
        public int Update<DbEntity>(DbEntity entity, string appendWhere, params object[] paras) where DbEntity : class, new()
        {
            return GetDb<DbEntity>().Update<DbEntity>(entity, appendWhere, paras);
        }
        public int Update<DbEntity>(List<DbEntity> entities) where DbEntity : class, new()
        {
            return Update<DbEntity>(entities, "");
        }
        public int Update<DbEntity>(List<DbEntity> entities, string appendWhere, params object[] paras) where DbEntity : class, new()
        {
            return GetDb<DbEntity>().Update<DbEntity>(entities, appendWhere, paras);
        }
        public int UpdateByNameValueCollection<DbEntity>(NameValueCollection source) where DbEntity : class, new()
        {
            return UpdateByCustomEntity<DbEntity>(source, "");
        }
        public int UpdateByNameValueCollection<DbEntity>(NameValueCollection source, string appendWhere, params object[] paras) where DbEntity : class, new()
        {
            return UpdateByNameValueCollection(typeof(DbEntity), source, appendWhere, paras);
        }
        public int UpdateByNameValueCollection(Type entityType, NameValueCollection source, string appendWhere, params object[] paras)
        {
            return GetDb(entityType.FullName).UpdateByNameValueCollection(entityType, source, appendWhere, paras);
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
            return GetDb(entityType.FullName).UpdateByCustomEntity(entityType, source, appendWhere, paras);
        }
        public int UpdateByDictionary<DbEntity>(Dictionary<string, object> dic) where DbEntity : class, new()
        {
            return UpdateByDictionary<DbEntity>(dic, "");
        }
        public int UpdateByDictionary<DbEntity>(Dictionary<string, object> dic, string appendWhere, params object[] paras) where DbEntity : class, new()
        {
            return UpdateByDictionary(typeof(DbEntity), dic, appendWhere, paras);
        }
        public int UpdateByDictionary(Type entityType, Dictionary<string, object> dic, string appendWhere, params object[] paras)
        {
            return GetDb(entityType.FullName).UpdateByDictionary(entityType, dic, appendWhere, paras);
        }


        public int Delete<DbEntity>(string where, params object[] paras) where DbEntity : class, new()
        {
            return GetDb<DbEntity>().Delete<DbEntity>(where, paras);
        }
        public int Delete<DbEntity>(DbEntity source) where DbEntity : class, new()
        {
            return GetDb<DbEntity>().Delete<DbEntity>(source);
        }
        public int Delete(Type entityType, string where, params object[] paras)
        {
            return GetDb(entityType.FullName).Delete(entityType, where, paras);
        }
        public int DeleteByPrimaryKey<DbEntity>(object key) where DbEntity : class, new()
        {
            return DeleteByPrimaryKey(typeof(DbEntity), key);
        }
        public int DeleteByPrimaryKey(Type entityType, object key)
        {
            return GetDb(entityType.FullName).DeleteByPrimaryKey(entityType, key);
        }
        public int DeleteByCustomEntity<DbEntity>(object source) where DbEntity : class, new()
        {
            return DeleteByCustomEntity(typeof(DbEntity), source);
        }
        public int DeleteByCustomEntity(Type entityType, object source)
        {
            return GetDb(entityType.FullName).DeleteByCustomEntity(entityType, source);
        }
        public int DeleteByDictionary<DbEntity>(Dictionary<string, object> source) where DbEntity : class, new()
        {
            return DeleteByDictionary(typeof(DbEntity), source);
        }
        public int DeleteByDictionary(Type entityType, Dictionary<string, object> source)
        {
            return GetDb(entityType.FullName).DeleteByDictionary(entityType, source);
        }
        public int DeleteByNameValueCollection<DbEntity>(NameValueCollection source) where DbEntity : class, new()
        {
            return DeleteByNameValueCollection(typeof(DbEntity), source);
        }
        public int DeleteByNameValueCollection(Type entityType, NameValueCollection source)
        {
            return GetDb(entityType.FullName).DeleteByNameValueCollection(entityType, source);
        }


    }
}
