using System.CodeDom;
using System.Collections.Generic;
using Microwave.LanguageModel;
using Microwave.WebServiceGenerator.Util;
using Microwave.WebServiceModel.Application;
using Microwave.WebServiceModel.SqlAdapter;

namespace Microwave.WebServiceGenerator
{
    public class DependencyInjectionBuilderAsyncHost
    {
        private readonly ClassBuilderUtil _classBuilderUtil;
        private readonly NameSpaceBuilderUtil _nameSpaceBuilderUtil;
        private NameBuilderUtil _nameBuilderUtil;

        public DependencyInjectionBuilderAsyncHost()
        {
            _classBuilderUtil = new ClassBuilderUtil();
            _nameSpaceBuilderUtil = new NameSpaceBuilderUtil();
            _nameBuilderUtil = new NameBuilderUtil();
        }

        public CodeNamespace Write(IList<DomainClass> domainClasses, IList<AsyncDomainHook> domainHooks)
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
            codeMemberMethod.Statements.Add(new CodeSnippetExpression($"collection.AddTransient<{new QueueRepositoryInterface().Name}, {new QueueRepositoryClass().Name}>()"));
            codeMemberMethod.Statements.Add(new CodeSnippetExpression("collection.AddTransient<EventJobRegistration>()"));
            codeMemberMethod.Statements.Add(new CodeSnippetExpression($"collection.AddTransient<{new HangfireQueueInterface().Name}, HangfireQueue>()"));

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
                codeMemberMethod.Statements.Add(new CodeSnippetExpression($"collection.AddTransient<{_nameBuilderUtil.AsyncEventHookHandlerName(hook)}>()"));
                codeMemberMethod.Statements.Add(new CodeSnippetExpression($"collection.AddTransient<{_nameBuilderUtil.AsyncEventHookName(hook)}>()"));
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

            foreach (var hook in domainHooks)
            {
                codeMemberMethodApplicationConfig.Statements.Add(new CodeSnippetExpression($"RecurringJob.AddOrUpdate<{_nameBuilderUtil.AsyncEventHookHandlerName(hook)}>(handler => handler.Run(), Cron.Minutely())"));
            }

            codeNamespace.Types.Add(codeTypeDeclaration);

            return codeNamespace;
        }
    }
}