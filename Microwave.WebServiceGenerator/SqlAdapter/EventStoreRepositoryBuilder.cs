using System.CodeDom;
using System.Collections.Generic;
using Microwave.WebServiceGenerator.Util;
using Microwave.WebServiceModel.Application;
using Microwave.WebServiceModel.SqlAdapter;

namespace Microwave.WebServiceGenerator.SqlAdapter
{
    public class EventStoreRepositoryBuilder : IConcreteClassBuilder
    {
        private readonly NameSpaceBuilderUtil _nameSpaceBuilderUtil;
        private readonly ClassBuilderUtil _classBuilderUtil;
        private readonly ConstructorBuilderUtil _constructorBuilderUtil;
        private readonly PropertyBuilderUtil _propertyBuilderUtil;
        private readonly EventStoreRepository _eventStoreRepository;

        public EventStoreRepositoryBuilder(EventStoreRepository eventStoreRepository)
        {
            _eventStoreRepository = eventStoreRepository;
            _nameSpaceBuilderUtil = new NameSpaceBuilderUtil();
            _propertyBuilderUtil = new PropertyBuilderUtil();
            _constructorBuilderUtil = new ConstructorBuilderUtil();
            _classBuilderUtil = new ClassBuilderUtil();
        }


        public CodeNamespace BuildNameSpace()
        {
            return _nameSpaceBuilderUtil
                .WithName("SqlAdapter")
                .WithDomain()
                .WithTask()
                .WithList()
                .WithApplication()
                .Build();
        }

        public CodeTypeDeclaration BuildClassType()
        {
            return _classBuilderUtil.Build(_eventStoreRepository.Name);
        }

        public void AddClassProperties(CodeTypeDeclaration targetClass)
        {
            _propertyBuilderUtil.Build(targetClass, _eventStoreRepository.Properties);
        }

        public void AddConstructor(CodeTypeDeclaration targetClass)
        {
            var codeConstructor = _constructorBuilderUtil.BuildPublic(_eventStoreRepository.Properties);
            targetClass.Members.Add(codeConstructor);
        }

        public void AddBaseTypes(CodeTypeDeclaration targetClass)
        {
            targetClass.BaseTypes.Add(new CodeTypeReference(new EventStoreRepositoryInterface().Name));
        }

        public void AddConcreteMethods(CodeTypeDeclaration targetClass)
        {
            var codeMemberMethods = new List<CodeMemberMethod>();
            foreach (var method in _eventStoreRepository.Methods)
            {
                var codeMemberMethod = new CodeMemberMethod();
                codeMemberMethod.Name = method.Name;
                codeMemberMethod.ReturnType = new CodeTypeReference(method.ReturnType);
                codeMemberMethod.Attributes = MemberAttributes.Final | MemberAttributes.Public;
                foreach (var parameter in method.Parameters)
                {
                    var expression = new CodeParameterDeclarationExpression()
                    {
                        Name = parameter.Name,
                        Type = new CodeTypeReference(parameter.Type)
                    };
                    codeMemberMethod.Parameters.Add(expression);
                }

                codeMemberMethods.Add(codeMemberMethod);
            }

            var memberMethod = codeMemberMethods[0];
            memberMethod.Statements.Add(new CodeSnippetExpression("await Context.EventHistory.AddRangeAsync(domainEvents)"));
            memberMethod.Statements.Add(new CodeSnippetExpression("await HangfireQueue.AddEvents(domainEvents)"));
            memberMethod.Statements.Add(new CodeSnippetExpression("await Context.SaveChangesAsync()"));
            targetClass.Members.AddRange(codeMemberMethods.ToArray());
        }
    }
}