using System;
using System.Collections.Generic;
using System.Text;

namespace ZeroDbs.Common
{
    public class DbConfigInfo
    {
        public List<DatabaseInfo> Dbs { get; set; }
        public List<DbConfigDataviewInfo> Dvs { get; set; }
    }
}
