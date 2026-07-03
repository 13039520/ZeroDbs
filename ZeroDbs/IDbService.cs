using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
