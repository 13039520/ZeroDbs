﻿using System;

namespace ZeroDbs.Test
{
    /// <summary>
    /// 
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {

            //CodeGenerator();

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


        static ZeroDbs.IDbService dbService = null;
        static void DataQuery()
        {
            dbService = new ZeroDbs.Common.DbService(
                new ZeroDbs.Common.DbExecuteSqlEvent((sender, e) => {
#if DEBUG
                     dbService.Log.Writer("DbKey={0}&ExecuteType={1}&ExecuteSql=\r\n{2}\r\n&ExecuteResult={3}",
                    e.DbKey,
                    e.ExecuteType,
                    e.ExecuteSql != null && e.ExecuteSql.Count > 0 ? string.Join("\r\n", e.ExecuteSql.ToArray()) : "no sql",
                    e.Message);
#endif
                }),
                new ZeroDbs.Common.LocalMemCache(null));

            long page = 1;
            long pageSize = 1000;
            dbService.AddZeroDbMapping<MyCategory>("TestDb", "T_ArticleCategory");
            var pageData = dbService.Page<MyCategory>(page, pageSize, "ID>0");
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
        class MyCategory
        {
            public long ID { get; set; }
            public string Name { get; set; }
        }

    }
}
