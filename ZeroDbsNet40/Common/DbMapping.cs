using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace ZeroDbs.Common
{
    static class DbMapping
    {
        public static DbConfigDatabaseInfo GetZeroDbConfigDatabaseInfo(string dbKey)
        {
            var zeroConfigInfo = DbConfigReader.GetZeroDbConfigInfo();
            if (zeroConfigInfo != null && zeroConfigInfo.Dbs != null && zeroConfigInfo.Dbs.Count > 0)
            {
                return zeroConfigInfo.Dbs.Find(o => string.Equals(o.dbKey, dbKey, StringComparison.OrdinalIgnoreCase));
            }
            return null;
        }
        public static List<DbConfigDatabaseInfo> GetZeroDbConfigDatabaseInfo<T>()
        {
            return GetZeroDbConfigDatabaseInfoByEntityFullName(typeof(T).FullName);
        }
        public static List<DbConfigDatabaseInfo> GetZeroDbConfigDatabaseInfoByEntityFullName(string entityFullName)
        {
            if (string.IsNullOrEmpty(entityFullName)) { return null; }
            var zeroConfigInfo = DbConfigReader.GetZeroDbConfigInfo();
            if (zeroConfigInfo != null && zeroConfigInfo.Dbs != null && zeroConfigInfo.Dvs.Count > 0)
            {
                var entityKey = entityFullName;
                var info1 = zeroConfigInfo.Dvs.FindAll(o => string.Equals(o.entityKey, entityKey, StringComparison.OrdinalIgnoreCase));
                if (info1 == null || info1.Count < 1)
                {
                    return null;
                }
                var dbKeys = info1.Select(o => o.dbKey).Distinct().ToArray();
                var reval = new List<DbConfigDatabaseInfo>();
                foreach (var dbKey in dbKeys)
                {
                    var db = zeroConfigInfo.Dbs.Find(o => string.Equals(o.dbKey, dbKey, StringComparison.OrdinalIgnoreCase));
                    if (db != null)
                    {
                        reval.Add(db);
                    }
                }
                return reval;
            }
            return null;
        }
        public static List<DbConfigDataviewInfo> GetDbConfigDataViewInfo<T>()
        {
            var zeroConfigInfo = DbConfigReader.GetZeroDbConfigInfo();
            if (zeroConfigInfo != null && zeroConfigInfo.Dbs != null && zeroConfigInfo.Dvs.Count > 0)
            {
                var entityKey = typeof(T).FullName;
                var info1 = zeroConfigInfo.Dvs.FindAll(o => string.Equals(o.entityKey, entityKey, StringComparison.OrdinalIgnoreCase));
                if (info1 == null)
                {
                    return null;
                }
                return info1;
            }
            return null;
        }


    }
}
