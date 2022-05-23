using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZeroDbs.Common
{
    public class PageQuery
    {
        private long _Page = 1;
        private long _Size = 1;
        private string _Where = "";
        private string _Orderby = "";
        private string[] _Fields = new string[0];
        private string _Unique = "";
        private object[] _Paras = new object[0];

        public long Page { get { return _Page; } set { _Page = value; } } 
        public long Size { get { return _Size; } set { _Size = value; } }
        public string Where { get { return _Where; } set { _Where = value; } }
        public string Orderby { get { return _Orderby; } set { _Orderby = value; } }
        public string[] Fields { get { return _Fields; } set { _Fields = value; } }
        public string Unique { get { return _Unique; } set { _Unique = value; } }
        public object[] Paras { get { return _Paras; } set { _Paras = value; } }

        public PageQuery UsePage(long page)
        {
            if (page > 0)
            {
                _Page = page;
            }
            return this;
        }
        public PageQuery UseSize(long size)
        {
            if (size > 0)
            {
                _Size = Size;
            }
            return this;
        }
        public PageQuery UseWhere(string where)
        {
            _Where = where;
            return this;
        }
        public PageQuery UseOrderby(string orderby)
        {
            _Orderby = orderby;
            return this;
        }
        public PageQuery UseFields(params string[] fields)
        {
            _Fields = fields;
            return this;
        }
        public PageQuery UseUnique(string field)
        {
            _Unique = field;
            return this;
        }
        public PageQuery UseParas(params object[] paras)
        {
            _Paras = paras;
            return this;
        }

        public PageQuery() : this(1, 1)
        {

        }
        public PageQuery(long page, long size) : this(page, size, "")
        {

        }
        public PageQuery(long page, long size, string where) : this(page, size, where, "")
        {

        }
        public PageQuery(long page, long size, string where, string orderby) : this(page, size, where, orderby, new string[0])
        {

        }
        public PageQuery(long page, long size, string where, string orderby, string[] fields) : this(page, size, where, orderby, fields, "")
        {

        }
        public PageQuery(long page, long size, string where, string orderby, string[] fields, string unique = "") : this(page, size, where, orderby, fields, unique, new object[0])
        {

        }
        public PageQuery(long page, long size, string where, string orderby, string[] fields, string unique = "", params object[] paras)
        {
            this._Page = page;
            this._Size = size;
            this._Where = where;
            this._Orderby = orderby;
            this._Fields = fields;
            this._Unique = unique;
            this._Paras = paras;
        }



    }
}
