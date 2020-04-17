using System;
using System.Collections.Generic;
using System.Text;

namespace ZeroDbs.DataAccess
{
    public class DbSearcher : ZeroDbs.Interfaces.IDbSearcher
    {
        static Dictionary<string, Interfaces.IDb> _dbInstanceDic = new Dictionary<string, Interfaces.IDb>();
        static object _lock = new object();
        Interfaces.Common.DbExecuteSqlEvent _dbExecuteSqlEvent = null;

        public DbSearcher(Interfaces.Common.DbExecuteSqlEvent dbExecuteSqlEvent)
        {
            _dbExecuteSqlEvent = dbExecuteSqlEvent;
        }
        public Dictionary<string, ZeroDbs.Interfaces.IDb> GetDbs()
        {
            var dbs = new Dictionary<string, ZeroDbs.Interfaces.IDb>();
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
                                ZeroDbs.Interfaces.IDb db = ZeroDbs.DataAccess.Common.DbFactory.Create(m, _dbExecuteSqlEvent);
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
        public Dictionary<string, ZeroDbs.Interfaces.IDb> GetDbs(List<ZeroDbs.Interfaces.Common.DbConfigDatabaseInfo> dbConfigList)
        {
            var dbs = new Dictionary<string, ZeroDbs.Interfaces.IDb>();
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
                                ZeroDbs.Interfaces.IDb db = ZeroDbs.DataAccess.Common.DbFactory.Create(m, _dbExecuteSqlEvent);
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
        public ZeroDbs.Interfaces.IDb GetDb(string dbKey)
        {
            ZeroDbs.Interfaces.IDb reval = null;
            var info = Common.DbMapping.GetZeroDbConfigDatabaseInfo(dbKey);
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
                        ZeroDbs.Interfaces.IDb db = ZeroDbs.DataAccess.Common.DbFactory.Create(info, _dbExecuteSqlEvent);
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
                reval= _dbInstanceDic[info.dbKey];
            }
            return reval;
        }
        public ZeroDbs.Interfaces.IDb GetDb<T>() where T : class, new()
        {
            return GetDbByEntityFullName(typeof(T).FullName);
        }
        public ZeroDbs.Interfaces.IDb GetDbByEntityFullName(string entityFullName)
        {
            ZeroDbs.Interfaces.IDb reval = null;
            if (string.IsNullOrEmpty(entityFullName))
            {
                return reval;
            }
            var list = Common.DbMapping.GetZeroDbConfigDatabaseInfoByEntityFullName(entityFullName);
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
                        ZeroDbs.Interfaces.IDb db = ZeroDbs.DataAccess.Common.DbFactory.Create(info, _dbExecuteSqlEvent);
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

    }
}
