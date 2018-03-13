using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using DslModel.Domain;
using DslModelToCSharp.Domain;

namespace DslModelToCSharp
{
    public class DomainClassWriter
    {
        private readonly IClassBuilder _classBuilder;
        private readonly ConstBuilder _constBuilder;
        private readonly string _domain;
        private readonly IFileWriter _fileWriter;
        private readonly IInterfaceBuilder _interfaceBuilder;
        private readonly INameSpaceBuilder _nameSpaceBuilder;
        private readonly IPropertyBuilder _propertyBuilder;
        private readonly IStaticConstructorBuilder _staticConstructorBuilder;
        private CommandBuilder _commandBuilder;
        private ListPropBuilder _listPropBuilder;

        public DomainClassWriter(string domainNameSpace, string basePath)
        {
            _interfaceBuilder = new InterfaceBuilder();
            _propertyBuilder = new PropBuilder();
            _classBuilder = new ClassBuilder();
            _fileWriter = new FileWriter(basePath);
            _constBuilder = new ConstBuilder();
            _staticConstructorBuilder = new StaticConstructorBuilder();
            _nameSpaceBuilder = new NameSpaceBuilder();
            _domain = domainNameSpace;
            _commandBuilder = new CommandBuilder();
            _listPropBuilder = new ListPropBuilder();
        }

        public void Write(DomainClass domainClass)
        {
            var nameSpace = _nameSpaceBuilder.BuildWithListImport($"{_domain}.{domainClass.Name}s");
            var iface = _interfaceBuilder.Build(domainClass);

            var targetClass = _classBuilder.BuildPartial(domainClass.Name);
            targetClass.BaseTypes.Add(iface.Name);

            nameSpace.Types.Add(iface);

            foreach (var createMethod in domainClass.CreateMethods)
            {
                var properties = createMethod.Parameters.Select(param => new Property {Name = param.Name, Type = param.Type}).ToList();
                var constructor = _constBuilder.BuildPrivateWithAdditionalId(properties);
                targetClass.Members.Add(constructor);
            }

            targetClass = _listPropBuilder.Build(targetClass, domainClass.ListProperties);

            foreach (var listProperty in domainClass.ListProperties)
            {
                nameSpace.Imports.Add(new CodeNamespaceImport($"Domain.{listProperty.Type}s"));
            }

            var commands = _commandBuilder.Build(domainClass);

            foreach (var command in commands)
            {
                _fileWriter.WriteToFile(command.Types[0].Name, $"{domainClass.Name}s/Commands", command);
            }

            var emptyConstructor = _constBuilder.BuildPrivate(new List<Property>());

            targetClass = _propertyBuilder.Build(targetClass, domainClass.Properties);
            targetClass.Members.Add(emptyConstructor);

            nameSpace.Types.Add(targetClass);

            _fileWriter.WriteToFile(domainClass.Name, nameSpace.Name.Split(".")[1], nameSpace);
        }

        public void Write(CreationResultBaseClass userClass)
        {
            var nameSpace = _nameSpaceBuilder.BuildWithListImport(_domain);

            var targetClass = _classBuilder.Build(userClass.Name);

            var userClassGenericType = new CodeTypeParameter(userClass.GenericType);
            userClassGenericType.Constraints.Add(" class");
            targetClass.TypeParameters.Add(userClassGenericType);

            var constructor = _constBuilder.BuildPrivate(userClass.Properties);

            targetClass = _propertyBuilder.Build(targetClass, userClass.Properties);

            var buildOkResultConstructor = BuildOkConstructor(userClass);
            var errorResultConstructor = BuildErrorConstructor(userClass);

            targetClass.Members.Add(constructor);
            targetClass.Members.Add(buildOkResultConstructor);
            targetClass.Members.Add(errorResultConstructor);

            nameSpace.Types.Add(targetClass);

            _fileWriter.WriteToFile(userClass.Name, "Base", nameSpace);
        }

        private CodeMemberMethod BuildErrorConstructor(CreationResultBaseClass userClass)
        {
            var properties = userClass.Properties.Take(3).ToList();
            properties.Add(new Property {Name = "null"});
            var errorResultConstructor = _staticConstructorBuilder.BuildErrorResultGeneric(new List<string>
                {
                    $"new {userClass.Properties[1].Type}()",
                    userClass.Properties[2].Name,
                    "null"
                },
                new List<Property> { userClass.Properties[2] }, userClass.Name,
                userClass.GenericType);
            return errorResultConstructor;
        }

        private CodeMemberMethod BuildOkConstructor(CreationResultBaseClass userClass)
        {
            var buildOkResultConstructor = _staticConstructorBuilder.BuildOkResultGeneric(
                new List<string>
                {
                    userClass.Properties[1].Name,
                    $"new {userClass.Properties[2].Type}()",
                    userClass.Properties[3].Name
                },
                new List<Property> {userClass.Properties[1], userClass.Properties[3]}, userClass.Name,
                userClass.GenericType);
            return buildOkResultConstructor;
        }
    }
}