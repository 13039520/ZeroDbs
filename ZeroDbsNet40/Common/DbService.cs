using System;
using System.Collections.Generic;
using System.Text;

namespace ZeroDbs.Common
{
    public class DbService:IDbService
    {
        ICache _Cache = null;
        ILog _Log = null;
        IDbSearcher _DbSearcher = null;
        IDbOperator _DataOperator = null;
        IStrCommon _StrCommon = null;
        public ICache Cache { get { return _Cache; } }
        public ILog Log { get { return _Log; } }
        public IDbOperator DbOperator { get { return _DataOperator;  } }
        public IStrCommon StrCommon { get { return _StrCommon; } }
        public IDbSearcher DbSearcher { get { return _DbSearcher; } }
        public DbService(IDbSearcher dbSearcher, ILog log, ICache cache)
        {
            _DbSearcher = dbSearcher;
            _Cache = cache;
            _Log = log;
            _StrCommon = new Common.StrCommon();
            _DataOperator = new Common.DbOperator(this);
        }
        public IDb GetDb<T>() where T: class, new()
        {
            return DbSearcher.GetDb<T>();
        }
        public IDb GetDb(string entityFullName)
        {
            return DbSearcher.GetDbByEntityFullName(entityFullName);
        }
        public IDbCommand GetDbCommand<T>() where T : class, new()
        {
            return DbSearcher.GetDb<T>().GetDbCommand();
        }
        public IDbCommand GetDbCommand(string entityFullName)
        {
            return DbSearcher.GetDbByEntityFullName(entityFullName).GetDbCommand();
        }
        public IDbTransactionScope GetDbTransactionScope<T>(System.Data.IsolationLevel level, string identification = "", string groupId = "") where T : class, new()
        {
            var db = this.GetDb<T>();
            return db.GetDbTransactionScope(level, identification, groupId);
        }
        public IDbTransactionScope GetDbTransactionScope(string entityFullName, System.Data.IsolationLevel level, string identification = "", string groupId = "")
        {
            var db = this.GetDb(entityFullName);
            return db.GetDbTransactionScope(level, identification, groupId);
        }
        public IDbTransactionScopeCollection GetDbTransactionScopeCollection()
        {
            return new DbTransactionScopeCollection();
        }

    }
}
