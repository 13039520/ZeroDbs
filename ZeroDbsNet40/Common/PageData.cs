using System;
using System.Collections.Generic;
using System.Text;

namespace ZeroDbs.Common
{
    public class PageData<T> where T :class
    {
        public long Total { get; set; }
        public List<T> Items{get;set;}
    }
}
