using System;
using System.Collections.Generic;
using System.Text;

namespace ZeroDbs.Common
{
    public class DbExecuteSqlEventArgs : EventArgs
    {
        public string DbKey;
        public readonly List<string> ExecuteSql;
        public readonly DbExecuteSqlType ExecuteType;
        public readonly string Message;
        public DbExecuteSqlEventArgs(string DbKey, List<string> Sql, DbExecuteSqlType Type, string Message)
        {
            this.DbKey = DbKey;
            this.ExecuteSql = Sql;
            this.ExecuteType = Type;
            this.Message = Message;
        }
    }
}
