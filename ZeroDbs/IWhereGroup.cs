using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZeroDbs
{
    public interface IWhereGroup : IEnumerable<IWherePartOptions>
    {
        /// <summary>
        /// IWherePart 数量
        /// </summary>
        int Count { get; }
        /// <summary>
        /// 与连接(true AND false OR)
        /// </summary>
        bool IsAnd { get; }
        IWherePartOptions this[int index] { get; }
    }
}
