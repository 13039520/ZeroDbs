using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZeroDbs
{
    internal class Table : ITable
    {
        public string DbType { get; set; }
        public string DbKey { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsView { get; set; }
        public List<IColumn> Columns { get; set; }
    }
    
}
