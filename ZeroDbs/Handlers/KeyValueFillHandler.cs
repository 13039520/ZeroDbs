using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZeroDbs
{
    /// <summary>
    /// 键值对填充委托
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="source"></param>
    /// <param name="target"></param>
    public delegate void KeyValueFillHandler<T>(T source, IKeyValueOptions target);
}
