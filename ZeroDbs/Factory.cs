using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZeroDbs
{
    /// <summary>
    /// 静态工厂
    /// </summary>
    public static class Factory
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
        /// <summary>
        /// 添加创建数据库对象的委托
        /// </summary>
        /// <param name="createHandler"></param>
        public static void AddDbCreator(DbCreateHandler createHandler)
        {
            if (createHandler == null) { throw new ArgumentNullException(nameof(createHandler)); }
            _dbCreatFuncs.Add(createHandler);
        }
        /// <summary>
        /// 创建数据库配置
        /// </summary>
        /// <param name="dbKey"></param>
        /// <param name="dbType"></param>
        /// <param name="dbConnectionString"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static IDbConfig CreateDbConfig(string dbKey, string dbType, string dbConnectionString)
        {
            if (string.IsNullOrWhiteSpace(dbKey)) { throw new ArgumentNullException(nameof(dbKey)); }
            if (string.IsNullOrWhiteSpace(dbType)) { throw new ArgumentNullException(nameof(dbType)); }
            if (string.IsNullOrWhiteSpace(dbConnectionString)) { throw new ArgumentNullException(nameof(dbConnectionString)); }
            return new DbConfig { DbKey = dbKey, DbType = dbType, ConnectionString = dbConnectionString };
        }
        /// <summary>
        /// 创建雪花Id生成器
        /// </summary>
        /// <param name="snowflakeDataCenterId"></param>
        /// <param name="snowflakeWorkerId"></param>
        /// <returns></returns>
        public static ISnowflakeIdGenerator CreateSnowflakeIdGenerator(int snowflakeDataCenterId, int snowflakeWorkerId)
        {
            return new SnowflakeIdGenerator(snowflakeWorkerId, snowflakeDataCenterId);
        }
        /// <summary>
        /// 创建数据库实例(测试或单数据库场景)
        /// </summary>
        /// <param name="config"></param>
        /// <param name="snowflake"></param>
        /// <returns></returns>
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
        /// <summary>
        /// 初始化IDbService对象(只执行一次)
        /// </summary>
        /// <param name="snowflake">雪花Id生成器</param>
        /// <param name="configs">数据库配置</param>
        /// <exception cref="InvalidOperationException"></exception>
        public static IDbService DbServiceInit(ISnowflakeIdGenerator snowflake, List<IDbConfig> configs)
        {
            if (_dbService == null)
            {
                lock (_dbServiceLock)
                {
                    if (_dbService == null)
                    {
                        var service = new DbService(snowflake, _dbCreatFuncs, configs);//IDbService 只有一个 GetDb(string dbKey) 的方法
                        _dbService = service;
                    }
                }
            }
            return _dbService;
        }

        public static ICodeGenerator CodeGenerator(IDatabase db)
        {
            return new CodeGenerator(db);
        }
        
    }
}
