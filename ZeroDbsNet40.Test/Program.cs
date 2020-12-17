using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZeroDbsNet40.Test
{
    class Program
    {
        static void Main(string[] args)
        {

            //CodeGenerator();

            DataQuery();

            while (true)
            {
                System.Threading.Thread.Sleep(1000);
            }
        }


        static void CodeGenerator()
        {
            Console.WriteLine("正在生成……");

            ZeroDbs.Tools.CodeGenerator generator = new ZeroDbs.Tools.CodeGenerator();
            generator.Dbs.Add(new ZeroDbs.Common.DbConfigDatabaseInfo
            {
                dbConnectionString = "Data Source=.;Initial Catalog=MyTestDb;User ID=sa;Password=123;",
                dbKey = "TestDb",
                dbType = "SqlServer"
            });
            generator.GeneratorConfig = new ZeroDbs.Tools.CodeGenerator.Config
            {
                AppProjectDir = @"D:\Work\ZeroDbs\ZeroDbs.Test40",
                AppProjectNamespace = "ZeroDbs.Test40",
                AppProjectName = "ZeroDbs.Test40",
                EntityDir = @"D:\Work\ZeroDbs\ZeroDbs.Test40\MyDbs",
                EntityNamespace = "MyDbs",
                EntityProjectName = "ZeroDbs.Test40"
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
            ZeroDbs.ILog log = ZeroDbs.Logs.Factory.GetLogger("sql", 7);
            dbService = new ZeroDbs.Common.DbService(
                new ZeroDbs.Common.DbExecuteSqlEvent((sender, e) => {
#if DEBUG
                    log.Writer("DbKey={0}&ExecuteType={1}&ExecuteSql=\r\n{2}\r\n&ExecuteResult={3}",
                    e.DbKey,
                    e.ExecuteType,
                    e.ExecuteSql != null && e.ExecuteSql.Count > 0 ? string.Join("\r\n", e.ExecuteSql.ToArray()) : "no sql",
                    e.Message);
#endif
                }),
                new ZeroDbs.Common.LocalMemCache(null));

            long page = 1;
            long pageSize = 1000;
            var pageData = dbService.Page<MyDbs.TestDb.tArticleCategory>(page, pageSize, "ID>0");
            if (pageData.Total > 0)
            {
                foreach (MyDbs.TestDb.tArticleCategory m in pageData.Items)
                {
                    Console.WriteLine("{0}\t{1}\t{2}\n", m.ID, m.Name, m.IsDel);
                }
                Console.WriteLine("total={0}&page={1}&pageSize={2}&currentPageListCount={3}", pageData.Total, page, pageSize, pageData.Items.Count);
            }
            else
            {
                Console.WriteLine("no data");
            }
        }


    }
}
