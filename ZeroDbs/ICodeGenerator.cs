using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZeroDbs
{
    public interface ICodeGenerator
    {
        ICodeGenerator SetDb(IDatabase db);
        ICodeGenerator SetUsingNamespaces(params string[]? usingNamespaces);
        ICodeGenerator SetClassNamespace(string classNamespace);
        ICodeGenerator SetPartial(bool isPartial);
        ICodeGenerator SetStaticMethod(bool useStaticMethod);
        /// <summary>
        /// 黑名单表集合(排除项)
        /// </summary>
        /// <param name="tabelNames"></param>
        /// <returns></returns>
        ICodeGenerator SetTableBlackList(params string[]? tabelNames);
        /// <summary>
        /// 白名单表集合(仅生成)
        /// </summary>
        /// <param name="tabelNames"></param>
        /// <returns></returns>
        ICodeGenerator SetTableWhiteList(params string[]? tabelNames);
        void Start(CodeGenerateTableHandler tableCallback, CodeGenerateDoneHandler doneCallback);
    }
}
