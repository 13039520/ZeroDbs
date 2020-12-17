using System;
using System.Collections.Generic;
using System.Text;

namespace ZeroDbs
{
    public interface IDbService: IDbOperator
    {
        ICache Cache { get; }
        Dictionary<string, IDb> GetDbs();
        Dictionary<string, IDb> GetDbs(List<Common.DbConfigDatabaseInfo> dbConfigList);
        IDb GetDb<T>() where T : class, new();
        IDb GetDb(string entityFullName);
        IDb GetDbByDbKey(string dbKey);
        IStrCommon StrCommon { get; }
        IDbCommand GetDbCommand<T>() where T : class, new();
        IDbCommand GetDbCommand(string entityFullName);
        IDbCommand GetDbCommandByDbKey(string dbKey);
        IDbTransactionScope GetDbTransactionScope<T>(System.Data.IsolationLevel level, string identification = "", string groupId = "") where T : class, new();
        IDbTransactionScope GetDbTransactionScope(string entityFullName, System.Data.IsolationLevel level, string identification = "", string groupId = "");
        IDbTransactionScope GetDbTransactionScopeByDbKey(string dbKey, System.Data.IsolationLevel level, string identification = "", string groupId = "");
        IDbTransactionScopeCollection GetDbTransactionScopeCollection();


        bool AddZeroDbMapping<T>(string dbKey, string tableName) where T : class;
        bool AddZeroDbMapping(string entityFullName, string dbKey, string tableName);

    }
}
