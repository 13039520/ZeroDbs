using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZeroDbs.Common
{
    public class ListQuery
    {
        private int _Top = 0;
        private string _Where = "";
        private string _Orderby = "";
        private string[] _Fields = new string[0];
        private object[] _Paras = new object[0];

        public int Top { get { return _Top; } set { _Top = value; } }
        public string Where { get { return _Where; } set { _Where = value; } }
        public string Orderby { get { return _Orderby; } set { _Orderby = value; } }
        public string[] Fields { get { return _Fields; } set { _Fields = value; } }
        public object[] Paras { get { return _Paras; } set { _Paras = value; } }

        public ListQuery UseWhere(string where)
        {
            this._Where = where;
            return this;
        }
        public ListQuery UseTop(int top)
        {
            this._Top = top;
            return this;
        }
        public ListQuery UseFields(params string[] fields)
        {
            this._Fields = fields;
            return this;
        }
        public ListQuery UseOrderby(string orderby)
        {
            this._Orderby = orderby;
            return this;
        }
        public ListQuery UseParas(params object[] paras)
        {
            this._Paras = paras;
            return this;
        }

        public static ListQuery Builder()
        {
            return new ListQuery();
        }

        public ListQuery() : this("")
        {

        }
        public ListQuery(string where) : this(where, "")
        {

        }
        public ListQuery(string where, string orderby) : this(where, orderby, 0)
        {

        }
        public ListQuery(string where, string orderby, int top) : this(where, orderby, top, new string[0])
        {

        }
        public ListQuery(string where, string orderby, int top, string[] fields) : this(where, orderby, top, fields, new object[0])
        {

        }
        public ListQuery(string where, string orderby, int top, string[] fields, params object[] paras)
        {
            this._Where = where;
            this._Orderby = orderby;
            this._Top = top;
            this._Fields = fields;
            this.Paras = paras;
        }

    }
}
