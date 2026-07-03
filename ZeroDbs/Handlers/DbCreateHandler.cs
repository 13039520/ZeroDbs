using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZeroDbs
{
    public delegate IDatabase? DbCreateHandler(IDbConfig config, ISnowflakeIdGenerator snowflake);
}
