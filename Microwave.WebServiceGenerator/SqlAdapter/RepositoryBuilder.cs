using System.CodeDom;
using System.Collections.Generic;
using Microwave.LanguageModel;
using Microwave.WebServiceGenerator.Util;

namespace Microwave.WebServiceGenerator.SqlAdapter
{
    public class RepositoryBuilder
    {
        private readonly string _nameSpace;
        private readonly NameSpaceBuilderUtil _nameSpaceBuilderUtil;
        private readonly ClassBuilderUtil _classBuilderUtil;
        private readonly ConstructorBuilderUtil _constructorBuilderUtil;
        private readonly PropertyBuilderUtil _propertyBuilderUtil;
        private NameBuilderUtil _nameBuilderUtil;

        public RepositoryBuilder(string nameSpace)
        {
            _nameSpace = nameSpace;
            _nameSpaceBuilderUtil = new NameSpaceBuilderUtil();
            _propertyBuilderUtil = new PropertyBuilderUtil();
            _constructorBuilderUtil = new ConstructorBuilderUtil();
            _classBuilderUtil = new ClassBuilderUtil();
            _nameBuilderUtil = new NameBuilderUtil();
        }

        public CodeNamespace Build(DomainClass domainClass)
        {
            var nameSpace = _nameSpaceBuilderUtil
                .WithName($"{_nameSpace}.{domainClass.Name}s")
                .WithList()
                .WithLinq()
                .WithTask()
                .WithDomainEntityNameSpace(domainClass.Name)
                .WithApplicationEntityNameSpace(domainClass.Name)
                .WithEfCore()
                .Build();
            
            var repository = _classBuilderUtil.Build($"{domainClass.Name}Repository");

            repository.BaseTypes.Add(new CodeTypeReference($"I{domainClass.Name}Repository"));

            var properties = new List<Property> {new Property {Name = "EventStore", Type = "EventStoreContext"}};
            _propertyBuilderUtil.Build(repository,
                properties);
            var codeConstructor = _constructorBuilderUtil.BuildPublic(properties);
            repository.Members.Add(codeConstructor);

            var createMethod = MakeCreateMethod(domainClass);
            repository.Members.Add(createMethod);

            var updateMethod = MakeUpdateMethod(domainClass);
            repository.Members.Add(updateMethod);

            var getByIdMethod = MakeGetByIdMethod(domainClass);
            repository.Members.Add(getByIdMethod);

            var getAllMethod = MakeGetAllMethod(domainClass);
            repository.Members.Add(getAllMethod);

            var getParentMethods = MakeGetParentMethods(domainClass);
            repository.Members.AddRange(getParentMethods);

            nameSpace.Types.Add(repository);

            return nameSpace;
        }

        private CodeMemberMethod[] MakeGetParentMethods(DomainClass domainClass)
        {
            var parentMethods = new List<CodeMemberMethod>();

            foreach (var onChildHookMethod in domainClass.ChildHookMethods)
            {
                var childEntityName = onChildHookMethod.OriginFieldName;
                var getParentMethod = new CodeMemberMethod
                {
                    Name = $"Get{childEntityName}Parent",
                    ReturnType = new CodeTypeReference($"async Task<{domainClass.Name}>")
                };
                getParentMethod.Parameters.Add(
                    new CodeParameterDeclarationExpression {Type = new CodeTypeReference("Guid"), Name = "childId"});

                if (onChildHookMethodIsListProperty(onChildHookMethod, domainClass.ListProperties))
                {
                    getParentMethod.Statements.Add(
                        new CodeSnippetExpression(
                            $"return await EventStore.{domainClass.Name}s{LoadNestedArgument(domainClass)}" +
                            $".FirstOrDefaultAsync(parent => parent.{childEntityName}.Any(child => child.Id == childId))"));
                }
                else
                {
                    getParentMethod.Statements.Add(
                        new CodeSnippetExpression(
                            $"return await EventStore.{domainClass.Name}s{LoadNestedArgument(domainClass)}" +
                            $".FirstOrDefaultAsync(parent => parent.{childEntityName}.Id == childId)"));
                }

                getParentMethod.Attributes = MemberAttributes.Final | MemberAttributes.Public;

                parentMethods.Add(getParentMethod);
            }

            return parentMethods.ToArray();
        }

        private bool onChildHookMethodIsListProperty(OnChildHookMethod onChildHookMethod, List<ListProperty> domainClassListProperties)
        {
            foreach (var listProperty in domainClassListProperties)
            {
                if (listProperty.Name == onChildHookMethod.OriginFieldName) return true;
            }

            return false;
        }

        private static CodeMemberMethod MakeGetAllMethod(DomainClass domainClass)
        {
            var getAllMethod = new CodeMemberMethod
            {
                Name = $"Get{domainClass.Name}s",
                ReturnType = new CodeTypeReference($"async Task<List<{domainClass.Name}>>")
            };

            getAllMethod.Statements.Add(
                new CodeSnippetExpression(
                    $"return await EventStore.{domainClass.Name}s{LoadNestedArgument(domainClass)}.ToListAsync()"));

            getAllMethod.Attributes = MemberAttributes.Final | MemberAttributes.Public;
            return getAllMethod;
        }

        private static CodeMemberMethod MakeGetByIdMethod(DomainClass domainClass)
        {
            var getByIdMethod = new CodeMemberMethod
            {
                Name = $"Get{domainClass.Name}",
                ReturnType = new CodeTypeReference($"async Task<{domainClass.Name}>")
            };
            getByIdMethod.Parameters.Add(
                new CodeParameterDeclarationExpression {Type = new CodeTypeReference("Guid"), Name = "id"});

            getByIdMethod.Statements.Add(
                new CodeSnippetExpression(
                    $"return await EventStore.{domainClass.Name}s{LoadNestedArgument(domainClass)}.FirstOrDefaultAsync(entity => entity.Id == id)"));

            getByIdMethod.Attributes = MemberAttributes.Final | MemberAttributes.Public;
            return getByIdMethod;
        }

        private static string LoadNestedArgument(DomainClass domainClass)
        {
            var loadNestedListsArgument = string.Empty;
            foreach (var listProperty in domainClass.ChildHookMethods)
                loadNestedListsArgument += $".Include(entity => entity.{listProperty.OriginFieldName})";
            foreach (var childHook in domainClass.ListProperties)
                loadNestedListsArgument += $".Include(entity => entity.{childHook.Name})";

            return loadNestedListsArgument;
        }

        private static CodeMemberMethod MakeUpdateMethod(DomainClass domainClass)
        {
            var updateMethod = new CodeMemberMethod
            {
                Name = $"Update{domainClass.Name}",
                ReturnType = new CodeTypeReference("async Task")
            };
            updateMethod.Parameters.Add(new CodeParameterDeclarationExpression
            {
                Type = new CodeTypeReference(domainClass.Name),
                Name = domainClass.Name.ToLower()
            });

            updateMethod.Statements.Add(
                new CodeSnippetExpression(
                    $"EventStore.{domainClass.Name}s.Update({domainClass.Name.ToLower()})"));
            updateMethod.Statements.Add(
                new CodeSnippetExpression("await EventStore.SaveChangesAsync()"));

            updateMethod.Attributes = MemberAttributes.Final | MemberAttributes.Public;
            return updateMethod;
        }

        private static CodeMemberMethod MakeCreateMethod(DomainClass domainClass)
        {
            var createMethod = new CodeMemberMethod
            {
                Name = $"Create{domainClass.Name}",
                ReturnType = new CodeTypeReference("async Task")
            };
            createMethod.Parameters.Add(new CodeParameterDeclarationExpression
            {
                Type = new CodeTypeReference(domainClass.Name),
                Name = domainClass.Name.ToLower()
            });
            createMethod.Statements.Add(
                new CodeSnippetExpression($"EventStore.{domainClass.Name}s.Add({domainClass.Name.ToLower()})"));
            createMethod.Statements.Add(
                new CodeSnippetExpression("await EventStore.SaveChangesAsync()"));

            createMethod.Attributes = MemberAttributes.Final | MemberAttributes.Public;
            return createMethod;
        }
    }
}