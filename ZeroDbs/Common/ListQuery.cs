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
    }
}
