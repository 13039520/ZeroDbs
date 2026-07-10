using System;

namespace ZeroDbs
{
    public interface IDeleteOptions : ISqlCompiler
    {
        string TableName { get; }
        IWhereOptions? Where {  get; }
        IDeleteOptions SetTableName(string tableName);
        IDeleteOptions SetWhere(IWhereOptions? Where);
    }
}
