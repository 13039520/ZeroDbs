using System;
using System.Collections.Generic;
using System.IO;

namespace ZeroDbs.Test
{
    /// <summary>
    /// 
    /// </summary>
    class Program
    {
        static ZeroDbs.IDbService dbService = null;

        static void Main(string[] args)
        {
            //Trying to create a SQLite database directory
            string dir = Path.GetDirectoryName(Path.GetFullPath("./Data/Dbs/events.db"));
            Directory.CreateDirectory(dir);


            var snowflake = Factory.CreateSnowflakeIdGenerator(1, 1);
            var dbs = new System.Collections.Generic.List<IDbConfig>
            {
                Factory.CreateDbConfig("Db001","SQLite","data source=./Data/Dbs/events.db"),
                Factory.CreateDbConfig("Db002","MySql","Server=127.0.0.1;Port=3306;Database=testdb;User Id=root;Password=123456;")
            };
            dbService = Factory.DbServiceInit(snowflake, dbs);
            dbService.AddNewDb(Factory.CreateDbConfig("Db003", "SqlServer", "Server=127.0.0.1,1433;Database=TestDb;User Id=sa;Password=123456;"));
            dbService.AddNewDb(Factory.CreateDbConfig("Db004", "PostgreSQL", "Host=127.0.0.1;Port=5432;Database=testdb;Username=postgres;Password=123456;"));


            //GetDb
            var db = dbService.GetDb("db001");//SQLite
            //Trying to create table
            db.ExecuteNonQuery(db.RawSqlOptions(
                db.SqlOptions("CREATE TABLE IF NOT EXISTS @n0(@n1 BIGINT PRIMARY KEY NOT NULL,@n2 VARCHAR(255) NOT NULL,@n3 DATETIME NOT NULL);")
                .SetNames("Table001","ID","Name","CreateTime")));

            Console.WriteLine("----------Insert----------");
            var nRows = new List<MyEntity>
            {
                new MyEntity { ID = db.Snowflake.NextId(), Name = "ABC", CreateTime = DateTime.Now },
                new MyEntity { ID = db.Snowflake.NextId(), Name = "DEF", CreateTime = DateTime.Now },
                new MyEntity { ID = db.Snowflake.NextId(), Name = "GHI", CreateTime = DateTime.Now }
            };
            var n = db.Insert<MyEntity>("Table001", nRows, (m, kvs) =>
            {
                kvs["ID"] = m.ID;
                kvs["Name"] = m.Name;
                kvs["CreateTime"] = m.CreateTime;
            });
            Console.WriteLine("Inserted {0} rows", n);

            var where = db.WhereOptions(
                        db.WherePartOptions("@n0>@p0").SetFields("ID").SetParams(5l),
                        db.WherePartOptions("@n0>=@p0", true).SetFields("CreateTime").SetParams(DateTime.Now.Date.AddDays(-7))
                    );//.And(...)

            Console.WriteLine("----------Select----------");
            var rs = db.Select<MyEntity>(
                db.SelectOptions<MyEntity>("Table001")
                .SetTop(5)
                .SetWhere(where)
                .SetOrderby(db.OrderbyOptions("ID", false))
                .SetConverter((r) =>
                {
                    return new MyEntity { ID = r.GetValue<long>("ID"), Name = r.GetValue<string>("Name"), CreateTime = r.GetValue<DateTime>("CreateTime") };
                }));

            foreach (var r in rs)
            {
                Console.WriteLine("{0}\t{1}\t{2}", r.ID, r.Name, r.CreateTime);
            }

            Console.WriteLine("----------SelectEach----------");
            db.SelectEach<MyEntity>(
                db.SelectOptions<MyEntity>("Table001")
                .SetTop(10)
                .SetWhere(where)
                .SetOrderby(db.OrderbyOptions("ID", false))
                .SetConverter((r) =>
                {
                    return new MyEntity { ID = r.GetValue<long>("ID"), Name = r.GetValue<string>("Name"), CreateTime = r.GetValue<DateTime>("CreateTime") };
                }), 
                //callback
                (m) => {
                    Console.WriteLine("{0}\t{1}\t{2}", m.ID, m.Name, m.CreateTime);
                });

            Console.WriteLine("----------Page----------");
            var pageData = db.Page<MyEntity>(
                db.PageOptions<MyEntity>("Table001")
                .SetWhere(where)
                .SetOrderby(db.OrderbyOptions("ID", false))
                .SetPage(1)
                .SetSize(10)
                .SetUniqueField("ID")
                .SetConverter((r) =>
                {
                    return new MyEntity { ID = r.GetValue<long>("ID"), Name = r.GetValue<string>("Name"), CreateTime = r.GetValue<DateTime>("CreateTime") };
                }));
            Console.WriteLine("Total {0} rows", pageData.Total);
            foreach (var r in pageData.Rows)
            {
                Console.WriteLine("{0}\t{1}\t{2}", r.ID, r.Name, r.CreateTime);
            }
        }
        class MyEntity
        {
            public long ID { get; set; }
            public string Name { get; set; }
            public DateTime CreateTime { get; set; }
        }

    }
}
