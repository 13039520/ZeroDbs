using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Reflection;
using System.CodeDom;
using System.CodeDom.Compiler;

namespace ZeroDbs.Tools
{
    /// <summary>
    /// 实体类生成器
    /// </summary>
    public class EntityCodeDom
    {
        List<string> pNames = new List<string>();
        List<CodeTypeMember> members = new List<CodeTypeMember>();
        List<CodeTypeMember> properties = new List<CodeTypeMember>();
        readonly bool isPartial = false;
        readonly CodeNamespaceImport[] codeNamespaceImports = new CodeNamespaceImport[0];
        readonly CodeAttributeDeclaration[] classCodeAttributeDeclarations;
        readonly string classComments = string.Empty;
        readonly string nameSpace = string.Empty;
        readonly string className = string.Empty;
        private string classFullName = string.Empty;
        public string ClassFullName
        {
            get { return classFullName; }
        }

        /// <summary>
        /// 实体类生成器
        /// </summary>
        /// <param name="nameSpace">命名空间</param>
        /// <param name="className">类名</param>
        /// <param name="isPartial">分部类标识</param>
        /// <param name="comments">文档型注释</param>
        /// <param name="namespaceImports">命名空间引用导入集合</param>
        /// <param name="codeAttributeDeclarations">特性集合</param>
        public EntityCodeDom(string nameSpace, string className, bool isPartial, string comments, string[] namespaceImports = null, CodeAttributeDeclaration[] codeAttributeDeclarations = null)
        {
            this.nameSpace = string.IsNullOrEmpty(nameSpace) ? "MyNameSpace" : nameSpace;
            this.className = string.IsNullOrEmpty(className) ? "MyClassName" : className;
            this.isPartial = isPartial;
            this.classFullName = string.Format("{0}.{1}", nameSpace, className);
            if (namespaceImports != null && namespaceImports.Length > 0)
            {
                List<string> s = new List<string>();
                foreach (string ns in namespaceImports)
                {
                    if (string.IsNullOrEmpty(ns)) { continue; }
                    if (s.Find(o => o.Equals(ns, StringComparison.OrdinalIgnoreCase)) != null) { continue; }
                    s.Add(ns);
                }
                namespaceImports = s.ToArray();
                if (namespaceImports.Length < 1)
                {
                    namespaceImports = new string[] { "System" };
                }
            }
            else
            {
                namespaceImports = new string[] { "System" };
            }
            this.classCodeAttributeDeclarations = codeAttributeDeclarations;
            this.codeNamespaceImports = new CodeNamespaceImport[namespaceImports.Length];
            for (int i = 0; i < namespaceImports.Length; i++)
            {
                this.codeNamespaceImports[i] = new CodeNamespaceImport(namespaceImports[i]);
            }
            this.classComments = comments;
        }

        private CodeCompileUnit GetCodeCompileUnit()
        {
            return new CodeCompileUnit();
        }
        private void SetNamespaces(CodeCompileUnit cu, out CodeNamespace codeNamespace)
        {
            codeNamespace = new CodeNamespace(this.nameSpace);
            foreach (var ns in codeNamespaceImports)
            {
                codeNamespace.Imports.Add(ns);
            }
            cu.Namespaces.Add(codeNamespace);
        }
        private void SetCodeTypeDeclaration(CodeNamespace cn, out CodeTypeDeclaration typeClass)
        {
            typeClass = new CodeTypeDeclaration(this.className);
            typeClass.IsClass = true;
            typeClass.TypeAttributes = TypeAttributes.Public;
            typeClass.IsPartial = this.isPartial;
            List<CodeCommentStatement> codeComments = CreateSummaryCodeCommentStatements(classComments);
            if (codeComments.Count > 0)
            {
                foreach (CodeCommentStatement codeComment in codeComments)
                {
                    typeClass.Comments.Add(codeComment);
                }
            }

            if (classCodeAttributeDeclarations != null && classCodeAttributeDeclarations.Length > 0)
            {
                foreach (var dec in classCodeAttributeDeclarations)
                {
                    typeClass.CustomAttributes.Add(dec);
                }
            }
            cn.Types.Add(typeClass);
        }
        private void SetCodeMembers(CodeTypeDeclaration typeClass)
        {
            for (var i = 0; i < members.Count; i++)
            {
                var member = members[i];
                if (i < 1)
                {
                    member.StartDirectives.Clear();
                    member.StartDirectives.Add(new CodeRegionDirective(CodeRegionMode.Start, "-- Members --"));
                    member.EndDirectives.Clear();
                    member.EndDirectives.Add(new CodeRegionDirective(CodeRegionMode.None, ""));
                }
                if (i == members.Count - 1)
                {
                    member.StartDirectives.Clear();
                    member.StartDirectives.Add(new CodeRegionDirective(CodeRegionMode.None, ""));
                    member.EndDirectives.Clear();
                    member.EndDirectives.Add(new CodeRegionDirective(CodeRegionMode.End, ""));
                }
                typeClass.Members.Add(member);
            }
            for (var i = 0; i < properties.Count; i++)
            {
                var member = properties[i];
                if (i < 1)
                {
                    member.StartDirectives.Clear();
                    member.StartDirectives.Add(new CodeRegionDirective(CodeRegionMode.Start, "-- Properties --"));
                    member.EndDirectives.Clear();
                    member.EndDirectives.Add(new CodeRegionDirective(CodeRegionMode.None, ""));
                }
                if (i == members.Count - 1)
                {
                    member.StartDirectives.Clear();
                    member.StartDirectives.Add(new CodeRegionDirective(CodeRegionMode.None, ""));
                    member.EndDirectives.Clear();
                    member.EndDirectives.Add(new CodeRegionDirective(CodeRegionMode.End, ""));
                }
                typeClass.Members.Add(member);
            }

        }
        private List<CodeCommentStatement> CreateSummaryCodeCommentStatements(string comments)
        {
            List<CodeCommentStatement> statements = new List<CodeCommentStatement>();
            if (!string.IsNullOrEmpty(comments))
            {
                statements.Add(new CodeCommentStatement("<summary>", true));
                string[] strings = comments.Split(new string[] { "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);
                if (strings.Length > 1)
                {
                    statements.Add(new CodeCommentStatement(strings[0], true));
                    for (int i = 1; i < strings.Length; i++)
                    {
                        statements.Add(new CodeCommentStatement("<para>" + strings[i] + "</para>", true));
                    }
                }
                else
                {
                    statements.Add(new CodeCommentStatement(strings[0], true));
                }
                statements.Add(new CodeCommentStatement("</summary>", true));
            }
            return statements;
        }

        /// <summary>
        /// 添加属性
        /// </summary>
        /// <param name="name">属性名称</param>
        /// <param name="type">属性数据类型</param>
        /// <param name="initExpression">属性初始默认值</param>
        /// <param name="comments">属性文档型注释</param>
        /// <param name="codeAttributeDeclarations">特性集合</param>
        public void AddProperty(string name, Type type, CodeExpression initExpression, string comments, CodeAttributeDeclaration[] codeAttributeDeclarations = null)
        {
            if (string.IsNullOrEmpty(name)) { return; }
            if (pNames.Find(o => o.Equals(name, StringComparison.OrdinalIgnoreCase)) != null) { return; }

            string memberName = "_" + name;
            //创建成员
            CodeMemberField menber = new CodeMemberField();
            menber.Attributes = MemberAttributes.Private;
            menber.Type = new CodeTypeReference(type);
            menber.Name = memberName;
            if (initExpression != null)
            {
                menber.InitExpression = initExpression;
            }
            members.Add(menber);

            //创建属性访问器
            CodeMemberProperty property = new CodeMemberProperty();
            property.Attributes = MemberAttributes.Public | MemberAttributes.Final;
            property.Name = name;
            property.HasGet = true;
            property.HasSet = true;
            property.Type = new CodeTypeReference(type);

            List<CodeCommentStatement> codeComments = CreateSummaryCodeCommentStatements(comments);
            if (codeComments.Count > 0)
            {
                foreach (CodeCommentStatement codeComment in codeComments)
                {
                    property.Comments.Add(codeComment);
                }
            }
            if (codeAttributeDeclarations != null && codeAttributeDeclarations.Length > 0)
            {
                foreach (var attr in codeAttributeDeclarations)
                {
                    property.CustomAttributes.Add(attr);
                }
            }
            property.GetStatements.Add(new CodeMethodReturnStatement(
                new CodeFieldReferenceExpression(
                new CodeThisReferenceExpression(), memberName)));
            property.SetStatements.Add(new CodeAssignStatement(
                new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), memberName), //left
                new CodePropertySetValueReferenceExpression()) //right
                );
            properties.Add(property);
        }

        public string GenerateCSharpCode(string remark = "")
        {
            var targetUnit = GetCodeCompileUnit();
            CodeNamespace myCodeNamespace;
            SetNamespaces(targetUnit, out myCodeNamespace);
            CodeTypeDeclaration myClass;
            SetCodeTypeDeclaration(myCodeNamespace, out myClass);
            SetCodeMembers(myClass);

            CodeDomProvider provider = CodeDomProvider.CreateProvider("CSharp");//CSharp VisualBasic
            CodeGeneratorOptions options = new CodeGeneratorOptions();
            options.BracingStyle = "C";
            StringBuilder result = new StringBuilder();
            using (StringWriter stringWriter = new StringWriter(result))
            {
                provider.GenerateCodeFromCompileUnit(targetUnit, stringWriter, options);
            }
            string reval = result.ToString();
            if (!string.IsNullOrEmpty(remark))
            {
                string pat = @"//\-+[\s\S]+?//\-+";
                string[] a = remark.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                List<string> lines = new List<string>();
                foreach (string line in a)
                {
                    if (!string.IsNullOrEmpty(line))
                    {
                        lines.Add(string.Format("// {0}", line.Trim()));
                    }
                }
                remark = string.Join("\r\n", lines.ToArray());
                reval = System.Text.RegularExpressions.Regex.Replace(reval, pat, remark);
            }
            return reval;
        }



    }
}
