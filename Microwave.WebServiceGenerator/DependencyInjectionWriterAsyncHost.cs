using System.CodeDom;
using System.Collections.Generic;
using Microwave.LanguageModel;
using Microwave.WebServiceGenerator.Util;

namespace Microwave.WebServiceGenerator
{
    public class DependencyInjectionWriterAsyncHost
    {
        private readonly IFileWriter _fileWriter;
        private readonly ClassBuilderUtil _classBuilderUtil;
        private readonly NameSpaceBuilderUtil _nameSpaceBuilderUtil;

        public DependencyInjectionWriterAsyncHost(string basePath)
        {
            _fileWriter = new FileWriter(basePath);
            _classBuilderUtil = new ClassBuilderUtil();
            _nameSpaceBuilderUtil = new NameSpaceBuilderUtil();
        }

        public void Write(IList<DomainClass> domainClasses, IList<AsyncDomainHook> domainHooks, string basePath)
        {
            var codeTypeDeclaration = _classBuilderUtil.Build("GeneratedDependencies");
            codeTypeDeclaration.Attributes = MemberAttributes.Final | MemberAttributes.Public;

            _nameSpaceBuilderUtil.WithName("AsyncHost")
                .WithApplication()
                .WithDependencyInjection()
                .WithHangfire()
                .WithApplicatioBuilder()
                .WithSqlAdapter();

            var codeMemberMethod = new CodeMemberMethod();
            codeMemberMethod.Attributes = MemberAttributes.Static | MemberAttributes.Public |  MemberAttributes.Final;
            codeMemberMethod.Name = "ConfigureGeneratedServices";
            codeTypeDeclaration.Members.Add(codeMemberMethod);
            codeMemberMethod.Parameters.Add(new CodeParameterDeclarationExpression("IServiceCollection", "collection"));

            codeMemberMethod.Statements.Add(new CodeSnippetExpression("collection.AddTransient<IEventStore, EventStore>()"));
            codeMemberMethod.Statements.Add(new CodeSnippetExpression("collection.AddTransient<IEventStoreRepository, EventStoreRepository>()"));

            foreach (var domainClass in domainClasses)
            {
                _nameSpaceBuilderUtil
                    .WithApplicationEntityNameSpace(domainClass.Name)
                    .WithSqlEntityNameSpace(domainClass.Name);

                codeMemberMethod.Statements.Add(new CodeSnippetExpression(
                    $"collection.AddTransient<I{domainClass.Name}Repository, {domainClass.Name}Repository>()"));
            }

            foreach (var hook in domainHooks)
            {
                _nameSpaceBuilderUtil.WithAsyncHookEntityNameSpace(hook.ClassType);
                codeMemberMethod.Statements.Add(new CodeSnippetExpression($"collection.AddTransient<{hook.Name}EventHandler>()"));
                codeMemberMethod.Statements.Add(new CodeSnippetExpression($"collection.AddTransient<{hook.Name}AsyncHook>()"));
            }

            var codeNamespace = _nameSpaceBuilderUtil.Build();

            var codeMemberMethodApplicationConfig = new CodeMemberMethod();
            codeMemberMethodApplicationConfig.Attributes = MemberAttributes.Static | MemberAttributes.Public | MemberAttributes.Final;
            codeMemberMethodApplicationConfig.Name = "ConfigureApplication";
            codeMemberMethodApplicationConfig.Parameters.Add(new CodeParameterDeclarationExpression("IApplicationBuilder", "app"));

            codeTypeDeclaration.Members.Add(codeMemberMethodApplicationConfig);

            codeMemberMethodApplicationConfig.Statements.Add(new CodeSnippetExpression("var option = new BackgroundJobServerOptions {WorkerCount = 1}"));
            codeMemberMethodApplicationConfig.Statements.Add(new CodeSnippetExpression("app.UseHangfireServer(option)"));
            codeMemberMethodApplicationConfig.Statements.Add(new CodeSnippetExpression("app.UseHangfireDashboard()"));

            foreach (var asyncDomainHook in domainHooks)
            {
                codeMemberMethodApplicationConfig.Statements.Add(new CodeSnippetExpression($"RecurringJob.AddOrUpdate<{asyncDomainHook.Name}EventHandler>(handler => handler.Run(), Cron.Minutely())"));
            }

            codeNamespace.Types.Add(codeTypeDeclaration);

            _fileWriter.WriteToFile(codeTypeDeclaration.Name, "Base", codeNamespace);

            new PrivateSetPropertyHackCleaner().ReplaceHackPropertyNames(basePath);
        }
    }
}