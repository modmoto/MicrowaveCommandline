using System.CodeDom;
using System.Collections.Generic;
using DslModel.Domain;

namespace DslModelToCSharp.SqlAdapter
{
    public class ControllerBuilder
    {
        private readonly string _nameSpace;
        private readonly NameSpaceBuilder _nameSpaceBuilder;
        private readonly ClassBuilder _classBuilder;
        private readonly ConstBuilder _constBuilder;
        private readonly PropBuilder _propBuilder;

        public ControllerBuilder(string nameSpace)
        {
            _nameSpace = nameSpace;
            _nameSpaceBuilder = new NameSpaceBuilder();
            _propBuilder = new PropBuilder();
            _constBuilder = new ConstBuilder();
            _classBuilder = new ClassBuilder();
        }

        public CodeNamespace Build(DomainClass domainClass)
        {
            var nameSpace =
                _nameSpaceBuilder.BuildWithMvcApplicationImport($"{_nameSpace}.{domainClass.Name}s", domainClass.Name);
            var repository = _classBuilder.Build($"{domainClass.Name}Controller");

            repository.BaseTypes.Add(new CodeTypeReference("Controller"));

            var properties = new List<Property>
            {
                new Property {Name = "Handler", Type = $"{domainClass.Name}CommandHandler"}
            };
            _propBuilder.Build(repository,
                properties);
            var codeConstructor = _constBuilder.BuildPublic(properties);
            repository.Members.Add(codeConstructor);

            foreach (var createM in domainClass.CreateMethods)
            {
                var createMethod = MakeCreateMethod(domainClass, createM);
                createMethod.CustomAttributes.Add(new CodeAttributeDeclaration("HttpPost"));
                repository.Members.Add(createMethod);
            }

            foreach (var domainMethod in domainClass.Methods)
            {
                var updateMethod = MakeUpdateMethod(domainClass, domainMethod);
                updateMethod.CustomAttributes.Add(new CodeAttributeDeclaration("HttpPut",
                    new CodeAttributeArgument(new CodePrimitiveExpression($"{{id}}/{domainMethod.Name.ToLower()}"))));
                repository.Members.Add(updateMethod);
            }

            var getByIdMethod = MakeGetByIdMethod(domainClass);
            getByIdMethod.CustomAttributes.Add(new CodeAttributeDeclaration("HttpGet",
                new CodeAttributeArgument(new CodePrimitiveExpression("{id}"))));
            repository.Members.Add(getByIdMethod);

            var getAllMethod = MakeGetAllMethod(domainClass);
            getAllMethod.CustomAttributes.Add(new CodeAttributeDeclaration("HttpGet"));
            repository.Members.Add(getAllMethod);

            repository.CustomAttributes.Add(new CodeAttributeDeclaration("Route",
                new CodeAttributeArgument(new CodePrimitiveExpression($"api/{domainClass.Name.ToLower()}s"))));

            nameSpace.Types.Add(repository);

            return nameSpace;
        }

        private static CodeMemberMethod MakeGetAllMethod(DomainClass domainClass)
        {
            var getAllMethod = new CodeMemberMethod
            {
                Name = $"Get{domainClass.Name}s",
                ReturnType = new CodeTypeReference($"async Task<IActionResult>")
            };

            getAllMethod.Statements.Add(
                new CodeSnippetExpression($"return await Handler.Get{domainClass.Name}s()"));

            getAllMethod.Attributes = MemberAttributes.Final | MemberAttributes.Public;
            return getAllMethod;
        }

        private static CodeMemberMethod MakeGetByIdMethod(DomainClass domainClass)
        {
            var getByIdMethod = new CodeMemberMethod
            {
                Name = $"Get{domainClass.Name}",
                ReturnType = new CodeTypeReference($"async Task<IActionResult>")
            };
            getByIdMethod.Parameters.Add(
                new CodeParameterDeclarationExpression {Type = new CodeTypeReference("Guid"), Name = "id"});

            getByIdMethod.Statements.Add(
                new CodeSnippetExpression($"return await Handler.Get{domainClass.Name}(id)"));

            getByIdMethod.Attributes = MemberAttributes.Final | MemberAttributes.Public;
            return getByIdMethod;
        }

        private static CodeMemberMethod MakeUpdateMethod(DomainClass domainClass, DomainMethod domainMethod)
        {
            var updateMethod = new CodeMemberMethod
            {
                Name = $"{domainMethod.Name}",
                ReturnType = new CodeTypeReference("async Task<IActionResult>")
            };
            updateMethod.Parameters.Add(new CodeParameterDeclarationExpression
            {
                Type = new CodeTypeReference("Guid"),
                Name = "id"
            });
            updateMethod.Parameters.Add(new CodeParameterDeclarationExpression
            {
                Type = new CodeTypeReference($"{domainClass.Name}{domainMethod.Name}Command"),
                Name = "command"
            });

            updateMethod.Statements.Add(
                new CodeSnippetExpression($"return await Handler.{domainMethod.Name}{domainClass.Name}(id, command)"));

            updateMethod.Attributes = MemberAttributes.Final | MemberAttributes.Public;
            return updateMethod;
        }

        private static CodeMemberMethod MakeCreateMethod(DomainClass domainClass, CreateMethod domainMethod)
        {
            var createMethod = new CodeMemberMethod
            {
                Name = $"Create{domainClass.Name}",
                ReturnType = new CodeTypeReference("async Task<IActionResult>")
            };
            createMethod.Parameters.Add(new CodeParameterDeclarationExpression
            {
                Type = new CodeTypeReference($"{domainClass.Name}{domainMethod.Name}Command"),
                Name = "command"
            });

            createMethod.Statements.Add(
                new CodeSnippetExpression($"return await Handler.Create{domainClass.Name}(command)"));

            createMethod.Attributes = MemberAttributes.Final | MemberAttributes.Public;
            return createMethod;
        }
    }
}