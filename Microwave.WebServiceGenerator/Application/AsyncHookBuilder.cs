﻿using System.CodeDom;
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
        private NameBuilderUtil _nameBuilderUtil;

        public AsyncHookBuilder(string applicationNameSpace)
        {
            _applicationNameSpace = applicationNameSpace;
            _nameSpaceBuilderUtil = new NameSpaceBuilderUtil();
            _classBuilderUtil = new ClassBuilderUtil();
            _nameBuilderUtil = new NameBuilderUtil();
        }

        public CodeNamespace BuildReplacementClass(AsyncDomainHook hook)
        {
            var codeNamespace = _nameSpaceBuilderUtil.WithName($"{_applicationNameSpace}.{hook.ClassType}s.AsyncHooks").WithList().Build();
            var codeTypeDeclaration = _classBuilderUtil.Build(_nameBuilderUtil.BuildAsyncEventHookName(hook));
            codeNamespace.Imports.Add(new CodeNamespaceImport($"Domain.{hook.ClassType}s"));

            codeNamespace.Types.Add(codeTypeDeclaration);

            var codeMemberMethod = new CodeMemberMethod();
            codeMemberMethod.Parameters.Add(
                new CodeParameterDeclarationExpression(new CodeTypeReference($"{hook.ClassType}{hook.MethodName}Event"),
                    "domainEvent"));
            codeMemberMethod.ReturnType = new CodeTypeReference(new HookResultBaseClass().Name);
            codeMemberMethod.Attributes = MemberAttributes.Public | MemberAttributes.Final;
            codeMemberMethod.Name = "Execute";

            codeMemberMethod.Statements.Add(new CodeSnippetExpression($"Console.WriteLine(\"ERROR: The generated Async Domain Hook Method {hook.Name} that is not implemented was called, aborting...\")"));
            codeMemberMethod.Statements.Add(new CodeSnippetExpression("return HookResult.ErrorResult(new List<string>())"));
            codeTypeDeclaration.Members.Add(codeMemberMethod);
            return codeNamespace;
        }
    }
}