using System;
using System.Collections.Generic;
using System.Text;

namespace ZeroDbs.Common
{
    public class DbExecuteArgs : EventArgs
    {
        public readonly string DbKey;
        public readonly string TransactionInfo;
        public readonly string ExecuteSql;
        public readonly DbExecuteSqlType ExecuteType;
        public readonly string Message;
        public DbExecuteArgs(string dbKey, string sql, string transactionInfo, DbExecuteSqlType type, string message)
        {
            this.DbKey = dbKey;
            this.ExecuteSql = sql;
            this.ExecuteType = type;
            this.Message = message;
            this.TransactionInfo = transactionInfo;
        }
    }
}
