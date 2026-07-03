using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZeroDbs.Internals
{
    internal class CodeGenerateContext : ICodeGenerateContext
    {
        public string UsingNamespaces { get; set; }
        public string ClassNamespace { get; set; }
        public bool IsPartial { get; set; }
        public bool IncludeStaticMethod { get; set; }
        public string CurrentTableModelCode { get; set; }
        public ITable CurrentTable { get; set; }
    }
}
