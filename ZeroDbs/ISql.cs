using System;

namespace ZeroDbs
{
    public interface ISql
    {
        string Text { get; }
        object[]? Params { get; }
    }
}
