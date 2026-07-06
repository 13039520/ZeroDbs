using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZeroDbs
{
    public static class DbFactory
    {
        private static readonly object _dbServiceLock = new object();
        private static readonly List<DbCreateHandler> _dbCreatFuncs = new List<DbCreateHandler>
        {
            (c,sn)=>{ if(!c.DbType.Equals("PostgreSQL", StringComparison.OrdinalIgnoreCase)){ return null; } return new PostgreSQL(c.DbKey, c.ConnectionString, sn); },
            (c,sn)=>{ if(!c.DbType.Equals("SQLite", StringComparison.OrdinalIgnoreCase)){ return null; } return new SQLite(c.DbKey, c.ConnectionString, sn); },
            (c,sn)=>{ if(!c.DbType.Equals("SqlServer", StringComparison.OrdinalIgnoreCase)){ return null; } return new SqlServer(c.DbKey, c.ConnectionString, sn); },
            (c,sn)=>{ if(!c.DbType.Equals("MySql", StringComparison.OrdinalIgnoreCase)){ return null; } return new MySQL(c.DbKey, c.ConnectionString, sn); },
        };
        private static volatile IDbService? _dbService;
        public static void AddDbCreator(DbCreateHandler createHandler)
        {
            if (createHandler == null) { throw new ArgumentNullException(nameof(createHandler)); }
            _dbCreatFuncs.Add(createHandler);
        }
        public static IDbConfig CreateDbConfig(string dbKey, string dbType, string dbConnectionString)
        {
            if (string.IsNullOrWhiteSpace(dbKey)) { throw new ArgumentNullException(nameof(dbKey)); }
            if (string.IsNullOrWhiteSpace(dbType)) { throw new ArgumentNullException(nameof(dbType)); }
            if (string.IsNullOrWhiteSpace(dbConnectionString)) { throw new ArgumentNullException(nameof(dbConnectionString)); }
            return new DbConfig { DbKey = dbKey, DbType = dbType, ConnectionString = dbConnectionString };
        }
        public static ISnowflakeIdGenerator CreateSnowflakeIdGenerator(int snowflakeDataCenterId, int snowflakeWorkerId)
        {
            return new SnowflakeIdGenerator(snowflakeWorkerId, snowflakeDataCenterId);
        }
        public static IDatabase CreateDatabase(IDbConfig config, ISnowflakeIdGenerator snowflake)
        {
            if (config == null) {  throw new ArgumentNullException(nameof(config)); }
            if (string.IsNullOrWhiteSpace(config.DbType)) { throw new ArgumentNullException(nameof(config.DbType)); }
            if (string.IsNullOrWhiteSpace(config.DbKey)) { throw new ArgumentNullException(nameof(config.DbKey)); }
            if (string.IsNullOrWhiteSpace(config.ConnectionString)) { throw new ArgumentNullException(nameof(config.ConnectionString)); }
            if (snowflake == null) { throw new ArgumentNullException(nameof(snowflake)); }
            foreach (DbCreateHandler func in _dbCreatFuncs)
            {
                var db = func(config, snowflake);
                if (db != null)
                {
                    return db;
                }
            }
            throw new InvalidOperationException($"DbType is not supported: {config.DbType}");
        }
        public static IDbService InitializeDbService(ISnowflakeIdGenerator snowflake, List<IDbConfig> configs)
        {
            if (_dbService == null)
            {
                lock (_dbServiceLock)
                {
                    if (_dbService == null)
                    {
                        var service = new DbService(snowflake, _dbCreatFuncs, configs);
                        _dbService = service;
                    }
                }
            }
            return _dbService;
        }

        public static ICodeGenerator CreateCodeGenerator(IDatabase db)
        {
            return new CodeGenerator(db);
        }
        
    }
}
