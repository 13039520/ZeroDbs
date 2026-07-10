using System;
using System.Collections.Generic;

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
