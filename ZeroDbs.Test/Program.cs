using System;

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
            dbService = new ZeroDbs.Common.DbService(new Common.DbExecuteSqlEvent((obj, e) => {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine(string.Join(Environment.NewLine, e.ExecuteSql));
                Console.WriteLine("DbKey={0}&Message={1}", e.DbKey, e.Message);
                Console.ResetColor();
            }));
            //CodeGenerator();
            //InsertTest();
            //UpdateTest();
            QueryTest();
        }


        static void CodeGenerator()
        {
            Console.WriteLine("正在生成……");

            ZeroDbs.Tools.CodeGenerator generator = new Tools.CodeGenerator();
            generator.Dbs.Add(new ZeroDbs.Common.DbInfo
            {
                ConnectionString = "Data Source=.;Initial Catalog=ZeroTestDb;User ID=sa;Password=123456;TrustServerCertificate=True;",
                UseKey = "SqlServer001",
                UseType = Common.DbType.SqlServer
            });
            generator.Dbs.Add(new ZeroDbs.Common.DbInfo
            {
                ConnectionString = "Host=localhost;Port=3306;Database=zerotestdb;User ID=root;Password=123456;",
                UseKey = "MySql001",
                UseType = Common.DbType.MySql
            });
            generator.Dbs.Add(new ZeroDbs.Common.DbInfo
            {
                ConnectionString = "Data Source=D:\\Program Files\\SQLiteStudio\\ZeroTestDb.db3;version=3;datetimeformat=CurrentCulture",
                UseKey = "Sqlite001",
                UseType = Common.DbType.Sqlite
            });
            generator.GeneratorConfig = new ZeroDbs.Tools.CodeGenerator.Config
            {
                AppProjectDir = @"D:\Work\ZeroDbs\ZeroDbs.Test",
                AppProjectNamespace = "ZeroDbs.Test",
                AppProjectName = "ZeroDbs.Test",
                EntityDir = @"D:\Work\ZeroDbs\ZeroDbs.Test\MyDbs",
                EntityNamespace = "MyDbs",
                EntityProjectName = "ZeroDbs.Test"
            };
            generator.OnSingleTableGenerated += (e) => {
                Console.WriteLine("[{0}/{1}]\t{2}\t{3}", e.tableNum, e.tableCount, e.table.Name, e.entityClassFullName);
            };
            generator.Run();

            Console.WriteLine("生成成功！");
        }
        static void InsertTest()
        {
            /*
            dbService.Insert(new MyDbs.SqlServer001.tUser { Name = "user004_sqlserver", Email = "user004@domain.com", Password = "123456" });
            dbService.Insert(new MyDbs.MySql001.tUser { Name = "user004_mysql", Email = "user004@domain.com", Password = "123456" });
            dbService.Insert(new MyDbs.Sqlite001.tUser { Name = "user004_sqlite", Email = "user004@domain.com", Password = "123456" });
            */

            var entities1 = new System.Collections.Generic.List<MyDbs.SqlServer001.tUser>
            {
                new MyDbs.SqlServer001.tUser{Name ="user005_sqlserver", Email="user005@domain.com", Password="123456"},
                new MyDbs.SqlServer001.tUser{Name ="user006_sqlserver", Email="user006@domain.com", Password="123456"},
                new MyDbs.SqlServer001.tUser{Name ="user007_sqlserver", Email="user007@domain.com", Password="123456"},
                new MyDbs.SqlServer001.tUser{Name ="user008_sqlserver", Email="user008@domain.com", Password="123456"},
                new MyDbs.SqlServer001.tUser{Name ="user009_sqlserver", Email="user009@domain.com", Password="123456"}
            };
            dbService.Insert(entities1);
            var entities2 = new System.Collections.Generic.List<MyDbs.MySql001.tUser>
            {
                new MyDbs.MySql001.tUser{Name ="user005_sqlserver", Email="user005@domain.com", Password="123456"},
                new MyDbs.MySql001.tUser{Name ="user006_sqlserver", Email="user006@domain.com", Password="123456"},
                new MyDbs.MySql001.tUser{Name ="user007_sqlserver", Email="user007@domain.com", Password="123456"},
                new MyDbs.MySql001.tUser{Name ="user008_sqlserver", Email="user008@domain.com", Password="123456"},
                new MyDbs.MySql001.tUser{Name ="user009_sqlserver", Email="user009@domain.com", Password="123456"}
            };
            dbService.Insert(entities2);
            var entities3 = new System.Collections.Generic.List<MyDbs.Sqlite001.tUser>
            {
                new MyDbs.Sqlite001.tUser{Name ="user005_sqlserver", Email="user005@domain.com", Password="123456"},
                new MyDbs.Sqlite001.tUser{Name ="user006_sqlserver", Email="user006@domain.com", Password="123456"},
                new MyDbs.Sqlite001.tUser{Name ="user007_sqlserver", Email="user007@domain.com", Password="123456"},
                new MyDbs.Sqlite001.tUser{Name ="user008_sqlserver", Email="user008@domain.com", Password="123456"},
                new MyDbs.Sqlite001.tUser{Name ="user009_sqlserver", Email="user009@domain.com", Password="123456"}
            };
            dbService.Insert(entities3);
        }
        static void UpdateTest()
        {
            /*
            dbService.Update(new MyDbs.SqlServer001.tUser { ID = 1, Name = "user009_sqlserver", Email = "user009@domain.com", Password = "123456789" });
            dbService.Update(new MyDbs.MySql001.tUser { ID = 1, Name = "user009_mysql", Email = "user009@domain.com", Password = "123456789" });
            dbService.Update(new MyDbs.Sqlite001.tUser { ID = 1, Name = "user009_sqlite", Email = "user009@domain.com", Password = "123456789" });
            */

            /*
            var nvc = new System.Collections.Specialized.NameValueCollection();
            nvc.Add("ID", "1");
            nvc.Add("Password", "aaaaaa");
            nvc.Add("Email", "aaaaa@bbbb.es");
            dbService.Update<MyDbs.SqlServer001.tUser>(nvc);
            dbService.Update<MyDbs.MySql001.tUser>(nvc);
            dbService.Update<MyDbs.Sqlite001.tUser>(nvc);
            */

            DateTime myTime = DateTime.Now;
            dbService.UpdateFromCustomEntity<MyDbs.SqlServer001.tUser>(new { ID = 1, Name = "abcdefgh", createTime = myTime });
            dbService.UpdateFromCustomEntity<MyDbs.MySql001.tUser>(new { ID = 1, Name = "abcdefgh", createTime = myTime });
            dbService.UpdateFromCustomEntity<MyDbs.Sqlite001.tUser>(new { ID = 1, Name = "abcdefgh", createTime = myTime });

        }
        static void QueryTest()
        {
            #region -- page --
            
            var query = new ZeroDbs.Common.PageQuery
            {
                Page = 1,
                Size = 5,
                Orderby = "ID DESC",
                Where = "ID>@0",
                Paras = new object[] { 2 },//@0=2
                Fields = new string[0],
                UniqueField = ""
            };
            var page1 = dbService.Page<MyDbs.SqlServer001.tUser>(query);
            Console.WriteLine("MyDbs.SqlServer001.tUser:");
            if (page1.Total > 0)
            {
                foreach (MyDbs.SqlServer001.tUser m in page1.Items)
                {
                    Console.WriteLine("{0}\t{1}\t{2}\t{3}", m.ID, m.Name, m.Email, m.CreateTime);
                }
            }
            else
            {
                Console.WriteLine("no data");
            }
            var page2 = dbService.Page<MyDbs.MySql001.tUser>(query);
            Console.WriteLine("MyDbs.MySql001.tUser:");
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
            Console.WriteLine("MyDbs.Sqlite001.tUser:");
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
            var list1 = dbService.Select<MyDbs.SqlServer001.tUser>(listQuery);
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

        }
        class MyEntity
        {
            public object ID { get; set; }
            public string Name { get; set; }
            public DateTime CreateTime { get; set; }
        }

    }
}
