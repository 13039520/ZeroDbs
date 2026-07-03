using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZeroDbs
{
    internal class Column: IColumn
    {
        public string Name { get; set; }
        public Type DataType {  get; set; }
        public string DbDataTypeName {  get; set; }
        public bool IsNullable { get; set; }
        public bool IsPrimaryKey { get; set; }
        public bool IsIdentity { get; set; }
        public long Byte { get; set; }
        public long MaxLength { get; set; }
        public int DecimalDigits { get; set; }
        public string DefaultValue { get; set; }
        public string Description { get; set; }
    }
}
