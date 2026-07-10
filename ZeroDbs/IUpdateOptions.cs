using System;

namespace ZeroDbs
{
    public interface IUpdateOptions : ISqlCompiler
    {
        string TableName { get; }
        IKeyValueOptions KeyValuePairs { get; }
        IWhereOptions? Where {  get; }
        IUpdateOptions SetTableName(string tableName);
        IUpdateOptions SetKeyValuePairs(IKeyValueOptions kvs);
        IUpdateOptions SetWhere(IWhereOptions? Where);
    }
}
