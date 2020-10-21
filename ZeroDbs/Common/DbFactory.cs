using System;
using System.Collections.Generic;
using System.Text;

namespace ZeroDbs.Common
{
    public static class DbFactory
    {
        public static IDb Create(Common.DbConfigDatabaseInfo dbConfig)
        {
            IDb db = null;
            switch (dbConfig.dbType)
            {
                case "SqlServer":
                    db = new SqlServer.Db(dbConfig);
                    break;
                case "MySql":
                    db = new MySql.Db(dbConfig);
                    break;
                case "Sqlite":
                    db = new Sqlite.Db(dbConfig);
                    break;
            }
            return db;
        }
        public static IDb Create(Common.DbConfigDatabaseInfo dbConfig, Common.DbExecuteSqlEvent dbExecuteSqlEvent)
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
