using System;
using System.Collections.Generic;
using System.Text;

namespace ZeroDbs
{
    public interface IDb: IDbOperator
    {
        Common.DbConfigDatabaseInfo DbConfigDatabaseInfo { get; }
        IDbSqlBuilder DbSqlBuilder { get; }
        IDataTypeMaping DbDataTypeMaping { get; }
        event Common.DbExecuteSqlEvent OnDbExecuteSqlEvent;
        void FireZeroDbExecuteSqlEvent(Common.DbExecuteSqlEventArgs args);
        System.Data.Common.DbConnection GetDbConnection();
        /// <summary>
        /// 获取成功之后数据库连接已经打开，结束之后一定要Dispose()
        /// </summary>
        /// <returns></returns>
        IDbCommand GetDbCommand();
        IDbCommand GetDbCommand(System.Data.Common.DbTransaction transaction);
        IDbTransactionScope GetDbTransactionScope(System.Data.IsolationLevel level, string identification = "", string groupId = "");
        Common.DbDataTableInfo GetTable<T>() where T : class, new();
        List<Common.DbDataTableInfo> GetTables();
        bool DbConnectionTest();
    }
}
