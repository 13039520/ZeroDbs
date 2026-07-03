using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ZeroDbs
{
    /// <summary>
    /// WHERE 配置项
    /// </summary>
    public interface IWhereOptions : IEnumerable<IWhereGroup>, ISqlCompiler
    {
        /// <summary>
        /// IWhereGroup 数量
        /// </summary>
        int Count { get; }
        IWhereGroup this[int index] { get; }
        /// <summary>
        /// AND 连接
        /// </summary>
        /// <param name="parts"></param>
        /// <returns></returns>
        IWhereOptions And(params IWherePartOptions[]? parts);
        /// <summary>
        /// OR 连接
        /// </summary>
        /// <param name="parts"></param>
        /// <returns></returns>
        IWhereOptions Or(params IWherePartOptions[]? parts);
    }

}
