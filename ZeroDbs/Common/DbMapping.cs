using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace ZeroDbs.Common
{
    static class DbMapping
    {
        private static DbConfigInfo GetDbConfigInfo()
        {
            var config = DbConfigReader.GetDbConfigInfo();
            if (config == null || config.Dbs == null || config.Dbs.Count < 1)
            {
                throw new Exception("DbConfigInfo error");
            }
            return config;
        }
        public static DbInfo GetDbInfo(string dbKey)
        {
            var db = GetDbConfigInfo().Dbs.Find(o => string.Equals(o.Key, dbKey, StringComparison.OrdinalIgnoreCase));
            if (db != null) { return db; }
            throw new Exception("the \"" + dbKey + "\" does not exists");
        }
        public static List<IDbInfo> GetDbInfo<T>()
        {
            return GetDbInfoByEntityFullName(typeof(T).FullName);
        }
        public static List<IDbInfo> GetDbInfoByEntityFullName(string entityFullName)
        {
            if (string.IsNullOrEmpty(entityFullName)) {
                throw new Exception("The entityFullName is noll or empty ");
            }
            var config = GetDbConfigInfo();
            var info1 = config.Dvs.Find(o => string.Equals(o.EntityKey, entityFullName, StringComparison.OrdinalIgnoreCase));
            if (info1 == null)
            {
                throw new Exception("\"" + entityFullName + "\" does not exists");
            }
            var reval = new List<IDbInfo>();
            var db = config.Dbs.Find(o => string.Equals(o.Key, info1.DbKey, StringComparison.OrdinalIgnoreCase));
            if (db != null)
            {
                reval.Add(db);
            }
            return reval;
        }
        public static List<DbTableEntityMap> DbTableEntityMap<T>()
        {
            string entityKey = typeof(T).FullName;
            return GetDbConfigInfo().Dvs.FindAll(o => string.Equals(o.EntityKey, entityKey, StringComparison.OrdinalIgnoreCase));
        }
        public static List<DbTableEntityMap> GetDbTableEntityMapByEntityFullName(string entityFullName)
        {
            return GetDbConfigInfo().Dvs.FindAll(o => string.Equals(o.EntityKey, entityFullName, StringComparison.OrdinalIgnoreCase));
        }
        public static List<DbTableEntityMap> GetDbTableEntityMap(string dbKey)
        {
            return GetDbConfigInfo().Dvs.FindAll(o => string.Equals(o.DbKey, dbKey, StringComparison.OrdinalIgnoreCase));
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
