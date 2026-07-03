using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZeroDbs
{
    /// <summary>
    /// 分页查询
    /// </summary>
    public interface IPageOptions<T>
    {
        /// <summary>
        /// 表名称
        /// </summary>
        string TableName {  get; }
        /// <summary>
        /// 查询字段
        /// </summary>
        INameOptions Fields {  get; }
        /// <summary>
        /// 排序集合
        /// </summary>
        IOrderbyOptions Orderby {  get; }
        /// <summary>
        /// WHERE 条件
        /// </summary>
        string Where {  get; }
        /// <summary>
        /// WHERE 参数
        /// </summary>
        object[]? WhereParams {  get; }
        /// <summary>
        /// 唯一字段(最好是主键)
        /// </summary>
        string UniqueField {  get; }
        /// <summary>
        /// 数据行转换器
        /// </summary>
        DataReaderFillEntityHandler<T> Converter {  get; }
        /// <summary>
        /// 页码
        /// </summary>
        int Page { get; }
        /// <summary>
        /// 页尺寸
        /// </summary>
        int Size { get; }

        IPageOptions<T> SetTableName(string tableName);
        IPageOptions<T> SetFields(INameOptions opts);
        IPageOptions<T> SetOrderby(IOrderbyOptions opts);
        IPageOptions<T> SetWhere(string where, params object[]? whereParams);
        IPageOptions<T> SetWhere(IWhereOptions opts);
        IPageOptions<T> SetConverter(DataReaderFillEntityHandler<T> convert);
        IPageOptions<T> SetUniqueField(string uniqueField);
        IPageOptions<T> SetSize(int size);
        IPageOptions<T> SetPage(int page);
    }
}
