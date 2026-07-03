using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZeroDbs
{
    internal class ExecuteResult: IExecuteResult
    {
        public IReadOnlyList<DataTable> Tables { get; set; }

        public int RecordsAffected { get; set; }
    }
}
