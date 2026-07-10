using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZeroDbs
{
    public delegate ISql InsertCompileHandler(IInsertOptions options, int startIndex);
}
