using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZeroDbs
{
    /// <summary>
    /// IN 集合项(用于快速识别类型的专用列表包装)
    /// </summary>
    public interface IInValueOptions : IEnumerable<object>
    {
        /// <summary>
        /// 元素个数
        /// </summary>
        int Count { get; }
        object this[int index] { get; }
    }
}
