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
                //Console.WriteLine(string.Join(Environment.NewLine, e.ExecuteSql));
                //Console.WriteLine("DbKey={0}&Message={1}", e.DbKey, e.Message);
            }));
            //CodeGenerator();
            //InsertTest();
            UpdateTest();
            QueryTest();
        }


        static void CodeGenerator()
        {
            Console.WriteLine("正在生成……");

            ZeroDbs.Tools.CodeGenerator generator = new Tools.CodeGenerator();
            generator.Dbs.Add(new ZeroDbs.Common.DbConfigDatabaseInfo
            {
                dbConnectionString = "Data Source=.;Initial Catalog=ZeroTestDb;User ID=sa;Password=123456;TrustServerCertificate=True;",
                dbKey = "SqlServer001",
                dbType = "SqlServer"
            });
            generator.Dbs.Add(new ZeroDbs.Common.DbConfigDatabaseInfo
            {
                dbConnectionString = "Host=localhost;Port=3306;Database=zerotestdb;User ID=root;Password=123456;",
                dbKey = "MySql001",
                dbType = "MySql"
            });
            generator.Dbs.Add(new ZeroDbs.Common.DbConfigDatabaseInfo
            {
                dbConnectionString = "Data Source=D:\\Program Files\\SQLiteStudio\\ZeroTestDb.db3;version=3;datetimeformat=CurrentCulture",
                dbKey = "Sqlite001",
                dbType = "Sqlite"
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
            dbService.Update(new MyDbs.SqlServer001.tUser { Name = "user001_sqlserver", Email = "user001@domain.com", Password = "123456" });
            dbService.Update(new MyDbs.MySql001.tUser { Name = "user001_mysql", Email = "user001@domain.com", Password = "123456" });
            dbService.Update(new MyDbs.Sqlite001.tUser { Name = "user001_sqlite", Email = "user001@domain.com", Password = "123456" });
        }
        static void UpdateTest()
        {
            //dbService.Update(new MyDbs.SqlServer001.tUser { ID = 1, Name = "user009_sqlserver", Email = "user009@domain.com", Password = "123456789" });
            //dbService.Update(new MyDbs.MySql001.tUser { ID = 1, Name = "user009_mysql", Email = "user009@domain.com", Password = "123456789" });
            //dbService.Update(new MyDbs.Sqlite001.tUser { ID = 1, Name = "user009_sqlite", Email = "user009@domain.com", Password = "123456789" });
            var nvc = new System.Collections.Specialized.NameValueCollection();
            nvc.Add("ID", "1");
            nvc.Add("Password", "aaaaaa");
            nvc.Add("Email", "aaaaa@bbbb.es");
            dbService.Update<MyDbs.SqlServer001.tUser>(nvc);
            dbService.Update<MyDbs.MySql001.tUser>(nvc);
            dbService.Update<MyDbs.Sqlite001.tUser>(nvc);
        }
        static void QueryTest()
        {
            long page = 1;
            long pageSize = 100;
            var pageData = dbService.Page<MyDbs.SqlServer001.tUser>(page, pageSize, "");
            Console.WriteLine("MyDbs.SqlServer001.tUser:");
            if (pageData.Total > 0)
            {
                foreach (MyDbs.SqlServer001.tUser m in pageData.Items)
                {
                    Console.WriteLine("{0}\t{1}\t{2}\t{3}\n", m.ID, m.Name, m.Email, m.CreateTime);
                }
            }
            else
            {
                Console.WriteLine("no data");
            }
            var pageData2 = dbService.Page<MyDbs.MySql001.tUser>(page, pageSize, "");
            Console.WriteLine("MyDbs.MySql001.tUser:");
            if (pageData2.Total > 0)
            {
                foreach (MyDbs.MySql001.tUser m in pageData2.Items)
                {
                    Console.WriteLine("{0}\t{1}\t{2}\t{3}\n", m.ID, m.Name, m.Email, m.CreateTime);
                }
            }
            else
            {
                Console.WriteLine("no data");
            }
            var pageData3 = dbService.Page<MyDbs.Sqlite001.tUser>(page, pageSize, "");
            Console.WriteLine("MyDbs.Sqlite001.tUser:");
            if (pageData3.Total > 0)
            {
                foreach (MyDbs.Sqlite001.tUser m in pageData3.Items)
                {
                    Console.WriteLine("{0}\t{1}\t{2}\t{3}\n", m.ID, m.Name, m.Email, m.CreateTime);
                }
            }
            else
            {
                Console.WriteLine("no data");
            }
        }


    }
}
