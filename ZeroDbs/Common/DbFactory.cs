using System;
using System.Collections.Generic;
using System.Text;

namespace ZeroDbs.Common
{
    public static class DbFactory
    {
        public static IDb Create(Common.DbInfo dbConfig)
        {
            IDb db = null;
            switch (dbConfig.UseType)
            {
                case DbType.SqlServer:
                    db = new SqlServer.Db(dbConfig);
                    break;
                case DbType.MySql:
                    db = new MySql.Db(dbConfig);
                    break;
                case DbType.Sqlite:
                    db = new Sqlite.Db(dbConfig);
                    break;
            }
            return db;
        }
        public static IDb Create(Common.DbInfo dbConfig, Common.DbExecuteSqlEvent dbExecuteSqlEvent)
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
