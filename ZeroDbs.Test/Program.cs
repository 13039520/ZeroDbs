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
            dbService = new ZeroDbs.Common.DbService();
            //CodeGenerator();
            InsertTest();
            DataQuery();

        }


        static void CodeGenerator()
        {
            Console.WriteLine("正在生成……");

            ZeroDbs.Tools.CodeGenerator generator = new Tools.CodeGenerator();
            generator.Dbs.Add(new ZeroDbs.Common.DbConfigDatabaseInfo
            {
                dbConnectionString = "Data Source=.;Initial Catalog=MyTestDb;User ID=sa;Password=123;",
                dbKey = "TestDb",
                dbType = "SqlServer"
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

        static void DataQuery()
        {
            using(var cmd = dbService.GetDbCommand(""))
            {
                cmd.SqlDeleteBuilder("a").ToString();
            }
            long page = 1;
            long pageSize = 1000;
            dbService.AddZeroDbMapping<MyCategory>("TestDb", "T_ArticleCategory");
            var pageData = dbService.Page<MyCategory>(page, pageSize, "IsDel=0");
            if (pageData.Total > 0)
            {
                foreach (MyCategory m in pageData.Items)
                {
                    Console.WriteLine("{0}\t{1}\t{2}\n", m.ID, m.Name, false);
                }
                Console.WriteLine("total={0}&page={1}&pageSize={2}&currentPageListCount={3}", pageData.Total, page, pageSize, pageData.Items.Count);
            }
            else
            {
                Console.WriteLine("no data");
            }
        }
        static void InsertTest()
        {
            using(var cmd = dbService.GetDbCommand<MyDbs.TestDb.tArticleCategory>())
            {
                cmd.CommandText = "INSERT INTO T_ArticleCategory(ID,IsDel,Name) VALUES(@ID,@IsDel,@Name)";
                cmd.LoadParameters(new MyDbs.TestDb.tArticleCategory { ID = 13, IsDel = false, Name = "Test777" });
                cmd.ExecuteNonQuery();
            }
        }
        class MyCategory
        {
            public long ID { get; set; }
            public string Name { get; set; }
        }

    }
}
