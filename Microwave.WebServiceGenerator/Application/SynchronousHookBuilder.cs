using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using Microwave.LanguageModel;
using Microwave.WebServiceGenerator.Util;
using Microwave.WebServiceModel.Application;
using Microwave.WebServiceModel.Domain;

namespace Microwave.WebServiceGenerator.Application
{
    public class SynchronousHookBuilder
    {
        private readonly string _applicationNameSpace;
        private NameSpaceBuilderUtil _nameSpaceBuilderUtil;
        private ClassBuilderUtil _classBuilderUtil;
        private PropertyBuilderUtil _propertyBuilderUtil;
        private ConstructorBuilderUtil _constructorBuilderUtil;
        private NameBuilderUtil _nameBuilderUtil;

        public SynchronousHookBuilder(string applicationNameSpace)
        {
            _applicationNameSpace = applicationNameSpace;
            _nameSpaceBuilderUtil = new NameSpaceBuilderUtil();
            _classBuilderUtil = new ClassBuilderUtil();
            _propertyBuilderUtil = new PropertyBuilderUtil();
            _constructorBuilderUtil = new ConstructorBuilderUtil();
            _nameBuilderUtil = new NameBuilderUtil();
        }

        public CodeNamespace Build(SynchronousDomainHook domainClass)
        {
            var codeNamespace = _nameSpaceBuilderUtil.WithName($"{_applicationNameSpace}.{domainClass.ClassType}s.Hooks")
                .WithTask()
                .WithDomainEntityNameSpace(domainClass.ClassType)
                .WithDomain()
                .Build();
            var codeTypeDeclaration = _classBuilderUtil.BuildPartial($"{domainClass.Name}Hook");

            codeTypeDeclaration.BaseTypes.Add(new CodeTypeReference(new DomainHookBaseClass().Name));

            var field = new CodeMemberField
            {
                Name = $"EventType {{ get; private set; }} = typeof({domainClass.ClassType}{domainClass.MethodName}Event);NewHackGuid302315ed-3a05-4992-9f76-4cf075cde515",
                Attributes = MemberAttributes.Public | MemberAttributes.Final,
                Type = new CodeTypeReference("Type")
            };

            codeTypeDeclaration.Members.Add(field);

            codeNamespace.Types.Add(codeTypeDeclaration);

            var codeMemberMethod = new CodeMemberMethod();
            codeMemberMethod.Parameters.Add(
                new CodeParameterDeclarationExpression(new CodeTypeReference(new DomainEventBaseClass().Name),
                    "domainEvent"));
            codeMemberMethod.ReturnType = new CodeTypeReference($"async {new DomainHookBaseClass().Methods[0].ReturnType}");
            codeMemberMethod.Attributes = MemberAttributes.Public | MemberAttributes.Final;
            codeMemberMethod.Name = new DomainHookBaseClass().Methods[0].Name;
            codeMemberMethod.Statements.Add(new CodeConditionStatement(
                new CodeSnippetExpression(
                    $"domainEvent is {domainClass.ClassType}{domainClass.MethodName}Event parsedEvent"),
                new CodeStatement[]
                {
                    new CodeExpressionStatement(new  CodeSnippetExpression("return await Execute(parsedEvent)"))
                }
            ));

            codeMemberMethod.Statements.Add(
                new CodeSnippetExpression("throw new Exception(\"Event is not in the correct list\")"));

            codeTypeDeclaration.Members.Add(codeMemberMethod);
            return codeNamespace;
        }

        private CodeStatement[] BuildHookExecution(OnChildHookMethod hook)
        {
            return new CodeStatement[]
            {
                new CodeExpressionStatement(new  CodeSnippetExpression($"var parent = await {hook.ContainingClassName}Repository.Get{hook.OriginFieldName}Parent(parsedEvent.EntityId)")),
                new CodeExpressionStatement(new  CodeSnippetExpression($"var domainResult = parent.{_nameBuilderUtil.OnChildHookMethodName(hook)}(parsedEvent)")),
                new CodeConditionStatement(
                    new CodeSnippetExpression(
                        $"domainResult.Ok"),
                    new CodeStatement[]
                    {
                        new CodeExpressionStatement(new  CodeSnippetExpression($"await {hook.ContainingClassName}Repository.Update{hook.ContainingClassName}(parent)")),
                        new CodeExpressionStatement(new  CodeSnippetExpression($"return HookResult.OkResult(domainResult.DomainEvents)")),

                    })
            };
        }

        public CodeNamespace BuildOnChildHook(OnChildHookMethod onChildHook, List<Property> classProperties, List<ListProperty> classListProperties)
        {
            var domainClassName = _nameBuilderUtil.GetClassName(onChildHook, classProperties, classListProperties);
            var methodName = _nameBuilderUtil.OnChildHookMethodName(onChildHook);
            var codeNamespace = _nameSpaceBuilderUtil.WithName($"{_applicationNameSpace}.{onChildHook.ContainingClassName}s.Hooks")
                .WithTask()
                .WithDomain()
                .WithApplicationEntityNameSpace(onChildHook.ContainingClassName)
                .WithDomainEntityNameSpace(domainClassName)
                .Build();

            var codeTypeDeclaration = _classBuilderUtil.BuildPartial($"{methodName}Hook");

            codeTypeDeclaration.BaseTypes.Add(new CodeTypeReference(new DomainHookBaseClass().Name));

            var codeConstructor = _constructorBuilderUtil.BuildPublic(new List<Property>
            {
                new Property {Name = $"{onChildHook.ContainingClassName}Repository", Type = $"I{onChildHook.ContainingClassName}Repository"}
            });
            codeTypeDeclaration.Members.Add(codeConstructor);

            _propertyBuilderUtil.Build(codeTypeDeclaration, new List<Property>{new Property {Name = $"{onChildHook.ContainingClassName}Repository", Type = $"I{onChildHook.ContainingClassName}Repository"}});

            var field = new CodeMemberField
            {
                Name = $"EventType {{ get; private set; }} = typeof({domainClassName}{onChildHook.MethodName}Event);NewHackGuid302315ed-3a05-4992-9f76-4cf075cde515",
                Attributes = MemberAttributes.Public | MemberAttributes.Final,
                Type = new CodeTypeReference("Type")
            };

            codeTypeDeclaration.Members.Add(field);

            codeNamespace.Types.Add(codeTypeDeclaration);

            var codeMemberMethod = new CodeMemberMethod();
            codeMemberMethod.Parameters.Add(
                new CodeParameterDeclarationExpression(new CodeTypeReference(new DomainEventBaseClass().Name),
                    "domainEvent"));
            codeMemberMethod.ReturnType = new CodeTypeReference($"async {new DomainHookBaseClass().Methods[0].ReturnType}");
            codeMemberMethod.Attributes = MemberAttributes.Public | MemberAttributes.Final;
            codeMemberMethod.Name = new DomainHookBaseClass().Methods[0].Name;
            codeMemberMethod.Statements.Add(new CodeConditionStatement(
                new CodeSnippetExpression(
                    $"domainEvent is {domainClassName}{onChildHook.MethodName}Event parsedEvent"),
                BuildHookExecution(onChildHook)
            ));

            codeMemberMethod.Statements.Add(
                new CodeSnippetExpression("throw new Exception(\"Event is not in the correct list\")"));

            codeTypeDeclaration.Members.Add(codeMemberMethod);
            return codeNamespace;
        }

        public CodeNamespace BuildReplacementClass(SynchronousDomainHook domainClass)
        {
            var codeNamespace = _nameSpaceBuilderUtil.WithName($"{_applicationNameSpace}.{domainClass.ClassType}s.Hooks").WithTask().WithList().Build();
            var codeTypeDeclaration = _classBuilderUtil.BuildPartial($"{domainClass.Name}Hook");
            codeNamespace.Imports.Add(new CodeNamespaceImport($"Domain.{domainClass.ClassType}s"));
            codeNamespace.Imports.Add(new CodeNamespaceImport("Domain"));
            codeNamespace.Types.Add(codeTypeDeclaration);

            var codeMemberMethod = new CodeMemberMethod();
            codeMemberMethod.Parameters.Add(
                new CodeParameterDeclarationExpression(new CodeTypeReference($"{domainClass.ClassType}{domainClass.MethodName}Event"),
                    "domainEvent"));
            codeMemberMethod.ReturnType = new CodeTypeReference($"async {new DomainHookBaseClass().Methods[0].ReturnType}");
            codeMemberMethod.Attributes = MemberAttributes.Public | MemberAttributes.Final;
            codeMemberMethod.Name = "Execute";

            codeMemberMethod.Statements.Add(new CodeSnippetExpression("// TODO: Implement this method"));
            codeMemberMethod.Statements.Add(new CodeSnippetExpression("return await Task.FromResult(HookResult.ErrorResult(new List<string>{\"A generated Synchronouse Doman Hook Method that is not implemented was called, aborting...\"}))"));
            codeTypeDeclaration.Members.Add(codeMemberMethod);
            return codeNamespace;
        }
    }
}