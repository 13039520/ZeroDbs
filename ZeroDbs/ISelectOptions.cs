using System;

namespace ZeroDbs
{
    /// <summary>
    /// SELECT 配置
    /// </summary>
    public interface ISelectOptions<T>
    {
        string TableName {  get; }
        INameOptions Fields {  get; }
        IOrderbyOptions Orderby { get; }
        int Top {  get; }
        string Where {  get; }
        object[]? WhereParams {  get; }
        DataReaderFillEntityHandler<T> Converter {  get; }

        ISelectOptions<T> SetTableName(string tableName);
        ISelectOptions<T> SetFields(INameOptions opts);
        ISelectOptions<T> SetOrderby(IOrderbyOptions opts);
        ISelectOptions<T> SetTop(int top);
        ISelectOptions<T> SetWhere(string where, params object[]? paras);
        ISelectOptions<T> SetWhere(IWhereOptions opts);
        ISelectOptions<T> SetConverter(DataReaderFillEntityHandler<T> converter);
    }
    
}
