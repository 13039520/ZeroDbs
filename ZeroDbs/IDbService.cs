using System;
using System.Collections.Generic;

namespace ZeroDbs
{
    public interface IDbService: IEnumerable<IDatabase>
    {
        int Count { get; }
        ISnowflakeIdGenerator Snowflake { get; }
        void AddNewDb(IDbConfig config);
        IDatabase GetDb(string dbKey);
    }

}
