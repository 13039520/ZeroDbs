using System;

namespace ZeroDbs
{
    /// <summary>
    /// 原始 SQL 配置项
    /// </summary>
    public interface IRawSqlOptions : ISql
    {
        System.Data.CommandType CmdType { get; }
        IRawSqlOptions SetCmdText(string cmdText, params object[]? cmdParams);
        IRawSqlOptions SetCmdText(ISql sql);
        IRawSqlOptions SetCmdType(System.Data.CommandType cmdType);
    }
}
