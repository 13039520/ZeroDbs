using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZeroDbs
{
    public interface ITableInfo: ICloneable
    {
        string DbName { get; }
        string Name { get; }
        string Description { get; }
        bool IsView { get; }
        List<IColumnInfo> Colunms { get; }
    }
}
