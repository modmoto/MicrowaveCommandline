using System.CodeDom;
using Microwave.LanguageModel;
using Microwave.WebServiceGenerator.Util;
using Microwave.WebServiceModel.Application;
using Microwave.WebServiceModel.Domain;

namespace Microwave.WebServiceGenerator.Application
{
    public class AsyncHookBuilder
    {
        private readonly string _applicationNameSpace;
        private readonly NameSpaceBuilderUtil _nameSpaceBuilderUtil;
        private readonly ClassBuilderUtil _classBuilderUtil;

        public AsyncHookBuilder(string applicationNameSpace)
        {
            _applicationNameSpace = applicationNameSpace;
            _nameSpaceBuilderUtil = new NameSpaceBuilderUtil();
            _classBuilderUtil = new ClassBuilderUtil();
        }

        public CodeNamespace BuildReplacementClass(AsyncDomainHook domainClass)
        {
            var codeNamespace = _nameSpaceBuilderUtil.WithName($"{_applicationNameSpace}.{domainClass.ClassType}s.AsyncHooks").WithList().Build();
            var codeTypeDeclaration = _classBuilderUtil.Build($"{domainClass.Name}AsyncHook");
            codeNamespace.Imports.Add(new CodeNamespaceImport($"Domain.{domainClass.ClassType}s"));

            codeNamespace.Types.Add(codeTypeDeclaration);

            var codeMemberMethod = new CodeMemberMethod();
            codeMemberMethod.Parameters.Add(
                new CodeParameterDeclarationExpression(new CodeTypeReference($"{domainClass.ClassType}{domainClass.MethodName}Event"),
                    "domainEvent"));
            codeMemberMethod.ReturnType = new CodeTypeReference(new HookResultBaseClass().Name);
            codeMemberMethod.Attributes = MemberAttributes.Public | MemberAttributes.Final;
            codeMemberMethod.Name = "Execute";

            codeMemberMethod.Statements.Add(new CodeSnippetExpression("return HookResult.ErrorResult(new List<string>{\"A generated Async Domain Hook Method that is not implemented was called, aborting...\"})"));
            codeTypeDeclaration.Members.Add(codeMemberMethod);
            return codeNamespace;
        }
    }
}