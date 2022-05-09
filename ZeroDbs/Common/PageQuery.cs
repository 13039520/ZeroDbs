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
        private string _UniqueField = "";
        private object[] _Paras = new object[0];

        public long Page { get { return _Page; } set { _Page = value; } } 
        public long Size { get { return _Size; } set { _Size = value; } }
        public string Where { get { return _Where; } set { _Where = value; } }
        public string Orderby { get { return _Orderby; } set { _Orderby = value; } }
        public string[] Fields { get { return _Fields; } set { _Fields = value; } }
        public string UniqueField { get { return _UniqueField; } set { _UniqueField = value; } }
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
        public PageQuery UseUniqueField(string field)
        {
            _UniqueField = field;
            return this;
        }
        public PageQuery UseParas(params object[] paras)
        {
            _Paras = paras;
            return this;
        }
    }
}
