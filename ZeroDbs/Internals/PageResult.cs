using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZeroDbs
{
    internal class PageResult<T>:IPageResult<T>
    {
        public long Total {  get; set; }
        public int Page {  get; set; }
        public int Size {  get; set; }
        public List<T> Rows { get; set; }
    }
}
