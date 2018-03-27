using System.CodeDom;
using System.Collections.Generic;
using Microwave.LanguageModel.Application;
using Microwave.LanguageModel.SqlAdapter;
using Microwave.ServiceParser.Util;

namespace Microwave.ServiceParser.SqlAdapter
{
    public class EventStoreRepositoryBuilder : ICSharpClassBuilder
    {
        private readonly string _nameSpace;
        private readonly NameSpaceBuilderUtil _nameSpaceBuilderUtil;
        private readonly ClassBuilderUtil _classBuilderUtil;
        private readonly ConstructorBuilderUtil _constructorBuilderUtil;
        private readonly PropertyBuilderUtil _propertyBuilderUtil;

        public EventStoreRepositoryBuilder(string nameSpace)
        {
            _nameSpace = nameSpace;
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
            return _classBuilderUtil.Build(new EventStoreRepository().Name);
        }

        public void AddClassProperties(CodeTypeDeclaration targetClass)
        {
            _propertyBuilderUtil.Build(targetClass, new EventStoreRepository().Properties);
        }

        public void AddConstructor(CodeTypeDeclaration targetClass)
        {
            var codeConstructor = _constructorBuilderUtil.BuildPublic(new EventStoreRepository().Properties);
            targetClass.Members.Add(codeConstructor);
        }

        public void AddBaseTypes(CodeTypeDeclaration targetClass)
        {
            targetClass.BaseTypes.Add(new CodeTypeReference(new EventStoreRepositoryInterface().Name));
        }

        public void AddConcreteMethods(CodeTypeDeclaration targetClass)
        {
            var codeMemberMethods = new List<CodeMemberMethod>();
            foreach (var method in new EventStoreRepository().Methods)
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
            memberMethod.Statements.Add(
                new CodeSnippetExpression("await Context.EventHistory.AddRangeAsync(domainEvents)"));
            targetClass.Members.AddRange(codeMemberMethods.ToArray());
        }
    }
}