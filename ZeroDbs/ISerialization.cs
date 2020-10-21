using System;
using System.Collections.Generic;
using System.Text;

namespace ZeroDbs
{
    public interface ISerialization
    {
        object Serialization<T>(T source) where T : class;
        System.IO.Stream SerializationStream<T>(T source) where T : class;
        T Deserialization<T>(T source) where T:class;
    }
}
