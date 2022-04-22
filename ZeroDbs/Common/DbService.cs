﻿using System;
using System.Collections.Generic;
using System.Text;

namespace ZeroDbs.Common
{
    public class DbService : IDbService
    {
        IStrCommon _StrCommon = null;
        DbExecuteSqlEvent _dbExecuteSqlEvent = null;
        static Dictionary<string, IDb> _dbInstanceDic = new Dictionary<string, IDb>();
        static object _lock = new object();

        public IStrCommon StrCommon { get { return _StrCommon; } }


        public DbService()
        {
            _StrCommon = new Common.StrCommon();
        }
        public DbService(DbExecuteSqlEvent dbExecuteSqlEvent)
        {
            _dbExecuteSqlEvent = dbExecuteSqlEvent != null ? dbExecuteSqlEvent : null;
            _StrCommon = new Common.StrCommon();
        }
        private IDb GetDbByEntityFullName(string entityFullName)
        {
            IDb reval = null;
            if (string.IsNullOrEmpty(entityFullName))
            {
                return reval;
            }
            var list = DbMapping.GetZeroDbConfigDatabaseInfoByEntityFullName(entityFullName);
            if (list == null || list.Count < 1)
            {
                return reval;
            }
            var info = list[0];
            if (!_dbInstanceDic.ContainsKey(info.dbKey))
            {
                lock (_lock)
                {
                    #region -- code --
                    if (!_dbInstanceDic.ContainsKey(info.dbKey))
                    {
                        IDb db = DbFactory.Create(info, _dbExecuteSqlEvent);
                        if (db != null)
                        {
                            _dbInstanceDic.Add(info.dbKey, db);
                            reval = db;
                        }
                    }
                    else
                    {
                        reval = _dbInstanceDic[info.dbKey];
                    }
                    #endregion
                }
            }
            else
            {
                reval = _dbInstanceDic[info.dbKey];
            }
            return reval;
        }

        public Dictionary<string, IDb> GetDbs()
        {
            var dbs = new Dictionary<string, IDb>();
            var zeroConfigInfo = Common.DbConfigReader.GetZeroDbConfigInfo();
            if (zeroConfigInfo != null && zeroConfigInfo.Dbs != null && zeroConfigInfo.Dbs.Count > 0)
            {
                foreach (var m in zeroConfigInfo.Dbs)
                {
                    if (!_dbInstanceDic.ContainsKey(m.dbKey))
                    {
                        lock (_lock)
                        {
                            #region -- code --
                            if (!_dbInstanceDic.ContainsKey(m.dbKey))
                            {
                                IDb db = DbFactory.Create(m, _dbExecuteSqlEvent);
                                if (db != null)
                                {
                                    _dbInstanceDic.Add(m.dbKey, db);
                                    dbs.Add(m.dbKey, db);
                                }
                            }
                            else
                            {
                                dbs.Add(m.dbKey, _dbInstanceDic[m.dbKey]);
                            }
                            #endregion
                        }
                    }
                    else
                    {
                        dbs.Add(m.dbKey, _dbInstanceDic[m.dbKey]);
                    }
                }
            }
            return dbs;
        }
        public Dictionary<string, IDb> GetDbs(List<DatabaseInfo> dbConfigList)
        {
            var dbs = new Dictionary<string, IDb>();
            if (dbConfigList != null && dbConfigList.Count > 0)
            {
                foreach (var m in dbConfigList)
                {
                    if (!_dbInstanceDic.ContainsKey(m.dbKey))
                    {
                        lock (_lock)
                        {
                            #region -- code --
                            if (!_dbInstanceDic.ContainsKey(m.dbKey))
                            {
                                IDb db = DbFactory.Create(m, _dbExecuteSqlEvent);
                                if (db != null)
                                {
                                    _dbInstanceDic.Add(m.dbKey, db);
                                    dbs.Add(m.dbKey, db);
                                }
                            }
                            else
                            {
                                dbs.Add(m.dbKey, _dbInstanceDic[m.dbKey]);
                            }
                            #endregion
                        }
                    }
                    else
                    {
                        dbs.Add(m.dbKey, _dbInstanceDic[m.dbKey]);
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
            var info = DbMapping.GetZeroDbConfigDatabaseInfo(dbKey);
            if (info == null)
            {
                return null;
            }
            if (!_dbInstanceDic.ContainsKey(info.dbKey))
            {
                lock (_lock)
                {
                    #region -- code --
                    if (!_dbInstanceDic.ContainsKey(info.dbKey))
                    {
                        IDb db = DbFactory.Create(info, _dbExecuteSqlEvent);
                        if (db != null)
                        {
                            _dbInstanceDic.Add(info.dbKey, db);
                            reval = db;
                        }
                    }
                    else
                    {
                        reval = _dbInstanceDic[info.dbKey];
                    }
                    #endregion
                }
            }
            else
            {
                reval = _dbInstanceDic[info.dbKey];
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
            return DbConfigReader.AddZeroDbMapping(entityFullName, dbKey, tableName);
        }


        public List<Target> Select<DbEntity, Target>(Common.ListQuery query) where DbEntity : class, new() where Target : class, new()
        {
            return GetDb<DbEntity>().Select<DbEntity, Target>(query);
        }
        public List<DbEntity> Select<DbEntity>(Common.ListQuery query) where DbEntity : class, new()
        {
            return GetDb<DbEntity>().Select<DbEntity>(query);
        }
        public List<IntoEntity> Select<DbEntity, IntoEntity>(string where, string orderby, int top, params object[] paras) where DbEntity : class, new() where IntoEntity : class, new()
        {
            return GetDb<DbEntity>().Select<DbEntity, IntoEntity>(where, orderby, top, paras);
        }
        public List<DbEntity> Select<DbEntity>(string where, string orderby, int top, string[] fields, params object[] paras) where DbEntity : class, new()
        {
            return GetDb<DbEntity>().Select<DbEntity>(where, orderby, top, fields, paras);
        }

        public Common.PageData<IntoEntity> Page<DbEntity, IntoEntity>(PageQuery query) where DbEntity : class, new() where IntoEntity : class, new()
        {
            return GetDb<DbEntity>().Page<DbEntity, IntoEntity>(query);
        }
        public Common.PageData<DbEntity> Page<DbEntity>(PageQuery query) where DbEntity : class, new()
        {
            return GetDb<DbEntity>().Page<DbEntity>(query);
        }
        public Common.PageData<IntoEntity> Page<DbEntity, IntoEntity>(long page, long size, string where, string orderby, string uniqueField, params object[] paras) where DbEntity : class, new() where IntoEntity : class, new()
        {
            return GetDb<DbEntity>().Page<DbEntity, IntoEntity>(page, size, where, orderby, uniqueField, paras);
        }
        public Common.PageData<DbEntity> Page<DbEntity>(long page, long size, string where, string orderby, string[] fields, string uniqueField, params object[] paras) where DbEntity : class, new()
        {
            return GetDb<DbEntity>().Page<DbEntity>(page, size, where, orderby, fields, uniqueField, paras);
        }
        public long Count<DbEntity>(string where, params object[] paras) where DbEntity : class, new()
        {
            return GetDb<DbEntity>().Count<DbEntity>(where, paras);
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
            return GetDb<DbEntity>().Update<DbEntity>(entity);
        }
        public int Update<DbEntity>(List<DbEntity> entities) where DbEntity : class, new()
        {
            return GetDb<DbEntity>().Update<DbEntity>(entities);
        }
        public int UpdateFromNameValueCollection<DbEntity>(System.Collections.Specialized.NameValueCollection nvc) where DbEntity : class, new()
        {
            return GetDb<DbEntity>().UpdateFromNameValueCollection<DbEntity>(nvc);
        }
        public int UpdateFromCustomEntity<DbEntity>(object source) where DbEntity : class, new()
        {
            return GetDb<DbEntity>().UpdateFromCustomEntity<DbEntity>(source);
        }
        public int UpdateFromDictionary<DbEntity>(Dictionary<string, object> dic) where DbEntity : class, new()
        {
            return GetDb<DbEntity>().UpdateFromDictionary<DbEntity>(dic);
        }

        public int Delete<DbEntity>(string where, params object[] paras) where DbEntity : class, new()
        {
            return GetDb<DbEntity>().Delete<DbEntity>(where, paras);
        }


    }
}
