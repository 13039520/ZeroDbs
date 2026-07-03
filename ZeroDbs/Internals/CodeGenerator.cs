using ZeroDbs.Internals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZeroDbs
{
    internal class CodeGenerator : ICodeGenerator
    {
        private bool isGenerating = false;
        private IDatabase db;
        private string usingNamespaces = "using System;";
        private string classNamespace = string.Empty;
        private bool isPartial = false;
        private bool isStaticMethod = true;
        private HashSet<string> blackTableNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        private HashSet<string> whiteTableNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        public CodeGenerator(IDatabase db)
        {
            if (db == null) { throw new ArgumentNullException(nameof(db)); }
            this.db = db;
        }
        public ICodeGenerator SetDb(IDatabase db)
        {
            if (isGenerating) { return this; }
            if (db == null) { throw new ArgumentNullException(nameof(db)); }
            this.db = db;
            return this;
        }
        public ICodeGenerator SetUsingNamespaces(params string[]? strings)
        {
            if (isGenerating) return this;
            if (strings == null || strings.Length == 0) return this;

            StringBuilder s = new StringBuilder();
            HashSet<string> hs = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            foreach (var str in strings)
            {
                if (string.IsNullOrWhiteSpace(str)) { continue; }

                string t = str.Trim();
                if (!hs.Add(t)) { continue; }

                s.Append("using ");
                s.Append(t);
                s.AppendLine(";");
            }

            if (s.Length > 0)
            {
                usingNamespaces = s.ToString().Trim();
            }

            return this;
        }
        public ICodeGenerator SetClassNamespace(string classNamespace)
        {
            if (isGenerating) { return this; }
            if (string.IsNullOrWhiteSpace(classNamespace))
            {
                throw new ArgumentNullException(nameof(classNamespace));
            }
            this.classNamespace = classNamespace;
            return this;
        }
        public ICodeGenerator SetPartial(bool isPartial)
        {
            if (isGenerating) { return this; }
            this.isPartial = isPartial;
            return this;
        }
        public ICodeGenerator SetStaticMethod(bool useStaticMethod)
        {
            if (isGenerating) { return this; }
            this.isStaticMethod = useStaticMethod;
            return this;
        }
        public ICodeGenerator SetTableBlackList(params string[]? tabelNames)
        {
            if (isGenerating) { return this; }
            blackTableNames.Clear();
            if (tabelNames != null && tabelNames.Length > 0)
            {
                for (int i = 0; i < tabelNames.Length; i++)
                {
                    if (!string.IsNullOrWhiteSpace(tabelNames[i]))
                    {
                        blackTableNames.Add(tabelNames[i]);
                    }
                }
            }
            return this;
        }
        public ICodeGenerator SetTableWhiteList(params string[]? tabelNames)
        {
            if (isGenerating) { return this; }
            whiteTableNames.Clear();
            if (tabelNames != null && tabelNames.Length > 0)
            {
                for (int i = 0; i < tabelNames.Length; i++)
                {
                    if (!string.IsNullOrWhiteSpace(tabelNames[i]))
                    {
                        whiteTableNames.Add(tabelNames[i]);
                    }
                }
            }
            return this;
        }
        public void Start(CodeGenerateTableHandler tableCallback, CodeGenerateDoneHandler doneCallback)
        {
            if (tableCallback == null) { throw new ArgumentNullException(nameof(tableCallback)); }
            if (doneCallback == null) { throw new ArgumentNullException(nameof(doneCallback)); }
            if (string.IsNullOrWhiteSpace(classNamespace)) { throw new ArgumentNullException(nameof(classNamespace)); }
            if (isGenerating) { return; }
            isGenerating = true;
            try
            {
                Generate(tableCallback, doneCallback);
            }
            finally
            {
                isGenerating = false;
            }
        }
        readonly string indent = "    ";
        private void WithIndent(StringBuilder s, int count)
        {
            for (int i = 0; i < count; i++)
            {
                s.Append(indent);
            }
        }
        private bool ShouldIgnoreTable(string tableName)
        {
            if (whiteTableNames.Count > 0)
            {
                return !whiteTableNames.Contains(tableName);
            }
            return blackTableNames.Contains(tableName);
        }
        private void Generate(CodeGenerateTableHandler tableCallback, CodeGenerateDoneHandler doneCallback)
        {
            
            var tables = db.GetTables();
            //创建上下文
            CodeGenerateContext context = new CodeGenerateContext
            {
                ClassNamespace = classNamespace,
                CurrentTableModelCode = "",
                CurrentTable = null,
                IncludeStaticMethod = isStaticMethod,
                IsPartial = isPartial,
                UsingNamespaces = usingNamespaces
            };

            StringBuilder s = new StringBuilder();
            int count = 0;
            foreach (var table in tables)
            {
                if (ShouldIgnoreTable(table.Name)) { continue; }
                s.Clear();
                s.AppendLine(usingNamespaces);
                s.AppendLine();
                //namespace BEGIN
                s.Append("namespace ");
                s.AppendLine(classNamespace);
                s.AppendLine("{");
                //class BEGIN
                WithIndent(s,  1);
                s.AppendLine("/// <summary>");
                WithIndent(s, 1);
                s.Append("/// ");
                s.AppendLine(table.Description);
                WithIndent(s, 1);
                s.AppendLine("/// </summary>");
                WithIndent(s, 1);
                s.Append("public ");
                if (isPartial) { s.Append("partial "); }
                s.Append("class ");
                s.AppendLine(table.Name);
                WithIndent(s, 1);
                s.AppendLine("{");
                //columns BEGIN
                foreach (var column in table.Columns)
                {
                    WithIndent(s, 2);
                    s.AppendLine("/// <summary>");
                    WithIndent(s, 2);
                    s.Append("/// ");
                    s.AppendLine(column.Description);
                    WithIndent(s, 2);
                    s.AppendLine("/// </summary>");
                    WithIndent(s, 2);
                    s.Append("public ");
                    s.Append(TypeAliasMap.GetAlias(column.DataType));
                    s.Append(" ");
                    s.Append(column.Name);
                    s.AppendLine("{ get; set; }");
                    s.AppendLine();
                }
                //columns END
                if (isStaticMethod)
                {
                    //StaticMethod 1 BEGIN
                    WithIndent(s, 2);
                    s.Append("public static ");
                    s.Append(table.Name);
                    s.AppendLine(" FromDataReader(IDataReaderWrapper r)");
                    WithIndent(s, 2);
                    s.AppendLine("{");
                    //new Object BEGIN
                    WithIndent(s, 3);
                    s.Append("return new ");
                    s.Append(table.Name);
                    s.AppendLine("{");
                    int n = 0;
                    foreach (var column in table.Columns)
                    {
                        n++;
                        WithIndent(s, 4);
                        s.Append(column.Name);
                        s.Append(" = r.GetValue<");
                        s.Append(TypeAliasMap.GetAlias(column.DataType));
                        s.Append(">(\"");
                        s.Append(column.Name);
                        s.Append("\")");
                        if (n < table.Columns.Count)
                        {
                            s.Append(",");
                        }
                        s.AppendLine();
                    }
                    WithIndent(s, 3);
                    s.AppendLine("};");//new Object END

                    WithIndent(s, 2);
                    s.AppendLine("}");//StaticMethod 1 END

                    s.AppendLine();
                    //StaticMethod 2 BEGIN
                    WithIndent(s, 2);
                    s.Append("public static void FillKeyValueOptions(");
                    s.Append(table.Name);
                    s.AppendLine(" m, IKeyValueOptions kvs)");
                    WithIndent(s, 2);
                    s.AppendLine("{");
                    //fill
                    n = 0;
                    foreach (var column in table.Columns)
                    {
                        n++;
                        WithIndent(s, 3);
                        s.Append("kvs[\"");
                        s.Append(column.Name);
                        s.Append("\"] = m.");
                        s.Append(column.Name);
                        s.AppendLine(";");
                    }
                    WithIndent(s, 2);
                    s.AppendLine("}");//StaticMethod 2 END
                }
                s.AppendLine();
                WithIndent(s, 1);
                s.AppendLine("}");//class END
                s.Append("}");//namespace END
                try
                {
                    context.CurrentTable = table;
                    context.CurrentTableModelCode = s.ToString();
                    tableCallback(context);
                }
                catch { }
                count++;
            }
            doneCallback(count);
        }
    }
}
