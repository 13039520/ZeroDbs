using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZeroDbs
{
    /// <summary>
    /// SQL 配置项
    /// </summary>
    public interface ISqlOptions : ISqlCompiler
    {
        /// <summary>
        /// 模板：@nN 引用名称 @pN 引用参数
        /// <para>示例：@n0>=@p0 AND @n0<@p1</para>
        /// </summary>
        public string Template { get; }
        /// <summary>
        /// 名称集合(表名、字段名、别名等)
        /// </summary>
        string[]? Names { get; }
        /// <summary>
        /// 参数(集合中的 IWhereOptions 和 IInValuesOptions 应该自动展开)
        /// </summary>
        object[]? Params { get; }
        /// <summary>
        /// 设置参数(集合中的 IWhereOptions 和 IInValuesOptions 会自动展开)
        /// </summary>
        /// <param name="ps"></param>
        /// <returns></returns>
        ISqlOptions SetParams(params object[]? ps);
        /// <summary>
        /// 设置名称(表名、字段名、别名等)
        /// </summary>
        /// <param name="names"></param>
        /// <returns></returns>
        ISqlOptions SetNames(params string[]? names);
    }
}
