using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZeroDbs
{
    public interface ITable
    {
        string DbType { get; }
        string DbKey { get; }
        string Name { get; }
        string Description { get; }
        bool IsView { get; }
        List<IColumn> Columns { get; }
    }
    
}
