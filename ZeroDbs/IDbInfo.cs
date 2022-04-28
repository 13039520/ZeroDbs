using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZeroDbs
{
    public interface IDbInfo
    {
        string Key { get; }
        string ConnectionString { get; }
        Common.DbType Type { get; }
    }
}
