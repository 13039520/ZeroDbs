using System;
using System.Collections.Generic;
using System.Text;

namespace ZeroDbs.Common
{
    public class DbInfo: IDbInfo
    {
        public string Key { get; set; }
        public string ConnectionString { get; set; }
        public string Type { get; set; }
    }
}
