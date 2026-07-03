using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZeroDbs
{
    internal class WhereGroup : IWhereGroup
    {
        private readonly IWherePartOptions[] _parts;
        private readonly bool _isAnd;
        public int Count { get { return _parts.Length; } }
        public bool IsAnd { get { return _isAnd; } }
        public IWherePartOptions this[int index]
        {
            get { return _parts[index]; }
        }
        public WhereGroup(IWherePartOptions[] parts, bool isAnd)
        {
            if (parts == null || parts.Length < 1) { throw new ArgumentNullException(nameof(parts)); }
            this._parts = parts;
            this._isAnd = isAnd;
        }
        public IEnumerator<IWherePartOptions> GetEnumerator()
        {
            return ((IEnumerable<IWherePartOptions>)_parts).GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
