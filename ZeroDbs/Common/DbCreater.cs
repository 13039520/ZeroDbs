using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZeroDbs.Common
{
    public delegate IDb DbCreateHandler(IDbInfo dbInfo);
    public class DbCreater
    {
        public string DbType { get; set; }
        public DbCreateHandler Create { get; set; }
    }
}
