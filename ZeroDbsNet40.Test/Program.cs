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
            /*Console.WriteLine("正在生成……");
            ZeroDbs.Tools.EntityBuilder.Builder(
                new ZeroDbs.Interfaces.Common.DbConfigDatabaseInfo
                {
                    dbConnectionString = "Data Source=.;Initial Catalog=BankStatementAnalysis;User ID=sa;Password=yangshaogao;",
                    dbKey = "BSA",
                    dbType = "SqlServer"
                },
                @"D:\Work\ZeroDbs\ZeroDbsNet40.Test\Models",
                @"D:\Work\ZeroDbs\ZeroDbsNet40.Test",
                "Models");
            Console.WriteLine("生成成功！");*/

            ZeroDbs.Interfaces.IDbService dbService = new ZeroDbs.Interfaces.Common.DbService(new ZeroDbs.DataAccess.DbSearcher(null), null, null);
            var pageData = dbService.DbOperator.Page<Models.BSA.tCaseBankStatement>(1, 100, "");
            if (pageData.Total > 0)
            {
                foreach(Models.BSA.tCaseBankStatement m in pageData.Items)
                {
                    Console.WriteLine("{0}\t{1}\t{2}\n",m.BSClientName,m.BSDealDirection,m.BSPartnerName);
                }
                Console.WriteLine("共有{0}条，当前返回{1}条数据！", pageData.Total, pageData.Items.Count);
            }
            else
            {
                Console.WriteLine("没有数据！");
            }

        }
    }
}
