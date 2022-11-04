using System;
using System.Collections.Generic;
using System.Text;

namespace ZeroDbs.Common
{
    public static class DbFactory
    {
        public delegate Db DbCreateHandler(IDbInfo dbInfo);
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
            TryAddDbCreater("SqlServer", (dnInfo) => { return new SqlServer.Db(dnInfo); });
            TryAddDbCreater("MySql", (dnInfo) => { return new MySql.Db(dnInfo); });
            TryAddDbCreater("Sqlite", (dnInfo) => { return new Sqlite.Db(dnInfo); });
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
