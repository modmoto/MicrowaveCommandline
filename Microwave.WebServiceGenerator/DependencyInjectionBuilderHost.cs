using System.CodeDom;
using System.Collections.Generic;
using Microwave.LanguageModel;
using Microwave.WebServiceGenerator.Util;
using Microwave.WebServiceModel.Application;
using Microwave.WebServiceModel.SqlAdapter;

namespace Microwave.WebServiceGenerator
{
    public class DependencyInjectionBuilderHost
    {
        private ClassBuilderUtil _classBuilderUtil;
        private NameSpaceBuilderUtil _nameSpaceBuilderUtil;

        public DependencyInjectionBuilderHost()
        {
            _classBuilderUtil = new ClassBuilderUtil();
            _nameSpaceBuilderUtil = new NameSpaceBuilderUtil();
        }

        public CodeNamespace Build(IList<DomainClass> domainClasses, IList<SynchronousDomainHook> domainHooks, IList<OnChildDomainHook> childDomainHooks)
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
            codeMemberMethod.Statements.Add(new CodeSnippetExpression($"collection.AddTransient<{new QueueRepositoryInterface().Name}, {new QueueRepositoryClass().Name}>()"));
            codeMemberMethod.Statements.Add(new CodeSnippetExpression("collection.AddTransient<EventJobRegistration>()"));
            codeMemberMethod.Statements.Add(new CodeSnippetExpression($"collection.AddTransient<{new HangfireQueueInterface().Name}, HangfireQueue>()"));
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
                codeMemberMethod.Statements.Add(new CodeSnippetExpression($"collection.AddTransient<{new DomainHookBaseClass().Name}, {hook.Name}Hook>()"));
            }

            foreach (var hook in childDomainHooks)
            {
                codeMemberMethod.Statements.Add(new CodeSnippetExpression($"collection.AddTransient<{new DomainHookBaseClass().Name}, {hook.Name}Hook>()"));
            }

            var codeNamespace = _nameSpaceBuilderUtil.Build();

            codeNamespace.Types.Add(codeTypeDeclaration);

            return codeNamespace;
        }
    }
}