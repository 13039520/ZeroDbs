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
        public Dictionary<string, IDb> GetDbs(List<DbConfigDatabaseInfo> dbConfigList)
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
        public IDb GetDb<T>() where T : class, new()
        {
            return GetDb(typeof(T).FullName);
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
        public IDbCommand GetDbCommand<T>() where T : class, new()
        {
            return GetDb<T>().GetDbCommand();
        }
        public IDbCommand GetDbCommand(string entityFullName)
        {
            return GetDb(entityFullName).GetDbCommand();
        }
        public IDbCommand GetDbCommandByDbKey(string dbKey)
        {
            return GetDbByDbKey(dbKey).GetDbCommand();
        }
        public IDbTransactionScope GetDbTransactionScope<T>(System.Data.IsolationLevel level, string identification = "", string groupId = "") where T : class, new()
        {
            var db = this.GetDb<T>();
            return db.GetDbTransactionScope(level, identification, groupId);
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
        public bool AddZeroDbMapping<T>(string dbKey, string tableName) where T : class
        {
            return AddZeroDbMapping(typeof(T).FullName, dbKey, tableName);
        }
        public bool AddZeroDbMapping(string entityFullName, string dbKey, string tableName)
        {
            return DbConfigReader.AddZeroDbMapping(entityFullName, dbKey, tableName);
        }

        public List<T> Select<T>(string where) where T : class, new()
        {
            return GetDb<T>().Select<T>(where);
        }
        public List<T> Select<T>(string where, string orderby) where T : class, new()
        {
            return GetDb<T>().Select<T>(where, orderby);
        }
        public List<T> Select<T>(string where, string orderby, int top) where T : class, new()
        {
            return GetDb<T>().Select<T>(where, orderby, top);
        }
        public List<T> Select<T>(string where, string orderby, int top, int threshold) where T : class, new()
        {
            return GetDb<T>().Select<T>(where, orderby, top, threshold);
        }
        public List<T> Select<T>(string where, string orderby, int top, string[] fieldNames) where T : class, new()
        {
            return GetDb<T>().Select<T>(where, orderby, top, fieldNames);
        }

        public Common.PageData<T> Page<T>(long page, long size, string where) where T : class, new()
        {
            return GetDb<T>().Page<T>(page, size, where);
        }
        public Common.PageData<T> Page<T>(long page, long size, string where, string orderby) where T : class, new()
        {
            return GetDb<T>().Page<T>(page, size, where, orderby);
        }
        public Common.PageData<T> Page<T>(long page, long size, string where, string orderby, int threshold) where T : class, new()
        {
            return GetDb<T>().Page<T>(page, size, where, orderby, threshold);
        }
        public Common.PageData<T> Page<T>(long page, long size, string where, string orderby, string[] fieldNames) where T : class, new()
        {
            return GetDb<T>().Page<T>(page, size, where, orderby, fieldNames);
        }
        public Common.PageData<T> Page<T>(long page, long size, string where, string orderby, int threshold, string uniqueFieldName) where T : class, new()
        {
            return GetDb<T>().Page<T>(page, size, where, orderby, threshold, uniqueFieldName);
        }
        public Common.PageData<T> Page<T>(long page, long size, string where, string orderby, string[] fieldNames, string uniqueFieldName) where T : class, new()
        {
            return GetDb<T>().Page<T>(page, size, where, orderby, fieldNames, uniqueFieldName);
        }

        public long Count<T>(string where) where T : class, new()
        {
            return GetDb<T>().Count<T>(where);
        }

        public int Insert<T>(T entity) where T : class, new()
        {
            return GetDb<T>().Insert<T>(entity);
        }
        public int Insert<T>(List<T> entityList) where T : class, new()
        {
            return GetDb<T>().Insert<T>(entityList);
        }
        public int Insert<T>(System.Collections.Specialized.NameValueCollection nvc) where T : class, new()
        {
            return GetDb<T>().Insert<T>(nvc);
        }
        public int Insert<T>(List<System.Collections.Specialized.NameValueCollection> nvcList) where T : class, new()
        {
            return GetDb<T>().Insert<T>(nvcList);
        }

        public int Update<T>(T entity) where T : class, new()
        {
            return GetDb<T>().Update<T>(entity);
        }
        public int Update<T>(List<T> entityList) where T : class, new()
        {
            return GetDb<T>().Update<T>(entityList);
        }
        public int Update<T>(System.Collections.Specialized.NameValueCollection nvc) where T : class, new()
        {
            return GetDb<T>().Update<T>(nvc);
        }
        public int Update<T>(List<System.Collections.Specialized.NameValueCollection> nvcList) where T : class, new()
        {
            return GetDb<T>().Update<T>(nvcList);
        }

        public int Delete<T>(T entity) where T : class, new()
        {
            return GetDb<T>().Delete<T>(entity);
        }
        public int Delete<T>(List<T> entityList) where T : class, new()
        {
            return GetDb<T>().Delete<T>(entityList);
        }
        public int Delete<T>(System.Collections.Specialized.NameValueCollection nvc) where T : class, new()
        {
            return GetDb<T>().Delete<T>(nvc);
        }
        public int Delete<T>(List<System.Collections.Specialized.NameValueCollection> nvcList) where T : class, new()
        {
            return GetDb<T>().Delete<T>(nvcList);
        }











    }
}
