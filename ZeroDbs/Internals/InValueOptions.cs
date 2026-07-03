using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZeroDbs
{
    internal class InValueOptions : IInValueOptions
    {
        private InValueCompileHandler _inCompileHandle;
        private object[] _values;
        public int Count { get { return _values.Length; } }
        public InValueOptions(InValueCompileHandler inCompileHandle, object[] values)
        {
            if (inCompileHandle == null) { throw new ArgumentNullException(nameof(inCompileHandle)); }
            if (values == null || values.Length < 1) { throw new ArgumentNullException(nameof(values)); }
            _inCompileHandle = inCompileHandle;
            _values = values;
        }
        public object this[int index]
        {
            get { return _values[index]; }
        }
        public IEnumerator<object> GetEnumerator()
        {
            return ((IEnumerable<object>)_values).GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
