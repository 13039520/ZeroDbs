using System;

namespace ZeroDbs
{
    /// <summary>
    /// 专用SQL参数包装器
    /// </summary>
    public interface ISqlParameter
    {
        object? Value { get; }
        System.Data.ParameterDirection Direction { get; }
        string DbDataTypeName { get; }
    }
}
