using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace ZeroDbs.Common
{
    static class DbMapping
    {
        public static DbInfo GetDbInfo(string dbKey)
        {
            var zeroConfigInfo = DbConfigReader.GetDbConfigInfo();
            if (zeroConfigInfo != null && zeroConfigInfo.Dbs != null && zeroConfigInfo.Dbs.Count > 0)
            {
                return zeroConfigInfo.Dbs.Find(o => string.Equals(o.Key, dbKey, StringComparison.OrdinalIgnoreCase));
            }
            return null;
        }
        public static List<DbInfo> GetDbInfo<T>()
        {
            return GetDbInfoByEntityFullName(typeof(T).FullName);
        }
        public static List<DbInfo> GetDbInfoByEntityFullName(string entityFullName)
        {
            if (string.IsNullOrEmpty(entityFullName)) { return null; }
            var zeroConfigInfo = DbConfigReader.GetDbConfigInfo();
            if (zeroConfigInfo != null && zeroConfigInfo.Dbs != null && zeroConfigInfo.Dvs.Count > 0)
            {
                var entityKey = entityFullName;
                var info1 = zeroConfigInfo.Dvs.FindAll(o => string.Equals(o.EntityKey, entityKey, StringComparison.OrdinalIgnoreCase));
                if (info1 == null || info1.Count < 1)
                {
                    return null;
                }
                var dbKeys = info1.Select(o => o.DbKey).Distinct().ToArray();
                var reval = new List<DbInfo>();
                foreach (var dbKey in dbKeys)
                {
                    var db = zeroConfigInfo.Dbs.Find(o => string.Equals(o.Key, dbKey, StringComparison.OrdinalIgnoreCase));
                    if (db != null)
                    {
                        reval.Add(db);
                    }
                }
                return reval;
            }
            return null;
        }
        public static List<DbTableEntityMap> GetDbConfigDataViewInfo<T>()
        {
            var zeroConfigInfo = DbConfigReader.GetDbConfigInfo();
            if (zeroConfigInfo != null && zeroConfigInfo.Dbs != null && zeroConfigInfo.Dvs.Count > 0)
            {
                var entityKey = typeof(T).FullName;
                var info1 = zeroConfigInfo.Dvs.FindAll(o => string.Equals(o.EntityKey, entityKey, StringComparison.OrdinalIgnoreCase));
                return info1;
            }
            return null;
        }
        public static bool IsStandardMapping<T>()
        {
            var zeroConfigInfo = DbConfigReader.GetDbConfigInfo();
            if (zeroConfigInfo != null && zeroConfigInfo.Dbs != null && zeroConfigInfo.Dvs.Count > 0)
            {
                var entityKey = typeof(T).FullName;
                var info1 = zeroConfigInfo.Dvs.Find(o => string.Equals(o.EntityKey, entityKey, StringComparison.OrdinalIgnoreCase));
                if (info1 == null)
                {
                    return false;
                }
                return info1.IsStandardMapping;
            }
            return false;
        }


    }
}
