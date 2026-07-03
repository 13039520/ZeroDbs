using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZeroDbs
{
    internal class WherePartOptions : IWherePartOptions
    {
        private string[]? _Fields = null;
        private object[]? _Params = null;
        public bool IsAnd { get; }
        public string Template { get; }
        public string[]? Fields { get { return _Fields; } }
        public object[]? Params { get { return _Params; } }
        public WherePartOptions(string template, bool isAnd = true)
        {
            if (string.IsNullOrWhiteSpace(template))
            {
                throw new ArgumentNullException(nameof(template));
            }
            Template = template;
            IsAnd = isAnd;
        }
        public IWherePartOptions SetFields(params string[]? fields)
        {
            _Fields = fields;
            return this;
        }
        public IWherePartOptions SetParams(params object[]? ps)
        {
            _Params = ps;
            return this;
        }
    }
}
