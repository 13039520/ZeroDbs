using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZeroDbs
{
    internal class WhereOptions : IWhereOptions
    {
        private readonly WhereCompileHandler _compileHandle;
        private readonly List<IWhereGroup> _groups = new List<IWhereGroup>();
        /// <summary>
        /// 锁(避免无变化下的重复编译)
        /// </summary>
        private readonly object _lock = new object();
        private bool _hasChanged = true;
        private ISql? _compileResult = null;

        public int Count { get { return _groups.Count; } }
        public IWhereGroup this[int index] { get { return _groups[index]; } }

        public WhereOptions(WhereCompileHandler whereCompileHandle)
        {
            if (whereCompileHandle == null) { throw new ArgumentNullException(nameof(whereCompileHandle)); }
            _compileHandle = whereCompileHandle;
        }
        public IWhereOptions And(params IWherePartOptions[]? parts)
        {
            lock (_lock)
            {
                if (parts != null && parts.Length > 0)
                {
                    _groups.Add(new WhereGroup(parts, true));
                    _hasChanged = true;
                }
            }
            return this;
        }
        public IWhereOptions Or(params IWherePartOptions[]? parts)
        {
            lock (_lock)
            {
                if (parts != null && parts.Length > 0)
                {
                    _groups.Add(new WhereGroup(parts, false));
                    _hasChanged = true;
                }
            }
            return this;
        }
        public IEnumerator<IWhereGroup> GetEnumerator()
        {
            return _groups.GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        public ISql Compile(int paramStartIndex)
        {
            if (_hasChanged)
            {
                lock (_lock)
                {
                    if (_hasChanged)
                    {
                        _compileResult = _compileHandle(this, paramStartIndex);
                        _hasChanged = false;
                    }
                }
            }
            return _compileResult;
        }
    }
}
