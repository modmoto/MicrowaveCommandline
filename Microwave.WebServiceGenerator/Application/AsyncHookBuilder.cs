using System.CodeDom;
using Microwave.LanguageModel;
using Microwave.WebServiceGenerator.Util;
using Microwave.WebServiceModel.Application;

namespace Microwave.WebServiceGenerator.Application
{
    public class AsyncHookBuilder : IConcreteClassBuilder
    {
        private readonly AsyncDomainHook _hook;
        private readonly NameSpaceBuilderUtil _nameSpaceBuilderUtil;
        private readonly ClassBuilderUtil _classBuilderUtil;
        private readonly NameBuilderUtil _nameBuilderUtil;
        private CodeNamespace _nameSpace;
        private CodeTypeDeclaration _targetClass;

        public AsyncHookBuilder(AsyncDomainHook hook)
        {
            _hook = hook;
            _nameSpaceBuilderUtil = new NameSpaceBuilderUtil();
            _classBuilderUtil = new ClassBuilderUtil();
            _nameBuilderUtil = new NameBuilderUtil();
        }

        public void AddNameSpace()
        {
            _nameSpace = _nameSpaceBuilderUtil.WithName($"Application.{_hook.ClassType}s.AsyncHooks")
                .WithList()
                .WithTask()
                .WithDomainEntityNameSpace(_hook.ClassType)
                .Build();
        }

        public void AddClassType()
        {
            _targetClass = _classBuilderUtil.Build(_nameBuilderUtil.AsyncEventHookName(_hook));
            _nameSpace.Types.Add(_targetClass);
        }

        public void AddClassProperties()
        {
        }

        public void AddConstructor()
        {
        }

        public void AddBaseTypes()
        {
        }

        public void AddConcreteMethods()
        {
            var codeMemberMethod = new CodeMemberMethod();
            codeMemberMethod.Parameters.Add(
                new CodeParameterDeclarationExpression(new CodeTypeReference($"{_hook.ClassType}{_hook.MethodName}Event"),
                    "domainEvent"));
            codeMemberMethod.ReturnType = new CodeTypeReference($"async Task<{new HookResultBaseClass().Name}>");
            codeMemberMethod.Attributes = MemberAttributes.Public | MemberAttributes.Final;
            codeMemberMethod.Name = "Execute";

            codeMemberMethod.Statements.Add(new CodeSnippetExpression($"Console.WriteLine(\"ERROR: The generated Async Domain Hook Method {_nameBuilderUtil.AsyncEventHookName(_hook)} that is not implemented was called, aborting...\")"));
            codeMemberMethod.Statements.Add(new CodeSnippetExpression("return await Task.FromResult(HookResult.ErrorResult(new List<string>()))"));
            _targetClass.Members.Add(codeMemberMethod);
        }

        public CodeNamespace Build()
        {
            return _nameSpace;
        }
    }
}