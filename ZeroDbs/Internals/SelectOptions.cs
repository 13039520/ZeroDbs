using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZeroDbs
{
    internal class SelectOptions<T> : ISelectOptions<T>
    {
        public string TableName { get; set; }
        public INameOptions Fields { get; set; }
        public IOrderbyOptions Orderby { get; set; }
        public int Top { get; set; }
        public string Where { get; set; }
        public object[]? WhereParams { get; set; }
        public DataReaderFillEntityHandler<T> Converter { get; set; }

        public ISelectOptions<T> SetTableName(string tableName)
        {
            this.TableName = tableName;
            return this;
        }
        public ISelectOptions<T> SetFields(INameOptions opts)
        {
            this.Fields = opts;
            return this;
        }
        public ISelectOptions<T> SetOrderby(IOrderbyOptions opts)
        {
            this.Orderby = opts;
            return this;
        }
        public ISelectOptions<T> SetTop(int top)
        {
            this.Top = top;
            return this;
        }
        public ISelectOptions<T> SetWhere(string where, params object[]? whereParams)
        {
            this.Where = where;
            this.WhereParams = whereParams;
            return this;
        }
        public ISelectOptions<T> SetWhere(IWhereOptions opts)
        {
            var sql = opts.Compile(0);
            this.Where = sql.Text;
            this.WhereParams = sql.Params;
            return this;
        }
        public ISelectOptions<T> SetConverter(DataReaderFillEntityHandler<T> converter)
        {
            this.Converter = converter;
            return this;
        }
    }
}
