using System;

namespace ZeroDbs
{
    public interface IDbConfig
    {
        public string DbKey { get; }
        public string DbType { get; }
        public string ConnectionString { get; }
    }
}
