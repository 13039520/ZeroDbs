using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZeroDbs
{
    public interface IWherePartOptions
    {
        bool IsAnd { get; }
        /// <summary>
        /// Example: @n0>=@p0 AND @n0<@p1
        /// </summary>
        string Template { get; }
        string[] Fields { get; }
        object[] Params { get; }
        IWherePartOptions SetFields(params string[] fields);
        IWherePartOptions SetParams(params object[]? ps);
    }
}
