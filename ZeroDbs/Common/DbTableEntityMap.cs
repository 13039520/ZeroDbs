using System;
using System.Collections.Generic;
using System.Text;

namespace ZeroDbs.Common
{
    public class DbTableEntityMap
    {
        public string DbKey { get; set; }
        public string TableName { get; set; }
        public string EntityKey { get; set; }
        public bool IsStandardMapping { get; set; }
    }
}
