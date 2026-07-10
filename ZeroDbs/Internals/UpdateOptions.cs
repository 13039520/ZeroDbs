using System;

namespace ZeroDbs
{
    internal class UpdateOptions : IUpdateOptions
    {
        readonly UpdateCompileHandler compile;
        private readonly object _lock = new object();
        private bool _hasChanged = true;
        private ISql? _compileResult = null;

        private string _TableName;
        private IKeyValueOptions _KeyValuePairs;
        private IWhereOptions? _Where;
        public string TableName { get { return _TableName; } }
        public IKeyValueOptions KeyValuePairs { get { return _KeyValuePairs; } }
        public IWhereOptions? Where { get { return _Where; } }
        public UpdateOptions(UpdateCompileHandler compile)
        {
            this.compile = compile;
        }
        public IUpdateOptions SetTableName(string tableName)
        {
            lock (_lock)
            {
                if (string.IsNullOrWhiteSpace(tableName)) { throw new ArgumentNullException(nameof(tableName)); }
                _TableName = tableName;
                _hasChanged = true;
            }
            return this;
        }
        public IUpdateOptions SetKeyValuePairs(IKeyValueOptions keyValuePairs)
        {
            lock (_lock)
            {
                if (keyValuePairs == null || keyValuePairs.Count < 1) { throw new ArgumentNullException(nameof(keyValuePairs)); }
                _KeyValuePairs = keyValuePairs;
                _hasChanged = true;
            }
            return this;
        }
        public IUpdateOptions SetWhere(IWhereOptions? whereOpts)
        {
            lock (_lock)
            {
                _Where = whereOpts;
                _hasChanged = true;
            }
            return this;
        }
        public ISql Compile(int startIndex = 0)
        {
            if (_hasChanged)
            {
                lock (_lock)
                {
                    if (_hasChanged)
                    {
                        _compileResult = compile(this, startIndex);
                        _hasChanged = false;
                    }
                }
            }
            return _compileResult;
        }
    }
}
