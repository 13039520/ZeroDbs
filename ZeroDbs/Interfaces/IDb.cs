using System;
using System.Collections.Generic;
using System.Text;

namespace ZeroDbs.Interfaces
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
        IDbTransactionScope GetDbTransactionScope(System.Data.IsolationLevel level, string identification = "", string groupId = "");
        Common.DbDataTableInfo GetDbDataTableInfo<T>() where T : class, new();
        List<Common.DbDataTableInfo> GetDbDataTableInfoAll();
        bool DbConnectionTest();
    }
}
