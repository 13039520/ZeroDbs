using System;

namespace ZeroDbs
{
    internal class InsertOptions : IInsertOptions
    {
        readonly InsertCompileHandler compile;
        private readonly object _lock = new object();
        private bool _hasChanged = true;
        private ISql? _compileResult = null;

        private string _TableName;
        private IKeyValueOptions _KeyValuePairs;
        private INameOptions? _IgnoreFields;
        private string? _ReturnIdentityColumn;
        public string TableName { get { return _TableName; } }
        public IKeyValueOptions KeyValuePairs { get { return _KeyValuePairs; } }
        public INameOptions? IgnoreFields { get { return _IgnoreFields; } }
        public string? ReturnIdentityColumn { get { return _ReturnIdentityColumn; } }
        public InsertOptions(InsertCompileHandler compile)
        {
            this.compile = compile;
        }
        public IInsertOptions SetTableName(string tableName)
        {
            lock (_lock)
            {
                if (string.IsNullOrWhiteSpace(tableName)) { throw new ArgumentNullException(nameof(tableName)); }
                _TableName = tableName;
                _hasChanged = true;
            }
            return this;
        }
        public IInsertOptions SetKeyValuePairs(IKeyValueOptions keyValuePairs)
        {
            lock (_lock)
            {
                if (keyValuePairs == null || keyValuePairs.Count < 1) { throw new ArgumentNullException(nameof(keyValuePairs)); }
                _KeyValuePairs = keyValuePairs;
                _hasChanged = true;
            }
            return this;
        }
        public IInsertOptions SetIgnoreFields(INameOptions? names)
        {
            lock (_lock)
            {
                _IgnoreFields = names;
                _hasChanged = true;
            }
            return this;
        }
        public IInsertOptions SetReturnIdentityColumn(string? identityColumnName)
        {
            lock (_lock)
            {
                _ReturnIdentityColumn = identityColumnName;
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
