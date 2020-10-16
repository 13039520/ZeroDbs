using System;

namespace ZeroDbs.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            /*Console.WriteLine("正在生成……");
            ZeroDbs.Tools.EntityBuilder.Builder(
                new ZeroDbs.Interfaces.Common.DbConfigDatabaseInfo
                {
                    dbConnectionString = "Data Source=.;Initial Catalog=MyTestDb;User ID=sa;Password=yangshaogao;",
                    dbKey = "Article",
                    dbType = "SqlServer"
                },
                @"D:\Work\ZeroDbs\ZeroDbs.Test\Models",
                @"D:\Work\ZeroDbs\ZeroDbs.Test",
                "Models");
            Console.WriteLine("生成成功！");*/

            ZeroDbs.Interfaces.IDbService dbService = new ZeroDbs.Interfaces.Common.DbService(new ZeroDbs.DataAccess.DbSearcher(null), null, null);
            long page = 1;
            long pageSize = 1000;
            var pageData = dbService.DbOperator.Page<Models.Article.tArticleCategory>(page, pageSize, "");
            if (pageData.Total > 0)
            {
                foreach (Models.Article.tArticleCategory m in pageData.Items)
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
