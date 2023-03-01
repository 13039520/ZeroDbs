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
            /*dbService = new ZeroDbs.Common.DbService(new Common.DbExecuteHandler((obj, e) => {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("DbKey={0}&Trans={1}&Sql=\r\n{2}\r\n&Message={3}", e.DbKey, e.TransactionInfo, e.ExecuteSql, e.Message);
                Console.ResetColor();
            }));*/
            dbService = ZeroDbs.Factory.GetDbService(new Common.DbExecuteHandler((obj, e) => {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("DbKey={0}&Trans={1}&Sql=\r\n{2}\r\n&Message={3}", e.DbKey, e.TransactionInfo, e.ExecuteSql, e.Message);
                Console.ResetColor();
            }));

            dbService.AddDbConfig("Sqlite002", "Sqlite", "Data Source=D:\\Program Files\\SQLiteStudio\\Database\\DeviceInfo.db;version=3;datetimeformat=CurrentCulture");
            dbService.AddTableMapping<Device>("Sqlite002", "Device");

            //CodeGenerator();
            //InsertTest();
            //UpdateTest();
            QueryTest();
        }


        static void CodeGenerator()
        {
            Console.WriteLine("start generating");

            ZeroDbs.Tools.CodeGenerator generator = new Tools.CodeGenerator();
            generator.Dbs.Add(new ZeroDbs.Common.DbInfo
            {
                ConnectionString = "Data Source=.;Initial Catalog=ZeroTestDb;User ID=sa;Password=123456;TrustServerCertificate=True;",
                Key = "SqlServer001",
                Type = "SqlServer"
            });
            generator.Dbs.Add(new ZeroDbs.Common.DbInfo
            {
                ConnectionString = "Host=localhost;Port=3306;Database=zerotestdb;User ID=root;Password=123456;",
                Key = "MySql001",
                Type = "MySql"
            });
            generator.Dbs.Add(new ZeroDbs.Common.DbInfo
            {
                ConnectionString = "Data Source=D:\\Program Files\\SQLiteStudio\\database\\ZeroTestDb.db3;version=3;datetimeformat=CurrentCulture",
                Key = "Sqlite001",
                Type = "Sqlite"
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
        static void InsertTest(int beginNum = 1000)
        {
            /*string num = beginNum.ToString().PadLeft(3, '0');
            dbService.Insert(new MyDbs.SqlServer001.tUser { Name = "user" + num + "_sqlserver", Email = "user" + num + "@domain.com", Password = "123456" });
            dbService.Insert(new MyDbs.MySql001.tUser { Name = "user" + num + "_mysql", Email = "user" + num + "@domain.com", Password = "123456" });
            dbService.Insert(new MyDbs.Sqlite001.tUser { Name = "user" + num + "_sqlite", Email = "user" + num + "@domain.com", Password = "123456" });
            beginNum++;*/

            var entities1 = new System.Collections.Generic.List<MyDbs.SqlServer001.tUser>();
            var entities2 = new System.Collections.Generic.List<MyDbs.MySql001.tUser>();
            var entities3 = new System.Collections.Generic.List<MyDbs.Sqlite001.tUser>();
            int endNum = beginNum + 10000;
            for (int i = beginNum; i < endNum; i++)
            {
                string num = i.ToString().PadLeft(3, '0');
                entities1.Add(new MyDbs.SqlServer001.tUser { Name = "user" + num + "_sqlserver", Email = "user" + num + "@domain.com", Password = "123456" });
                //entities2.Add(new MyDbs.MySql001.tUser { Name = "user" + num + "_mysql", Email = "user" + num + "@domain.com", Password = "123456" });
                //entities3.Add(new MyDbs.Sqlite001.tUser { Name = "user" + num + "_sqlite", Email = "user" + num + "@domain.com", Password = "123456" });
            }
            System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();
            dbService.Insert(entities1, 100);
            stopwatch.Stop();
            Console.WriteLine("ElapsedMilliseconds=>{0}", stopwatch.ElapsedMilliseconds);
            //dbService.Insert(entities2, 30);
            //dbService.Insert(entities3, 30);
        }
        static void UpdateTest()
        {
            /*
            dbService.Update(new MyDbs.SqlServer001.tUser { ID = 1, Name = "user009_sqlserver", Email = "user009@domain.com", Password = "123456789" });
            dbService.Update(new MyDbs.MySql001.tUser { ID = 1, Name = "user009_mysql", Email = "user009@domain.com", Password = "123456789" });
            dbService.Update(new MyDbs.Sqlite001.tUser { ID = 1, Name = "user009_sqlite", Email = "user009@domain.com", Password = "123456789" });
            */

            /**/
            var nvc = new System.Collections.Specialized.NameValueCollection();
            nvc.Add("ID", "1");
            nvc.Add("Password", "aaaaaa");
            nvc.Add("Email", "aaaaa@bbbb.es");
            dbService.UpdateByNameValueCollection<MyDbs.SqlServer001.tUser>(nvc);
            dbService.UpdateByNameValueCollection<MyDbs.MySql001.tUser>(nvc);
            dbService.UpdateByNameValueCollection<MyDbs.Sqlite001.tUser>(nvc);
            

            /**/
            var dic = new System.Collections.Generic.Dictionary<string, object>();
            dic.Add("ID", 1);
            dic.Add("Password", "aaaaaa");
            dic.Add("Email", "aaaaa@bbbb.es");
            dbService.UpdateByDictionary<MyDbs.SqlServer001.tUser>(dic);
            dbService.UpdateByDictionary<MyDbs.MySql001.tUser>(dic);
            dbService.UpdateByDictionary<MyDbs.Sqlite001.tUser>(dic);
            

            DateTime myTime = DateTime.Now;
            var customEntity = new { ID = 1, Name = "abcdefgh2", createTime = myTime };
            int n = dbService.UpdateByCustomEntity<MyDbs.SqlServer001.tUser>(customEntity, "Email=@0", "user010@domain.com");//WHERE ID=@ID AND Email=@0
            Console.WriteLine("{0} rows affected", n);
            n = dbService.UpdateByCustomEntity<MyDbs.MySql001.tUser>(customEntity);//WHERE ID=@ID
            Console.WriteLine("{0} rows affected", n);
            n = dbService.UpdateByCustomEntity<MyDbs.Sqlite001.tUser>(customEntity);//WHERE ID=@ID
            Console.WriteLine("{0} rows affected", n);

        }
        static void QueryTest()
        {
            #region -- page --
            var devices = dbService.Select<Device>();
            foreach(var d in devices)
            {
                Console.WriteLine("{0}\t{1}\t{2}\t{3}\t{4}", d.Key, d.ParentKey, d.Name, d.Type, d.RuntimeState);
            }
            /*var query = new ZeroDbs.Common.PageQuery
            {
                Page = 1,
                Size = 5,
                Orderby = "ID DESC",
                Where = "ID>@0",
                Paras = new object[] { 2 },//@0=2
                Fields = new string[0],
                UniqueField = ""
            };*/
            var query = new ZeroDbs.Common.PageQuery().UsePage(2).UseSize(10).UseWhere("ID>@0").UseParas(2).UseOrderby("ID DESC");
            var page1 = dbService.Page<MyDbs.SqlServer001.tUser,MyEntity>(query);
            Console.WriteLine("MyDbs.SqlServer001.tUser(page1):");
            if (page1.Total > 0)
            {
                foreach (MyEntity m in page1.Items)
                {
                    Console.WriteLine("{0}\t{1}\t{2}\t{3}", m.ID, m.Name, "", m.CreateTime);
                }
            }
            else
            {
                Console.WriteLine("no data");
            }
            var page2 = dbService.Page<MyDbs.MySql001.tUser>(query);
            Console.WriteLine("MyDbs.MySql001.tUser(page2):");
            if (page2.Total > 0)
            {
                foreach (MyDbs.MySql001.tUser m in page2.Items)
                {
                    Console.WriteLine("{0}\t{1}\t{2}\t{3}", m.ID, m.Name, m.Email, m.CreateTime);
                }
            }
            else
            {
                Console.WriteLine("no data");
            }
            var page3 = dbService.Page<MyDbs.Sqlite001.tUser>(query);
            Console.WriteLine("MyDbs.Sqlite001.tUser(page3):");
            if (page3.Total > 0)
            {
                foreach (MyDbs.Sqlite001.tUser m in page3.Items)
                {
                    Console.WriteLine("{0}\t{1}\t{2}\t{3}", m.ID, m.Name, m.Email, m.CreateTime);
                }
            }
            else
            {
                Console.WriteLine("no data");
            }
            var page4 = dbService.Page<MyDbs.Sqlite001.tUser>(1,5);
            Console.WriteLine("MyDbs.Sqlite001.tUser(page4):");
            if (page4.Total > 0)
            {
                foreach (MyDbs.Sqlite001.tUser m in page4.Items)
                {
                    Console.WriteLine("{0}\t{1}\t{2}\t{3}", m.ID, m.Name, m.Email, m.CreateTime);
                }
            }
            else
            {
                Console.WriteLine("no data");
            }
            /**/
            #endregion

            #region -- list --

            var listQuery = new ZeroDbs.Common.ListQuery
            {
                Where = "ID>@0",
                Paras = new object[] { 0 },
                Orderby = "ID DESC",
                Top = 10
            };
            var list1 = dbService.Select<MyDbs.SqlServer001.tUser, MyEntity>(listQuery);
            Console.WriteLine("MyDbs.SqlServer001.tUser:");
            if (list1.Count > 0)
            {
                foreach (var m in list1)
                {
                    Console.WriteLine("{0}\t{1}\t{2}\t{3}", m.ID, m.Name, "", m.CreateTime);
                }
            }
            else
            {
                Console.WriteLine("no data");
            }
            var list2 = dbService.Select<MyDbs.MySql001.tUser>(listQuery);
            Console.WriteLine("MyDbs.MySql001.tUser:");
            if (list2.Count > 0)
            {
                foreach (var m in list2)
                {
                    Console.WriteLine("{0}\t{1}\t{2}\t{3}", m.ID, m.Name, "", m.CreateTime);
                }
            }
            else
            {
                Console.WriteLine("no data");
            }
            var list3 = dbService.Select<MyDbs.Sqlite001.tUser, MyEntity>(listQuery);
            Console.WriteLine("MyDbs.Sqlite001.tUser:");
            if (list3.Count > 0)
            {
                foreach (var m in list3)
                {
                    Console.WriteLine("{0}\t{1}\t{2}\t{3}", m.ID, m.Name, "", m.CreateTime);
                }
            }
            else
            {
                Console.WriteLine("no data");
            }
            /**/
            #endregion

            #region -- MaxIdentityPrimaryKeyValue --
            
            var a = dbService.MaxIdentityPrimaryKeyValue<MyDbs.SqlServer001.tUser>();
            Console.WriteLine("MyDbs.SqlServer001.tUser.MaxIdentityPrimaryKeyValue={0}", a);
            a = dbService.MaxIdentityPrimaryKeyValue<MyDbs.SqlServer001.tUser>("ID<@0", 5);
            Console.WriteLine("MyDbs.SqlServer001.tUser.MaxIdentityPrimaryKeyValue(ID<5)={0}", a);
            a = dbService.MaxIdentityPrimaryKeyValue<MyDbs.MySql001.tUser>();
            Console.WriteLine("MyDbs.MySql001.tUser.MaxIdentityPrimaryKeyValue={0}", a);
            a = dbService.MaxIdentityPrimaryKeyValue<MyDbs.MySql001.tUser>("ID<@0", 5);
            Console.WriteLine("MyDbs.MySql001.tUser.MaxIdentityPrimaryKeyValue(ID<5)={0}", a);
            a = dbService.MaxIdentityPrimaryKeyValue<MyDbs.Sqlite001.tUser>();
            Console.WriteLine("MyDbs.Sqlite001.tUser.MaxIdentityPrimaryKeyValue={0}", a);
            a = dbService.MaxIdentityPrimaryKeyValue<MyDbs.Sqlite001.tUser>("ID<@0", 5);
            Console.WriteLine("MyDbs.Sqlite001.tUser.MaxIdentityPrimaryKeyValue(ID<5)={0}", a);
            /**/
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
