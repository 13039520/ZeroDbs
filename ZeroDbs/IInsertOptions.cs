using System;

namespace ZeroDbs
{
    public interface IInsertOptions: ISqlCompiler
    {
        string TableName { get; }
        IKeyValueOptions KeyValuePairs { get; }
        INameOptions? IgnoreFields {  get; }
        string? ReturnIdentityColumn {  get; }
        IInsertOptions SetTableName(string tableName);
        IInsertOptions SetKeyValuePairs(IKeyValueOptions kvs);
        IInsertOptions SetIgnoreFields(INameOptions? names);
        IInsertOptions SetReturnIdentityColumn(string? identityColumnName);
    }
}
