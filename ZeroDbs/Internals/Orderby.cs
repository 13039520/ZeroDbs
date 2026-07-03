using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZeroDbs
{
    internal class Orderby: IOrderby
    {
        public string Field { get; set; }
        public bool IsAscending { get; set; } = true;
    }
}
