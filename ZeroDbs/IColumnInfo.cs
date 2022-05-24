using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZeroDbs
{
    public interface IColumnInfo
    {
        string Name { get; }
        string Type { get; }
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
