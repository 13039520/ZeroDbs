using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZeroDbs
{
    internal class Sql : ISql
    {
        public string Text { get; set; } = string.Empty;
        public object[]? Params { set; get; }
    }
}
