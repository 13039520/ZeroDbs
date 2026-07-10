using System;

namespace ZeroDbs
{
    public interface ISqlCompiler
    {
        ISql Compile(int paramStartIndex = 0);
    }
}
