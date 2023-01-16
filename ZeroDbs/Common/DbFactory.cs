using System;
using System.Collections.Generic;
using System.Text;

namespace ZeroDbs.Common
{
    public static class DbFactory
    {
        public delegate IDb DbCreateHandler(IDbInfo dbInfo);
        public class DbCreater
        {
            public string DbType { get; set; }
            public DbCreateHandler Create { get; set; }
        }
        private static List<DbCreater> dbCreaters = new List<DbCreater>();
        private static object dbCreatersLock = new object();
        private static bool initializationFlag = false;
        private static void Initialization()
        {
            if (initializationFlag) { return; }
            initializationFlag = true;
            TryAddDbCreater("SqlServer", (dbInfo) => { return new SqlServer.Db(dbInfo); });
            TryAddDbCreater("MySql", (dbInfo) => { return new MySql.Db(dbInfo); });
            TryAddDbCreater("Sqlite", (dbInfo) => { return new Sqlite.Db(dbInfo); });
        }

        public static bool TryAddDbCreater(string dbType, DbCreateHandler create)
        {
            lock (dbCreatersLock)
            {
                if (null == dbCreaters.Find(o => o.DbType.Equals(dbType, StringComparison.OrdinalIgnoreCase)))
                {
                    dbCreaters.Add(new DbCreater { DbType = dbType, Create = create });
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
        public static IDb Create(IDbInfo dbConfig, Common.DbExecuteHandler dbExecuteSqlEvent)
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
