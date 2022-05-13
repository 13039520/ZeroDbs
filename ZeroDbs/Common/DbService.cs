using System;
using System.Collections.Generic;
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
            _StrCommon = new Common.StrCommon();
        }
        public DbService(DbExecuteHandler handler)
        {
            _dbExecuteHandler = handler != null ? handler : null;
            _StrCommon = new Common.StrCommon();
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
            var zeroConfigInfo = Common.DbConfigReader.GetDbConfigInfo();
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

        public Common.PageData<OutType> Page<DbEntity, OutType>(PageQuery query) where DbEntity : class, new() where OutType : class, new()
        {
            return GetDb<DbEntity>().Page<DbEntity, OutType>(query);
        }
        public Common.PageData<DbEntity> Page<DbEntity>(PageQuery query) where DbEntity : class, new()
        {
            return GetDb<DbEntity>().Page<DbEntity>(query);
        }
        public Common.PageData<OutType> Page<DbEntity, OutType>(long page, long size) where DbEntity : class, new() where OutType : class, new()
        {
            return Page<DbEntity, OutType>(page, size, "");
        }
        public Common.PageData<OutType> Page<DbEntity, OutType>(long page, long size, string where) where DbEntity : class, new() where OutType : class, new()
        {
            return Page<DbEntity, OutType>(page, size, where, "");
        }
        public Common.PageData<OutType> Page<DbEntity, OutType>(long page, long size, string where, string orderby) where DbEntity : class, new() where OutType : class, new()
        {
            return Page<DbEntity, OutType>(page, size, where, orderby, "");
        }
        public Common.PageData<OutType> Page<DbEntity, OutType>(long page, long size, string where, string orderby, string uniqueField) where DbEntity : class, new() where OutType : class, new()
        {
            return Page<DbEntity, OutType>(page, size, where, orderby, uniqueField, new object[0]);
        }
        public Common.PageData<OutType> Page<DbEntity, OutType>(long page, long size, string where, string orderby, string uniqueField, params object[] paras) where DbEntity : class, new() where OutType : class, new()
        {
            return GetDb<DbEntity>().Page<DbEntity, OutType>(page, size, where, orderby, uniqueField, paras);
        }
        public Common.PageData<DbEntity> Page<DbEntity>(long page, long size) where DbEntity : class, new()
        {
            return Page<DbEntity>(page, size, "");
        }
        public Common.PageData<DbEntity> Page<DbEntity>(long page, long size, string where) where DbEntity : class, new()
        {
            return Page<DbEntity>(page, size, where, "");
        }
        public Common.PageData<DbEntity> Page<DbEntity>(long page, long size, string where, string orderby) where DbEntity : class, new()
        {
            return Page<DbEntity>(page, size, where, orderby, new string[0]);
        }
        public Common.PageData<DbEntity> Page<DbEntity>(long page, long size, string where, string orderby, string[] fields) where DbEntity : class, new()
        {
            return Page<DbEntity>(page, size, where, orderby, fields, "");
        }
        public Common.PageData<DbEntity> Page<DbEntity>(long page, long size, string where, string orderby, string[] fields, string uniqueField) where DbEntity : class, new()
        {
            return Page<DbEntity>(page, size, where, orderby, fields, uniqueField, new object[0]);
        }
        public Common.PageData<DbEntity> Page<DbEntity>(long page, long size, string where, string orderby, string[] fields, string uniqueField, params object[] paras) where DbEntity : class, new()
        {
            return GetDb<DbEntity>().Page<DbEntity>(page, size, where, orderby, fields, uniqueField, paras);
        }
        
        public long Count<DbEntity>(string where, params object[] paras) where DbEntity : class, new()
        {
            return GetDb<DbEntity>().Count<DbEntity>(where, paras);
        }
        public long MaxIdentityPrimaryKeyValue<DbEntity>() where DbEntity : class, new()
        {
            return GetDb<DbEntity>().MaxIdentityPrimaryKeyValue<DbEntity>();
        }
        public long MaxIdentityPrimaryKeyValue<DbEntity>(string where, params object[] paras) where DbEntity : class, new()
        {
            return GetDb<DbEntity>().MaxIdentityPrimaryKeyValue<DbEntity>(where, paras);
        }
        public int Insert<DbEntity>(DbEntity entity) where DbEntity : class, new()
        {
            return GetDb<DbEntity>().Insert<DbEntity>(entity);
        }
        public int Insert<DbEntity>(List<DbEntity> entities) where DbEntity : class, new()
        {
            return GetDb<DbEntity>().Insert<DbEntity>(entities);
        }
        public int InsertFromNameValueCollection<DbEntity>(System.Collections.Specialized.NameValueCollection nvc) where DbEntity : class, new()
        {
            return GetDb<DbEntity>().InsertFromNameValueCollection<DbEntity>(nvc);
        }
        public int InsertFromCustomEntity<DbEntity>(object source) where DbEntity : class, new()
        {
            return GetDb<DbEntity>().InsertFromCustomEntity<DbEntity>(source);
        }
        public int InsertFromDictionary<DbEntity>(Dictionary<string, object> dic) where DbEntity : class, new()
        {
            return GetDb<DbEntity>().InsertFromDictionary<DbEntity>(dic);
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
        public int UpdateFromNameValueCollection<DbEntity>(System.Collections.Specialized.NameValueCollection source) where DbEntity : class, new()
        {
            return UpdateFromCustomEntity<DbEntity>(source, "");
        }
        public int UpdateFromNameValueCollection<DbEntity>(System.Collections.Specialized.NameValueCollection source, string appendWhere, params object[] paras) where DbEntity : class, new()
        {
            return GetDb<DbEntity>().UpdateFromNameValueCollection<DbEntity>(source, appendWhere, paras);
        }
        public int UpdateFromCustomEntity<DbEntity>(object source) where DbEntity : class, new()
        {
            return UpdateFromCustomEntity<DbEntity>(source, "");
        }
        public int UpdateFromCustomEntity<DbEntity>(object source, string appendWhere, params object[] paras) where DbEntity : class, new()
        {
            return GetDb<DbEntity>().UpdateFromCustomEntity<DbEntity>(source, appendWhere, paras);
        }
        public int UpdateFromDictionary<DbEntity>(Dictionary<string, object> dic) where DbEntity : class, new()
        {
            return UpdateFromDictionary<DbEntity>(dic, "");
        }
        public int UpdateFromDictionary<DbEntity>(Dictionary<string, object> dic, string appendWhere, params object[] paras) where DbEntity : class, new()
        {
            return GetDb<DbEntity>().UpdateFromDictionary<DbEntity>(dic, appendWhere, paras);
        }

        public int Delete<DbEntity>(string where, params object[] paras) where DbEntity : class, new()
        {
            return GetDb<DbEntity>().Delete<DbEntity>(where, paras);
        }


    }
}
