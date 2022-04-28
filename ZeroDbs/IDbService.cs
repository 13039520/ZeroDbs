using System;
using System.Collections.Generic;
using System.Text;

namespace ZeroDbs
{
    public interface IDbService: IDbOperator
    {
        Dictionary<string, IDb> GetDbs();
        Dictionary<string, IDb> GetDbs(List<IDbInfo> dbConfigList);
        IDb GetDb<DbEntity>() where DbEntity : class, new();
        IDb GetDb(string entityFullName);
        IDb GetDbByDbKey(string dbKey);
        IStrCommon StrCommon { get; }
        IDbCommand GetDbCommand<DbEntity>() where DbEntity : class, new();
        IDbCommand GetDbCommand(string entityFullName);
        IDbCommand GetDbCommandByDbKey(string dbKey);
        IDbTransactionScope GetDbTransactionScope<DbEntity>(System.Data.IsolationLevel level, string identification = "", string groupId = "") where DbEntity : class, new();
        IDbTransactionScope GetDbTransactionScope(string entityFullName, System.Data.IsolationLevel level, string identification = "", string groupId = "");
        IDbTransactionScope GetDbTransactionScopeByDbKey(string dbKey, System.Data.IsolationLevel level, string identification = "", string groupId = "");
        IDbTransactionScopeCollection GetDbTransactionScopeCollection();


        bool AddZeroDbMapping<DbEntity>(string dbKey, string tableName) where DbEntity : class;
        bool AddZeroDbMapping(string entityFullName, string dbKey, string tableName);

    }
}
