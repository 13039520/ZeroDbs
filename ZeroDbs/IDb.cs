using System;
using System.Collections.Generic;
using System.Text;

namespace ZeroDbs
{
    public interface IDb: IDbOperator
    {
        IDbInfo Database { get; }
        Common.SqlBuilder SqlBuilder { get; }
        IDataTypeMaping DataTypeMaping { get; }
        event Common.DbExecuteHandler OnDbExecuteSqlEvent;
        void FireZeroDbExecuteSqlEvent(Common.DbExecuteArgs args);
        System.Data.Common.DbConnection GetDbConnection();
        /// <summary>
        /// 获取成功之后数据库连接已经打开，结束之后一定要Dispose()
        /// </summary>
        /// <returns></returns>
        IDbCommand GetDbCommand();
        IDbCommand GetDbCommand(System.Data.Common.DbTransaction transaction);
        IDbTransactionScope GetDbTransactionScope(System.Data.IsolationLevel level, string identification = "", string groupId = "");
        ITableInfo GetTable<DbEntity>() where DbEntity : class, new();
        List<ITableInfo> GetTables();
        bool DbConnectionTest();
    }
}
