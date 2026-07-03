using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZeroDbs
{
    public interface ISqlCompiler
    {
        ISql Compile(int paramStartIndex = 0);
    }
}
