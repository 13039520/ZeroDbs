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
    internal class PageOptions<T>: IPageOptions<T>
    {
        public string TableName { get; set; }
        public INameOptions Fields {  get; set; }
        public IOrderbyOptions Orderby {  get; set; }
        public string Where {  get; set; }
        public object[]? WhereParams {  get; set; }
        public string UniqueField {  get; set; }
        public DataReaderFillEntityHandler<T> Converter {  get; set; }
        public int Page { get; set; }
        public int Size { get; set; }

        public IPageOptions<T> SetTableName(string name)
        {
            TableName = name;
            return this;
        }
        public IPageOptions<T> SetFields(INameOptions opts)
        {
            Fields = opts;
            return this;
        }
        public IPageOptions<T> SetOrderby(IOrderbyOptions opts)
        {
            Orderby = opts;
            return this;
        }
        public IPageOptions<T> SetWhere(string where, params object[]? whereParams)
        {
            Where = where;
            WhereParams = whereParams;
            return this;
        }
        public IPageOptions<T> SetWhere(IWhereOptions opts)
        {
            if (opts != null && opts.Count > 0)
            {
                var r = opts.Compile();
                Where = r.Text; 
                WhereParams = r.Params;
            }
            return this;
        }
        public IPageOptions<T> SetConverter(DataReaderFillEntityHandler<T> convert)
        {
            Converter = convert;
            return this;
        }
        public IPageOptions<T> SetUniqueField(string uniqueField)
        {
            UniqueField = uniqueField;
            return this;
        }
        public IPageOptions<T> SetSize(int size)
        {
            Size = size;
            return this;
        }
        public IPageOptions<T> SetPage(int page)
        {
            Page = page;
            return this;
        }
    }
}
