using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZeroDbs
{
    public interface IDbConfig
    {
        public string DbKey { get; }
        public string DbType { get; }
        public string ConnectionString { get; }
    }
}
