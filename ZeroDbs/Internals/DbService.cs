using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZeroDbs
{
    internal class DbService : IDbService
    {
        private readonly List<DbCreateHandler> _dbCreatFuncs;
        private readonly System.Collections.Concurrent.ConcurrentDictionary<string, IDatabase> _dbs = new System.Collections.Concurrent.ConcurrentDictionary<string, IDatabase>(StringComparer.OrdinalIgnoreCase);
        private ISnowflakeIdGenerator _snowflake;
        public ISnowflakeIdGenerator Snowflake { get { return _snowflake; } }
        public int Count {  get { return _dbs.Count; } }

        public DbService(ISnowflakeIdGenerator snowflake, List<DbCreateHandler> dbCreateHandlers, List<IDbConfig> configs)
        {
            if (snowflake == null) { throw new ArgumentNullException(nameof(snowflake)); }
            if (dbCreateHandlers == null || dbCreateHandlers.Count < 1) { throw new ArgumentNullException(nameof(dbCreateHandlers)); }
            _dbCreatFuncs = dbCreateHandlers;
            _snowflake = snowflake;
            if (configs != null)
            {
                foreach (var c in configs)
                {
                    CreateDb(c);
                }
            }
        }
        private void CreateDb(IDbConfig dbConfig)
        {
            if (dbConfig == null) { throw new ArgumentNullException(nameof(dbConfig)); }
            if (string.IsNullOrWhiteSpace(dbConfig.DbKey)) { throw new ArgumentNullException(nameof(dbConfig.DbKey)); }
            if (_dbs.ContainsKey(dbConfig.DbKey)) {throw new InvalidOperationException($"DbKey already exists: {dbConfig.DbKey}"); }
            if (string.IsNullOrWhiteSpace(dbConfig.DbType)) { throw new InvalidOperationException($"DbType is null: {dbConfig.DbKey}"); }
            if (string.IsNullOrWhiteSpace(dbConfig.ConnectionString)) { throw new InvalidOperationException($"ConnectionString is null: {dbConfig.DbKey}"); }

            foreach (DbCreateHandler func in _dbCreatFuncs)
            {
                if (func == null) { continue; }
                var db = func(dbConfig, _snowflake);
                if (db != null)
                {
                    _dbs.TryAdd(dbConfig.DbKey, db);
                    return;
                }
            }
            throw new InvalidOperationException($"DbType is not supported: {dbConfig.DbType}");
        }

        public void AddNewDb(IDbConfig dbConfig)
        {
            CreateDb(dbConfig);
        }
        public IDatabase GetDb(string dbKey)
        {
            if (!_dbs.TryGetValue(dbKey, out var db))
            {
                throw new KeyNotFoundException($"DbKey not found: {dbKey}");
            }
            return db;
        }
        public IEnumerator<IDatabase> GetEnumerator()
        {
            return _dbs.Values.GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
