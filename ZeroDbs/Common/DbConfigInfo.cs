using System;
using System.Collections.Generic;
using System.Text;

namespace ZeroDbs.Common
{
    public class DbConfigInfo
    {
        public List<DbInfo> Dbs { get; set; }
        public List<DbTableEntityMap> Dvs { get; set; }
    }
}
