using System;
using System.Collections.Generic;
using System.Text;

namespace ZeroDbs.Interfaces.Common
{
    public class DbService:ZeroDbs.Interfaces.IDbService
    {
        ZeroDbs.Interfaces.ICache _Cache = null;
        ZeroDbs.Interfaces.ILog _Log = null;
        ZeroDbs.Interfaces.IDbSearcher _DbSearcher = null;
        ZeroDbs.Interfaces.IDbOperator _DataOperator = null;
        ZeroDbs.Interfaces.IStrCommon _StrCommon = null;
        public ZeroDbs.Interfaces.ICache Cache { get { return _Cache; } }
        public ZeroDbs.Interfaces.ILog Log { get { return _Log; } }
        public ZeroDbs.Interfaces.IDbOperator DbOperator { get { return _DataOperator;  } }
        public ZeroDbs.Interfaces.IStrCommon StrCommon { get { return _StrCommon; } }
        public ZeroDbs.Interfaces.IDbSearcher DbSearcher { get { return _DbSearcher; } }
        public DbService(ZeroDbs.Interfaces.IDbSearcher dbSearcher, ZeroDbs.Interfaces.ILog log, ZeroDbs.Interfaces.ICache cache)
        {
            _DbSearcher = dbSearcher;
            _Cache = cache;
            _Log = log;
            _StrCommon = new Common.StrCommon();
            _DataOperator = new Common.DbOperator(this);
        }
        public ZeroDbs.Interfaces.IDb GetDb<T>() where T: class, new()
        {
            return DbSearcher.GetDb<T>();
        }
        public ZeroDbs.Interfaces.IDb GetDb(string entityFullName)
        {
            return DbSearcher.GetDbByEntityFullName(entityFullName);
        }
        public ZeroDbs.Interfaces.IDbCommand GetDbCommand<T>() where T : class, new()
        {
            return DbSearcher.GetDb<T>().GetDbCommand();
        }
        public ZeroDbs.Interfaces.IDbCommand GetDbCommand(string entityFullName)
        {
            return DbSearcher.GetDbByEntityFullName(entityFullName).GetDbCommand();
        }
        public ZeroDbs.Interfaces.IDbTransactionScope GetDbTransactionScope<T>(System.Data.IsolationLevel level, string identification = "", string groupId = "") where T : class, new()
        {
            var db = this.GetDb<T>();
            return db.GetDbTransactionScope(level, identification, groupId);
        }
        public ZeroDbs.Interfaces.IDbTransactionScope GetDbTransactionScope(string entityFullName, System.Data.IsolationLevel level, string identification = "", string groupId = "")
        {
            var db = this.GetDb(entityFullName);
            return db.GetDbTransactionScope(level, identification, groupId);
        }
        public ZeroDbs.Interfaces.IDbTransactionScopeCollection GetDbTransactionScopeCollection()
        {
            return new ZeroDbs.Interfaces.Common.DbTransactionScopeCollection();
        }

    }
}
