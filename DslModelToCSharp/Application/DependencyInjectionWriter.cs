using System.CodeDom;
using System.Collections.Generic;
using DslModel.Application;
using DslModel.Domain;

namespace DslModelToCSharp.Application
{
    public class DependencyInjectionWriter
    {
        private readonly IFileWriter _fileWriter;
        private HookResultBuilder _hookResultBuilder;
        private CommandHandlerBuilder _commandHandlerBuilder;
        private RepositoryInterfaceBuilder _repositoryInterfaceBuilder;
        private ClassBuilder _classBuilder;
        private NameSpaceBuilder _nameSpaceBuilder;

        public DependencyInjectionWriter(string basePath)
        {
            _fileWriter = new FileWriter(basePath);
            _classBuilder = new ClassBuilder();
            _nameSpaceBuilder = new NameSpaceBuilder();
        }

        public void Write(IList<DomainClass> domainClasses, string basePath)
        {
            var codeTypeDeclaration = _classBuilder.Build("GeneratedDependencies");
            codeTypeDeclaration.Attributes = MemberAttributes.Static | MemberAttributes.Public;
            var codeNamespace = _nameSpaceBuilder.Build("GeneratedWebService");
            codeNamespace.Types.Add(codeTypeDeclaration);

            codeNamespace.Imports.Add(new CodeNamespaceImport("Application"));
            codeNamespace.Imports.Add(new CodeNamespaceImport("Microsoft.EntityFrameworkCore"));
            codeNamespace.Imports.Add(new CodeNamespaceImport("Microsoft.Extensions.DependencyInjection"));
            codeNamespace.Imports.Add(new CodeNamespaceImport("SqlAdapter"));

            var codeMemberMethod = new CodeMemberMethod();
            codeMemberMethod.Attributes = MemberAttributes.Static | MemberAttributes.Public;
            codeMemberMethod.ReturnType = new CodeTypeReference("IServiceCollection");
            codeTypeDeclaration.Members.Add(codeMemberMethod);
            codeMemberMethod.Parameters.Add(new CodeParameterDeclarationExpression("this IServiceCollection", "collection"));

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

            _fileWriter.WriteToFile(codeTypeDeclaration.Name, "Base", codeNamespace);

            new PrivateSetPropertyHackCleaner().ReplaceHackPropertyNames(basePath);
        }
    }
}