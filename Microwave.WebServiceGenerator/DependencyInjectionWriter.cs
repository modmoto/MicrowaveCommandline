using System.CodeDom;
using System.Collections.Generic;
using Microwave.LanguageModel;
using Microwave.WebServiceGenerator.Util;

namespace Microwave.WebServiceGenerator
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

            _nameSpaceBuilderUtil.WithName("Host")
                .WithApplication()
                .WithDependencyInjection()
                .WithSqlAdapter();

            var codeMemberMethod = new CodeMemberMethod();
            codeMemberMethod.Attributes = MemberAttributes.Static | MemberAttributes.Public |  MemberAttributes.Final;
            codeMemberMethod.Name = "ConfigureGeneratedServices";
            codeTypeDeclaration.Members.Add(codeMemberMethod);
            codeMemberMethod.Parameters.Add(new CodeParameterDeclarationExpression("IServiceCollection", "collection"));

            codeMemberMethod.Statements.Add(new CodeSnippetExpression("collection.AddTransient<IEventStore, EventStore>()"));
            codeMemberMethod.Statements.Add(new CodeSnippetExpression("collection.AddTransient<IEventStoreRepository, EventStoreRepository>()"));
            codeMemberMethod.Statements.Add(new CodeSnippetExpression($"collection.AddMvc().AddApplicationPart(typeof({domainClasses[0].Name}Controller).Assembly)"));

            foreach (var domainClass in domainClasses)
            {
                _nameSpaceBuilderUtil
                    .WithApplicationEntityNameSpace(domainClass.Name)
                    .WithHttpAdapterEntityNameSpace(domainClass.Name)
                    .WithSqlEntityNameSpace(domainClass.Name);

                codeMemberMethod.Statements.Add(new CodeSnippetExpression(
                    $"collection.AddTransient<I{domainClass.Name}Repository, {domainClass.Name}Repository>()"));
                codeMemberMethod.Statements.Add(new CodeSnippetExpression(
                    $"collection.AddTransient<{domainClass.Name}CommandHandler>()"));
            }

            foreach (var hook in domainHooks)
            {
                _nameSpaceBuilderUtil.WithHookEntityNameSpace(hook.ClassType);
                codeMemberMethod.Statements.Add(new CodeSnippetExpression($"collection.AddTransient<{hook.Name}Hook>()"));
            }

            var codeNamespace = _nameSpaceBuilderUtil.Build();

            codeNamespace.Types.Add(codeTypeDeclaration);

            _fileWriter.WriteToFile(codeTypeDeclaration.Name, "Base", codeNamespace);

            new PrivateSetPropertyHackCleaner().ReplaceHackPropertyNames(basePath);
        }
    }
}