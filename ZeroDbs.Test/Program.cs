using System;

namespace ZeroDbs.Test
{
    /// <summary>
    /// 
    /// </summary>
    class Program
    {
        static ZeroDbs.Interfaces.IDbService dbService = null;

        static void Main(string[] args)
        {
            /*Console.WriteLine("正在生成……");
            ZeroDbs.Tools.EntityBuilder.Builder(
                new ZeroDbs.Interfaces.Common.DbConfigDatabaseInfo
                {
                    dbConnectionString = "Data Source=.;Initial Catalog=MyTestDb;User ID=sa;Password=123;",
                    dbKey = "TestDb",
                    dbType = "SqlServer"
                },
                @"D:\Work\ZeroDbs\ZeroDbs.Test\MyDbs",
                @"D:\Work\ZeroDbs\ZeroDbs.Test",
                "MyDbs");
            Console.WriteLine("生成成功！");
            */

            /**/
            dbService = new ZeroDbs.Interfaces.Common.DbService(
                new ZeroDbs.DataAccess.DbSearcher(new ZeroDbs.Interfaces.Common.DbExecuteSqlEvent((sender, e) => {
#if DEBUG
                     dbService.Log.Writer("DbKey={0}&ExecuteType={1}&ExecuteSql=\r\n{2}\r\n&ExecuteResult={3}",
                    e.DbKey,
                    e.ExecuteType,
                    e.ExecuteSql != null && e.ExecuteSql.Count > 0 ? string.Join("\r\n", e.ExecuteSql.ToArray()) : "no sql",
                    e.Message);
#endif
                })),
                ZeroDbs.Logs.Factory.GetLogger("sql", 7),
                new ZeroDbs.Caches.LocalMemCache(null));

            long page = 1;
            long pageSize = 1000;
            var pageData = dbService.DbOperator.Page<MyDbs.TestDb.tArticleCategory>(page, pageSize, "ID>0");
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
