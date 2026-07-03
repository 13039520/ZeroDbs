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
        /// 模板：@nN 引用名称 @pN 引用参数
        /// <para>示例：@n0>=@p0 AND @n0<@p1</para>
        /// </summary>
        string Template { get; }
        /// <summary>
        /// 字段(集合中的名称应该被格式化处理)
        /// </summary>
        /// <param name="fields"></param>
        /// <returns></returns>
        string[]? Fields { get; }
        /// <summary>
        /// 参数(集合中的 IInValuesOptions 应该自动展开)
        /// </summary>
        /// <param name="ps"></param>
        /// <returns></returns>
        object[]? Params { get; }
        /// <summary>
        /// 设置字段(集合中的名称会被格式化处理)
        /// </summary>
        /// <param name="fields"></param>
        /// <returns></returns>
        IWherePartOptions SetFields(params string[]? fields);
        /// <summary>
        /// 设置参数(集合中的 IInValuesOptions 会自动展开)
        /// </summary>
        /// <param name="ps"></param>
        /// <returns></returns>
        IWherePartOptions SetParams(params object[]? ps);
    }
}
