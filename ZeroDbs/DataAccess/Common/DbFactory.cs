using System;
using System.Collections.Generic;
using System.Text;

namespace ZeroDbs.DataAccess.Common
{
    public static class DbFactory
    {
        public static ZeroDbs.Interfaces.IDb Create(ZeroDbs.Interfaces.Common.DbConfigDatabaseInfo dbConfig)
        {
            ZeroDbs.Interfaces.IDb db = null;
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
        public static ZeroDbs.Interfaces.IDb Create(ZeroDbs.Interfaces.Common.DbConfigDatabaseInfo dbConfig, ZeroDbs.Interfaces.Common.DbExecuteSqlEvent dbExecuteSqlEvent)
        {
            ZeroDbs.Interfaces.IDb db = Create(dbConfig);
            if(db!=null&& dbExecuteSqlEvent != null)
            {
                db.OnDbExecuteSqlEvent += dbExecuteSqlEvent;
            }
            return db;
        }
    }
}
