using System;

namespace ZeroDbs
{
    /// <summary>
    /// SQL 配置项
    /// </summary>
    public interface ISqlOptions : ISqlCompiler
    {
        /// <summary>
        /// Template: @n0>=@p0 AND @n0<@p1</para>
        /// </summary>
        public string Template { get; }
        string[]? Names { get; }
        object[]? Params { get; }
        ISqlOptions SetParams(params object[]? ps);
        ISqlOptions SetNames(params string[]? names);
    }
}
