using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZeroDbs
{
    internal class SqlOptions : ISqlOptions
    {
        private readonly SqlCompileHandler _sqlCompileHandle;
        private readonly string _template = string.Empty;
        private object[]? _params = null;
        private string[]? _names = null;
        public string Template { get { return _template; } }
        public string[]? Names { get { return _names; } }
        public object[]? Params { get { return _params; } }
        public SqlOptions(SqlCompileHandler sqlCompileHandle, string template)
        {
            if (sqlCompileHandle == null) { throw new ArgumentNullException(nameof(sqlCompileHandle)); }
            if (string.IsNullOrWhiteSpace(template)) { throw new ArgumentNullException(nameof(template)); }
            _template = template;
            _sqlCompileHandle = sqlCompileHandle;
        }
        public ISqlOptions SetParams(params object[]? sqlParams)
        {
            if (sqlParams == null || sqlParams.Length < 1)
            {
                throw new ArgumentNullException(nameof(sqlParams));
            }
            _params = sqlParams;
            return this;
        }
        public ISqlOptions SetNames(params string[]? names)
        {
            if (names == null || names.Length < 1)
            {
                throw new ArgumentNullException(nameof(names));
            }
            _names = names;
            return this;
        }
        public ISql Compile(int pIndex)
        {
            pIndex = 0;
            return _sqlCompileHandle(this, pIndex);
        }
    }
}
