using System;
using System.Collections.Generic;
using System.Text;

namespace ZeroDbs.Common
{
    internal static class DbFactory
    {
        private static List<DbCreater> dbCreaters = new List<DbCreater>();
        private static object dbCreatersLock = new object();
        private static bool initializationFlag = false;
        private static void Initialization()
        {
            if (initializationFlag) { return; }
            TryAddDbCreater("SqlServer", (dbInfo) => { return new SqlServer.Db(dbInfo); });
            TryAddDbCreater("MySql", (dbInfo) => { return new MySql.Db(dbInfo); });
            TryAddDbCreater("Sqlite", (dbInfo) => { return new Sqlite.Db(dbInfo); });
            TryAddDbCreater("PostgreSql", (dbInfo) => { return new PostgreSql.Db(dbInfo); });
            initializationFlag = true;
        }

        public static bool TryAddDbCreater(string dbType, DbCreateHandler creator)
        {
            lock (dbCreatersLock)
            {
                if (null == dbCreaters.Find(o => o.DbType.Equals(dbType, StringComparison.OrdinalIgnoreCase)))
                {
                    dbCreaters.Add(new DbCreater { DbType = dbType, Create = creator });
                    return true;
                }
                return false;
            }
        }
        public static IDb Create(IDbInfo dbInfo)
        {
            if (!initializationFlag)
            {
                Initialization();
            }
            var creater=dbCreaters.Find(o=>o.DbType.Equals(dbInfo.Type, StringComparison.OrdinalIgnoreCase));
            if (creater != null)
            {
                return creater.Create(dbInfo);
            }
            return null;
        }
        public static IDb Create(IDbInfo dbConfig, DbExecuteHandler dbExecuteSqlEvent)
        {
            IDb db = Create(dbConfig);
            if(db!=null&& dbExecuteSqlEvent != null)
            {
                db.OnDbExecuteSqlEvent += dbExecuteSqlEvent;
            }
            return db;
        }

    }
}
