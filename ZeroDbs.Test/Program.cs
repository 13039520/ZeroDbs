using System;

namespace ZeroDbs.Test
{
    /// <summary>
    /// 
    /// </summary>
    class Program
    {
        static ZeroDbs.IDbService dbService = null;
        public class Device
        {
            public string Key { get; set; }
            public string ParentKey { get; set; }
            public string Name { get; set; }
            public string Type { get; set; }
            public string RuntimeState { get; set; }
        }
        static void Main(string[] args)
        {
            dbService = ZeroDbs.Factory.GetDbService();

            dbService.AddDbConfig("Sqlite002", "Sqlite", "Data Source=D:\\Program Files\\SQLiteStudio\\Database\\db002.db;version=3;datetimeformat=CurrentCulture");
            dbService.AddTableMapping<Device>("Sqlite002", "Device");

            //CodeGenerator();
            //InsertTest();
            //UpdateTest();
            //QueryTest();
        }


        static void CodeGenerator()
        {
            Console.WriteLine("start generating");

            ZeroDbs.Tools.CodeGenerator generator = new Tools.CodeGenerator();
            generator.Dbs.Add(new ZeroDbs.Common.DbInfo
            {
                ConnectionString = "Data Source=.;Initial Catalog=db001;User ID=sa;Password=123456;TrustServerCertificate=True;",
                Key = "SqlServer001",
                Type = "SqlServer"
            });
            generator.Dbs.Add(new ZeroDbs.Common.DbInfo
            {
                ConnectionString = "Host=192.168.10.2;Port=3306;Database=db001;User ID=wbh;Password=123456;",
                Key = "MySql001",
                Type = "MySql"
            });
            generator.Dbs.Add(new ZeroDbs.Common.DbInfo
            {
                ConnectionString = "Data Source=D:\\Program Files\\SQLiteStudio\\database\\db001.db3;version=3;datetimeformat=CurrentCulture",
                Key = "Sqlite001",
                Type = "Sqlite"
            });
            generator.Dbs.Add(new ZeroDbs.Common.DbInfo
            {
                ConnectionString = "Host=192.168.10.2;Port=5432;Database=db001;Username=postgres;Password=123456;",
                Key = "Pg001",
                Type = "PostgreSql"
            });
            generator.GeneratorConfig = new ZeroDbs.Tools.CodeGenerator.Config
            {
                AppProjectDir = @"D:\Work\ZeroDbs\ZeroDbs.Test",
                AppProjectNamespace = "ZeroDbs.Test",
                AppProjectName = "ZeroDbs.Test",
                EntityDir = @"D:\Work\ZeroDbs\ZeroDbs.Test\MyDbs",
                EntityNamespace = "MyDbs",
                EntityProjectName = "ZeroDbs.Test",
                GenerateRemark = "Version 1.0.0\r\nDate : " + DateTime.Now.ToString("yyyy-MM-dd"),
                IsPartialClass = false
            };
            generator.OnSingleTableGenerated += (e) => {
                Console.WriteLine("[{0}/{1}]\t{2}\t{3}", e.tableNum, e.tableCount, e.table.Name, e.entityClassFullName);
                foreach(var col in e.table.Colunms)
                {
                    Console.WriteLine("{0} {1} {2}",col.Type, col.Name,col.IsNullable);
                }
            };
            generator.Run();

            Console.WriteLine("Generated successfully.");
        }
        static void InsertTest()
        {
            //Single Insert
            dbService.Insert(new MyDbs.SqlServer001.User { Name = "u001", Email = "user@domain.com", Password = "123456" });
            dbService.Insert(new MyDbs.MySql001.User { Account = "u001", Email = "user@domain.com", Password = "123456" });
            dbService.Insert(new MyDbs.Sqlite001.User { Name = "u001", Email = "user@domain.com", Password = "123456" });
            dbService.Insert(new MyDbs.Pg001.User { Name = "u001", Password = "123456" });

            //Batch Insert
            var entities1 = new System.Collections.Generic.List<MyDbs.MySql001.User>();
            dbService.Insert(entities1);
        }
        static void UpdateTest()
        {
            dbService.Update(new MyDbs.SqlServer001.User { ID = 1, Name = "u002", Email = "u002@domain.com", Password = "123456789" });
            dbService.Update(new MyDbs.MySql001.User { ID = 1, Account = "u002", Email = "u002@domain.com", Password = "123456789" });
            dbService.Update(new MyDbs.Sqlite001.User { ID = 1, Name = "u002", Email = "u002@domain.com", Password = "123456789" });
            
            var nvc = new System.Collections.Specialized.NameValueCollection();
            nvc.Add("ID", "1");
            nvc.Add("Password", "aaaaaa");
            nvc.Add("Email", "aaaaa@bbbb.es");
            dbService.UpdateByNameValueCollection<MyDbs.Sqlite001.User>(nvc);

            var dic = new System.Collections.Generic.Dictionary<string, object>();
            dic.Add("ID", 1);
            dic.Add("Password", "aaaaaa");
            dic.Add("Email", "aaaaa@bbbb.es");
            dbService.UpdateByDictionary<MyDbs.Sqlite001.User>(dic);

            DateTime myTime = DateTime.Now;
            var customEntity = new { ID = 1, Name = "abcdefgh2", createTime = myTime };
            dbService.UpdateByCustomEntity<MyDbs.Sqlite001.User>(customEntity);//WHERE ID=@ID

        }
        static void QueryTest()
        {
            #region -- page --
            var query = dbService.PageQuery().UsePage(2).UseSize(10).UseWhere("ID>@0").UseParas(2).UseOrderby("ID DESC");
            //var pageData = dbService.Page<MyDbs.SqlServer001.User, MyEntity>(query);
            var pageData = dbService.Page<MyDbs.SqlServer001.User>(query);
            Console.WriteLine("MyDbs.SqlServer001.User(page):");
            if (pageData.Total > 0)
            {
                Console.WriteLine("There are a total of {0} rows of data, and {1} rows are returned this time.",
                    pageData.Total,
                    pageData.Items.Count);
            }
            else
            {
                Console.WriteLine("No data");
            }

            //var pageData2 = dbService.Page<MyDbs.SqlServer001.User, MyEntity>(2, 10, "ID>2", "ID DESC");
            //var pageData2 = dbService.Page<MyDbs.SqlServer001.User>(2, 10, "ID>2", "ID DESC");
            #endregion

            #region -- list --

            var listQuery = dbService.ListQuery().UseOrderby("ID DESC").UseTop(5).UseWhere("ID>@0").UseParas(1001);
            //var list1 = dbService.Select<MyDbs.SqlServer001.User, MyEntity>(listQuery);
            var list1 = dbService.Select<MyDbs.SqlServer001.User>(listQuery);
            
            Console.WriteLine("MyDbs.SqlServer001.User:");
            if (list1.Count > 0)
            {
                Console.WriteLine("It returned {0} rows of data.", list1.Count);
            }
            else
            {
                Console.WriteLine("No data");
            }

            //var list2 = dbService.Select<MyDbs.SqlServer001.User, MyEntity>("ID>1001", "ID DESC", 5);
            //var list2 = dbService.Select<MyDbs.SqlServer001.User>("ID>1001", "ID DESC", 5);

            #endregion

        }

        class MyEntity
        {
            public object ID { get; set; }
            public string Name { get; set; }
            public DateTime CreateTime { get; set; }
        }

    }
}
