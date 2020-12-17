using System;
using System.Collections.Generic;
using System.Text;

namespace ZeroDbs.Common
{
    public class DbConfigDataviewInfo
    {
        public string dbKey { get; set; }
        public string tableName { get; set; }
        public string entityKey { get; set; }
        public bool isStandardMapping { get; set; }
    }
}
