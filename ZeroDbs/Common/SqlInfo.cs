using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZeroDbs.Common
{
    public  class SqlInfo
    {
        private Dictionary<string, object> _Paras;
        public string Sql { get; set; }
        public Dictionary<string, object> Paras { get { return _Paras; } }

        public SqlInfo() {
            _Paras = new Dictionary<string, object>();
        }
        public SqlInfo(int parasCapacity) {
            _Paras = new Dictionary<string, object>(parasCapacity);
        }

    }
}
