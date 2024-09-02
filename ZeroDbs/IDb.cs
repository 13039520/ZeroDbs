using System;
using System.Collections.Generic;
using System.Text;

namespace ZeroDbs
{
    public interface IDb: IDbOperator
    {
        IDbInfo DbInfo { get; }
        Common.SqlBuilder SqlBuilder { get; }
        IDataTypeMaping DataTypeMaping { get; }
        event Common.DbExecuteHandler OnDbExecuteSqlEvent;
        void FireZeroDbExecuteSqlEvent(Common.DbExecuteArgs args);
        System.Data.Common.DbConnection GetDbConnection(bool useSecondDb = false);
        /// <summary>
        /// 获取成功之后数据库连接已经打开，结束之后一定要Dispose()
        /// </summary>
        /// <returns></returns>
        IDbCommand GetDbCommand(bool useSecondDb = false);
        IDbCommand GetDbCommand(System.Data.Common.DbTransaction transaction);
        IDbTransactionScope GetDbTransactionScope(System.Data.IsolationLevel level, string identification = "", string groupId = "");
        ITableInfo GetTable<DbEntity>() where DbEntity : class, new();
        ITableInfo GetTable(string entityFullName);
        List<ITableInfo> GetTables();
        bool DbConnectionTest();
    }
}
