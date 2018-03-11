using System.CodeDom;
using System.Reflection;
using DslModel.Domain;

namespace DslModelToCSharp.Application
{
    public class IRepositoryBuilder
    {
        private readonly string _nameSpace;
        private readonly NameSpaceBuilder _nameSpaceBuilder;

        public IRepositoryBuilder(string nameSpace)
        {
            _nameSpace = nameSpace;
            _nameSpaceBuilder = new NameSpaceBuilder();
        }

        public CodeNamespace Build(DomainClass domainClass)
        {
            var nameSpace = _nameSpaceBuilder.BuildWithTask($"{_nameSpace}.{domainClass.Name}s", domainClass.Name);
            var iface = new CodeTypeDeclaration($"I{domainClass.Name}Repository") {IsInterface = true};

            var createMethod = new CodeMemberMethod
            {
                Name = $"Create{domainClass.Name}",
                ReturnType = new CodeTypeReference("Task")
            };
            createMethod.Parameters.Add(new CodeParameterDeclarationExpression {Type = new CodeTypeReference(domainClass.Name), Name = domainClass.Name.ToLower()});
            iface.Members.Add(createMethod);


            var updateMethod = new CodeMemberMethod
            {
                Name = $"Update{domainClass.Name}",
                ReturnType = new CodeTypeReference("Task")
            };
            updateMethod.Parameters.Add(new CodeParameterDeclarationExpression {Type = new CodeTypeReference(domainClass.Name), Name = domainClass.Name.ToLower()});
            iface.Members.Add(updateMethod);

            var getByIdMethod = new CodeMemberMethod
            {
                Name = $"Get{domainClass.Name}",
                ReturnType = new CodeTypeReference($"Task<{domainClass.Name}>")
            };
            getByIdMethod.Parameters.Add(new CodeParameterDeclarationExpression {Type = new CodeTypeReference("Guid"), Name = "id"});
            iface.Members.Add(getByIdMethod);

            var getAllMethod = new CodeMemberMethod
            {
                Name = $"Get{domainClass.Name}s",
                ReturnType = new CodeTypeReference($"Task<List<{domainClass.Name}>>")
            };
            iface.Members.Add(getAllMethod);
            nameSpace.Types.Add(iface);

            return nameSpace;
        }
    }
}