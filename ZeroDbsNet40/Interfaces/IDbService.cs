using System;
using System.Collections.Generic;
using System.Text;

namespace ZeroDbs.Interfaces
{
    public interface IDbService
    {
        ICache Cache { get; }
        ILog Log { get; }
        IDbSearcher DbSearcher { get; }
        IDb GetDb<T>() where T : class, new();
        IDb GetDb(string entityFullName);
        IDbOperator DbOperator { get; }
        IStrCommon StrCommon { get;}
        IDbCommand GetDbCommand<T>() where T : class, new();
        IDbCommand GetDbCommand(string entityFullName);
        IDbTransactionScope GetDbTransactionScope<T>(System.Data.IsolationLevel level, string identification = "", string groupId = "") where T : class, new();
        IDbTransactionScope GetDbTransactionScope(string entityFullName, System.Data.IsolationLevel level, string identification = "", string groupId = "");
        IDbTransactionScopeCollection GetDbTransactionScopeCollection();
    }
}
