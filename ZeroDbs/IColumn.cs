using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZeroDbs
{
    public interface IColumn
    {
        string Name { get; }
        Type DataType {  get; }
        string DbDataTypeName {  get; }
        bool IsNullable { get; }
        bool IsPrimaryKey { get; }
        bool IsIdentity { get; }
        long Byte { get; }
        long MaxLength { get; }
        int DecimalDigits { get; }
        string DefaultValue { get; }
        string Description { get; }
    }
}
