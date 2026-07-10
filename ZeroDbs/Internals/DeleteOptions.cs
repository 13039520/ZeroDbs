using System;

namespace ZeroDbs
{
    internal class DeleteOptions : IDeleteOptions
    {
        readonly DeleteCompileHandler compile;
        private readonly object _lock = new object();
        private bool _hasChanged = true;
        private ISql? _compileResult = null;

        private string _TableName;
        private IWhereOptions? _Where;
        public string TableName { get { return _TableName; } }
        public IWhereOptions? Where { get { return _Where; } }
        public DeleteOptions(DeleteCompileHandler compile)
        {
            this.compile = compile;
        }
        public IDeleteOptions SetTableName(string tableName)
        {
            lock (_lock)
            {
                if (string.IsNullOrWhiteSpace(tableName)) { throw new ArgumentNullException(nameof(tableName)); }
                _TableName = tableName;
                _hasChanged = true;
            }
            return this;
        }
        public IDeleteOptions SetWhere(IWhereOptions? whereOpts)
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
