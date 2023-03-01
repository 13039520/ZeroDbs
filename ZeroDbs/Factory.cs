using System;
using System.Collections.Generic;
using System.Text;
using ZeroDbs.Common;

namespace ZeroDbs
{
    public static class Factory
    {
        public static bool TryAddDbCreater(string dbType, Common.DbCreateHandler create)
        {
            return Common.DbFactory.TryAddDbCreater(dbType, create);
        }
        private static IDbService instance = null;
        private static object locker = new object();
        public static IDbService GetDbService()
        {
            return GetDbService(null);
        }
        public static IDbService GetDbService(DbExecuteHandler handler)
        {
            if (instance != null) { return instance; }
            lock (locker)
            {
                if (instance != null) { return instance; }
                instance = new DbService(handler);
            }
            return instance;
        }
    }
}
