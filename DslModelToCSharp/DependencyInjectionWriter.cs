using System.CodeDom;
using System.Collections.Generic;
using DslModel.Domain;
using DslModelToCSharp.Domain;
using DslModelToCSharp.Util;

namespace DslModelToCSharp
{
    public class DependencyInjectionWriter
    {
        private readonly IFileWriter _fileWriter;
        private ClassBuilderUtil _classBuilderUtil;
        private NameSpaceBuilderUtil _nameSpaceBuilderUtil;

        public DependencyInjectionWriter(string basePath)
        {
            _fileWriter = new FileWriter(basePath);
            _classBuilderUtil = new ClassBuilderUtil();
            _nameSpaceBuilderUtil = new NameSpaceBuilderUtil();
        }

        public void Write(IList<DomainClass> domainClasses, IList<SynchronousDomainHook> domainHooks, string basePath)
        {
            var codeTypeDeclaration = _classBuilderUtil.Build("GeneratedDependencies");
            codeTypeDeclaration.Attributes = MemberAttributes.Final | MemberAttributes.Public;
            var codeNamespace = _nameSpaceBuilderUtil.Build("GeneratedWebService");
            codeNamespace.Types.Add(codeTypeDeclaration);

            codeNamespace.Imports.Add(new CodeNamespaceImport("Application"));
            codeNamespace.Imports.Add(new CodeNamespaceImport("Microsoft.Extensions.DependencyInjection"));
            codeNamespace.Imports.Add(new CodeNamespaceImport("SqlAdapter"));

            var codeMemberMethod = new CodeMemberMethod();
            codeMemberMethod.Attributes = MemberAttributes.Static | MemberAttributes.Public |  MemberAttributes.Final;
            codeMemberMethod.Name = "ConfigureGeneratedServices";
            codeTypeDeclaration.Members.Add(codeMemberMethod);
            codeMemberMethod.Parameters.Add(new CodeParameterDeclarationExpression("IServiceCollection", "collection"));

            codeMemberMethod.Statements.Add(new CodeSnippetExpression("collection.AddTransient<EventStore>()"));
            codeMemberMethod.Statements.Add(new CodeSnippetExpression("collection.AddTransient<IEventStoreRepository, EventStoreRepository>()"));
            codeMemberMethod.Statements.Add(new CodeSnippetExpression($"collection.AddMvc().AddApplicationPart(typeof({domainClasses[0].Name}Controller).Assembly)"));

            foreach (var domainClass in domainClasses)
            {
                codeNamespace.Imports.Add(new CodeNamespaceImport($"Application.{domainClass.Name}s"));
                codeNamespace.Imports.Add(new CodeNamespaceImport($"HttpAdapter.{domainClass.Name}s"));
                codeNamespace.Imports.Add(new CodeNamespaceImport($"SqlAdapter.{domainClass.Name}s"));

                codeMemberMethod.Statements.Add(new CodeSnippetExpression(
                    $"collection.AddTransient<I{domainClass.Name}Repository, {domainClass.Name}Repository>()"));
                codeMemberMethod.Statements.Add(new CodeSnippetExpression(
                    $"collection.AddTransient<{domainClass.Name}CommandHandler>()"));
            }

            foreach (var hook in domainHooks)
            {
                codeNamespace.Imports.Add(new CodeNamespaceImport($"Application.{hook.ClassType}s.Hooks"));
                codeMemberMethod.Statements.Add(new CodeSnippetExpression($"collection.AddTransient<{hook.Name}Hook>()"));
            }

            _fileWriter.WriteToFile(codeTypeDeclaration.Name, "Base", codeNamespace);

            new PrivateSetPropertyHackCleaner().ReplaceHackPropertyNames(basePath);
        }
    }
}