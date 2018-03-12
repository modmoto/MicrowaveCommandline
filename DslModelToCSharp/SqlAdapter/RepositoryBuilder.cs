using System.CodeDom;
using System.Collections.Generic;
using DslModel.Domain;

namespace DslModelToCSharp.SqlAdapter
{
    public class RepositoryBuilder
    {
        private readonly string _nameSpace;
        private readonly NameSpaceBuilder _nameSpaceBuilder;
        private ClassBuilder _classBuilder;
        private PropBuilder _propBuilder;
        private ConstBuilder _constBuilder;

        public RepositoryBuilder(string nameSpace)
        {
            _nameSpace = nameSpace;
            _nameSpaceBuilder = new NameSpaceBuilder();
            _propBuilder = new PropBuilder();
            _constBuilder = new ConstBuilder();
            _classBuilder = new ClassBuilder();
        }

        public CodeNamespace Build(DomainClass domainClass)
        {
            var nameSpace = _nameSpaceBuilder.BuildWithEfCore($"{_nameSpace}.{domainClass.Name}s", domainClass.Name);
            var repository = _classBuilder.Build($"{domainClass.Name}Repository");

            repository.BaseTypes.Add(new CodeTypeReference($"I{domainClass.Name}Repository"));

            var properties = new List<Property> {new Property {Name = "EventStore", Type = "EventStoreContext"}};
            _propBuilder.Build(repository,
                properties);
            var codeConstructor = _constBuilder.BuildPublic(properties);
            repository.Members.Add(codeConstructor);

            var createMethod = MakeCreateMethod(domainClass);
            repository.Members.Add(createMethod);


            var updateMethod = MakeUpdateMethod(domainClass);
            repository.Members.Add(updateMethod);

            var getByIdMethod = MakeGetByIdMethod(domainClass);
            repository.Members.Add(getByIdMethod);

            var getAllMethod = MakeGetAllMethod(domainClass);
            repository.Members.Add(getAllMethod);

            nameSpace.Types.Add(repository);

            return nameSpace;
        }

        private static CodeMemberMethod MakeGetAllMethod(DomainClass domainClass)
        {
            var getAllMethod = new CodeMemberMethod
            {
                Name = $"Get{domainClass.Name}s",
                ReturnType = new CodeTypeReference($"async Task<List<{domainClass.Name}>>")
            };

            getAllMethod.Statements.Add(
                new CodeSnippetExpression($"return await EventStore.{domainClass.Name}s.ToListAsync()"));

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
                new CodeSnippetExpression($"return await EventStore.{domainClass.Name}s.FindAsync(id)"));

            getByIdMethod.Attributes = MemberAttributes.Final | MemberAttributes.Public;
            return getByIdMethod;
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
                new CodeSnippetExpression($"EventStore.{domainClass.Name}s.Update({domainClass.Name.ToLower()})"));
            updateMethod.Statements.Add(
                new CodeSnippetExpression("await EventStore.SaveChangesAsync();"));

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
                new CodeSnippetExpression("await EventStore.SaveChangesAsync();"));

            createMethod.Attributes = MemberAttributes.Final | MemberAttributes.Public;
            return createMethod;
        }
    }
}