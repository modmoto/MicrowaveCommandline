﻿using System.CodeDom;
using System.Collections.Generic;
using DslModel.Application;
using DslModel.Domain;

namespace DslModelToCSharp.Application
{
    public class SynchronousHookBuilder
    {
        private readonly string _applicationNameSpace;
        private NameSpaceBuilder _nameSpaceBuilder;
        private ClassBuilder _classBuilder;
        private PropBuilder _propBuilder;

        public SynchronousHookBuilder(string applicationNameSpace)
        {
            _applicationNameSpace = applicationNameSpace;
            _nameSpaceBuilder = new NameSpaceBuilder();
            _classBuilder = new ClassBuilder();
            _propBuilder = new PropBuilder();
        }

        public CodeNamespace Build(SynchronousDomainHook domainClass)
        {
            var codeNamespace = _nameSpaceBuilder.Build($"{_applicationNameSpace}.{domainClass.ClassType}s.Hooks");
            var codeTypeDeclaration = _classBuilder.BuildPartial($"{domainClass.Name}Hook");
            codeNamespace.Imports.Add(new CodeNamespaceImport($"Domain.{domainClass.ClassType}s"));
            codeNamespace.Imports.Add(new CodeNamespaceImport($"Domain"));

            codeTypeDeclaration.BaseTypes.Add(new CodeTypeReference("IDomainHook"));
            _propBuilder.Build(codeTypeDeclaration, new List<Property> {new Property{Name = "EventType", Type = "Type"}});
            codeNamespace.Types.Add(codeTypeDeclaration);

            var constructor = new CodeConstructor();
            constructor.Statements.Add(new CodeAssignStatement(new CodeVariableReferenceExpression("this.EventType"), new CodeSnippetExpression("typeof(UserCreateEvent)")));
            constructor.Attributes = MemberAttributes.Public | MemberAttributes.Final;

            var codeMemberMethod = new CodeMemberMethod();
            codeMemberMethod.Parameters.Add(
                new CodeParameterDeclarationExpression(new CodeTypeReference(new DomainEventBaseClass().Name),
                    "domainEvent"));
            codeMemberMethod.ReturnType = new CodeTypeReference(new HookResultBaseClass().Name);
            codeMemberMethod.Attributes = MemberAttributes.Public | MemberAttributes.Final;
            codeMemberMethod.Name = "ExecuteSave";
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
    }
}