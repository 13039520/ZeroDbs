using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZeroDbs
{
    internal class DbConfig : IDbConfig
    {
        public string DbKey { get; set; }
        public string DbType { get; set; }
        public string ConnectionString { get; set; }
    }
}
