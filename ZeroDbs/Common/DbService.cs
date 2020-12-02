using System;
using System.Collections.Generic;
using System.Text;

namespace ZeroDbs.Common
{
    public class DbService : IDbService
    {
        ICache _Cache = null;
        ILog _Log = null;
        IDbSearcher _DbSearcher = null;
        IDbOperator _DataOperator = null;
        IStrCommon _StrCommon = null;
        public ICache Cache { get { return _Cache; } }
        public ILog Log { get { return _Log; } }
        public IDbOperator DbOperator { get { return _DataOperator; } }
        public IStrCommon StrCommon { get { return _StrCommon; } }
        public IDbSearcher DbSearcher { get { return _DbSearcher; } }
        public DbService(IDbSearcher dbSearcher, ILog log, ICache cache)
        {
            _DbSearcher = dbSearcher != null ? dbSearcher : new DbSearcher(new DbExecuteSqlEvent(_DbExecuteSql));
            _Cache = cache != null ? cache : new LocalMemCache(null);
            _Log = log != null ? log : Logs.Factory.GetLogger("Sql", 7);
            _StrCommon = new Common.StrCommon();
            _DataOperator = new Common.DbOperator(this);
        }
        private void _DbExecuteSql(object sender, DbExecuteSqlEventArgs e)
        {
#if DEBUG
            this.Log.Writer("DbKey={0}&ExecuteType={1}&ExecuteSql=\r\n{2}\r\n&ExecuteResult={3}",
                    e.DbKey,
                    e.ExecuteType,
                    e.ExecuteSql != null && e.ExecuteSql.Count > 0 ? string.Join("\r\n", e.ExecuteSql.ToArray()) : "no sql",
                    e.Message);
#endif
        }
        public IDb GetDb<T>() where T: class, new()
        {
            return DbSearcher.GetDb<T>();
        }
        public IDb GetDb(string entityFullName)
        {
            return DbSearcher.GetDbByEntityFullName(entityFullName);
        }
        public IDb GetDbByDbKey(string dbKey)
        {
            return DbSearcher.GetDb(dbKey);
        }
        public IDbCommand GetDbCommand<T>() where T : class, new()
        {
            return DbSearcher.GetDb<T>().GetDbCommand();
        }
        public IDbCommand GetDbCommand(string entityFullName)
        {
            return DbSearcher.GetDbByEntityFullName(entityFullName).GetDbCommand();
        }
        public IDbCommand GetDbCommandByDbKey(string dbKey)
        {
            return DbSearcher.GetDb(dbKey).GetDbCommand();
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
        public IDbTransactionScope GetDbTransactionScopeByDbKey(string dbKey, System.Data.IsolationLevel level, string identification = "", string groupId = "")
        {
            var db = DbSearcher.GetDb(dbKey);
            return db.GetDbTransactionScope(level, identification, groupId);
        }
        public IDbTransactionScopeCollection GetDbTransactionScopeCollection()
        {
            return new DbTransactionScopeCollection();
        }

    }
}
