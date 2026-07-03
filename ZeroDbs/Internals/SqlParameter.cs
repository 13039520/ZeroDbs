using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZeroDbs
{
    internal class SqlParameter: ISqlParameter
    {
        public object? Value { get; set; }
        public System.Data.ParameterDirection Direction { get; set; } = System.Data.ParameterDirection.Input;
        public string DbDataTypeName { get; set; }=string.Empty;
    }
}
