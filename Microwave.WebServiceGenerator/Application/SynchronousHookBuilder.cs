using System.CodeDom;
using Microwave.LanguageModel.Application;
using Microwave.LanguageModel.Domain;
using Microwave.WebServiceGenerator.Util;

namespace Microwave.WebServiceGenerator.Application
{
    public class SynchronousHookBuilder
    {
        private readonly string _applicationNameSpace;
        private NameSpaceBuilderUtil _nameSpaceBuilderUtil;
        private ClassBuilderUtil _classBuilderUtil;
        private PropertyBuilderUtil _propertyBuilderUtil;

        public SynchronousHookBuilder(string applicationNameSpace)
        {
            _applicationNameSpace = applicationNameSpace;
            _nameSpaceBuilderUtil = new NameSpaceBuilderUtil();
            _classBuilderUtil = new ClassBuilderUtil();
            _propertyBuilderUtil = new PropertyBuilderUtil();
        }

        public CodeNamespace Build(SynchronousDomainHook domainClass)
        {
            var codeNamespace = _nameSpaceBuilderUtil.WithName($"{_applicationNameSpace}.{domainClass.ClassType}s.Hooks").Build();
            var codeTypeDeclaration = _classBuilderUtil.BuildPartial($"{domainClass.Name}Hook");
            codeNamespace.Imports.Add(new CodeNamespaceImport($"Domain.{domainClass.ClassType}s"));
            codeNamespace.Imports.Add(new CodeNamespaceImport($"Domain"));

            codeTypeDeclaration.BaseTypes.Add(new CodeTypeReference(new DomainHookBaseClass().Name));

            var field = new CodeMemberField
            {
                Name = $"EventType {{ get; private set; }} = typeof({domainClass.ClassType}{domainClass.MethodName}Event);NewHackGuid302315ed-3a05-4992-9f76-4cf075cde515",
                Attributes = MemberAttributes.Public | MemberAttributes.Final,
                Type = new CodeTypeReference("Type")
            };

            codeTypeDeclaration.Members.Add(field);

            codeNamespace.Types.Add(codeTypeDeclaration);

            var constructor = new CodeConstructor();
            constructor.Statements.Add(new CodeAssignStatement(new CodeVariableReferenceExpression("this.EventType"), new CodeSnippetExpression("typeof(UserCreateEvent)")));
            constructor.Attributes = MemberAttributes.Public | MemberAttributes.Final;

            var codeMemberMethod = new CodeMemberMethod();
            codeMemberMethod.Parameters.Add(
                new CodeParameterDeclarationExpression(new CodeTypeReference(new DomainEventBaseClass().Name),
                    "domainEvent"));
            codeMemberMethod.ReturnType = new CodeTypeReference(new DomainHookBaseClass().Methods[0].ReturnType);
            codeMemberMethod.Attributes = MemberAttributes.Public | MemberAttributes.Final;
            codeMemberMethod.Name = new DomainHookBaseClass().Methods[0].Name;;
            codeMemberMethod.Statements.Add(new CodeConditionStatement(
                new CodeSnippetExpression(
                    $"domainEvent is {domainClass.ClassType}{domainClass.MethodName}Event parsedEvent"), 
                new CodeStatement[]
                {
                    new CodeExpressionStatement(new  CodeSnippetExpression("return Execute(parsedEvent)"))
                }
            ));

            codeMemberMethod.Statements.Add(
                new CodeSnippetExpression("throw new Exception(\"Event is not in the correct list\")"));

            codeTypeDeclaration.Members.Add(codeMemberMethod);
            return codeNamespace;
        }

        public CodeNamespace BuildReplacementClass(SynchronousDomainHook domainClass)
        {
            var codeNamespace = _nameSpaceBuilderUtil.WithName($"{_applicationNameSpace}.{domainClass.ClassType}s.Hooks").WithList().Build();
            var codeTypeDeclaration = _classBuilderUtil.BuildPartial($"{domainClass.Name}Hook");
            codeNamespace.Imports.Add(new CodeNamespaceImport($"Domain.{domainClass.ClassType}s"));
            codeNamespace.Imports.Add(new CodeNamespaceImport($"Domain"));
            codeNamespace.Types.Add(codeTypeDeclaration);

            var codeMemberMethod = new CodeMemberMethod();
            codeMemberMethod.Parameters.Add(
                new CodeParameterDeclarationExpression(new CodeTypeReference(new DomainEventBaseClass().Name),
                    "domainEvent"));
            codeMemberMethod.ReturnType = new CodeTypeReference(new DomainHookBaseClass().Methods[0].ReturnType);
            codeMemberMethod.Attributes = MemberAttributes.Private | MemberAttributes.Final;
            codeMemberMethod.Name = "Execute";

            codeMemberMethod.Statements.Add(new CodeSnippetExpression("return HookResult.ErrorResult(new List<string>{\"A generated Synchronouse Doman Hook Method that is not implemented was called, aborting...\"})"));
            codeTypeDeclaration.Members.Add(codeMemberMethod);
            return codeNamespace;
        }
    }
}