using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DslModel;

namespace DslModelToCSharp
{
    public class DomainClassWriter
    {
        private readonly IClassParser _classParser;
        private readonly IConstBuilder _constBuilder;
        private readonly string _domain;
        private readonly IDomainEventWriter _domainEventWriter;
        private readonly IFileWriter _fileWriter;
        private readonly IInterfaceBuilder _interfaceBuilder;
        private readonly IPropertyBuilder _propertyBuilder;
        private readonly IStaticConstructorBuilder _staticConstructorBuilder;

        public DomainClassWriter(IInterfaceBuilder interfaceBuilder, IPropertyBuilder propertyBuilder,
            IClassParser classParser, IDomainEventWriter domainEventWriter, IFileWriter fileWriter,
            IConstBuilder constBuilder, IStaticConstructorBuilder staticConstructorBuilder,
            string domainNameSpace)
        {
            _interfaceBuilder = interfaceBuilder;
            _propertyBuilder = propertyBuilder;
            _classParser = classParser;
            _domainEventWriter = domainEventWriter;
            _fileWriter = fileWriter;
            _constBuilder = constBuilder;
            _staticConstructorBuilder = staticConstructorBuilder;
            _domain = domainNameSpace;
        }

        public void Write(DomainClass userClass, string basePath)
        {
            var nameSpaceName = $"{_domain}.{userClass.Name}s";
            var nameSpace = new CodeNamespace(nameSpaceName);

            var iface = _interfaceBuilder.Build(userClass);
            nameSpace.Types.Add(iface);

            var targetClass = _classParser.Build(userClass);
            targetClass.BaseTypes.Add(iface.Name);

            nameSpace.Types.Add(targetClass);
            nameSpace.Imports.Add(new CodeNamespaceImport("System"));

            var constructor = _constBuilder.BuildPrivate(userClass.Properties);
            var emptyConstructor = _constBuilder.BuildPrivate(new List<Property>());

            targetClass = _propertyBuilder.Build(targetClass, userClass.Properties);

            targetClass.Members.Add(constructor);
            targetClass.Members.Add(emptyConstructor);

            foreach (var domainEvent in userClass.Events)
                _domainEventWriter.Write(domainEvent, nameSpaceName, basePath);

            _fileWriter.WriteToFile(userClass.Name, nameSpaceName.Split(".")[1], nameSpace);
        }

        public void Write(ValidationResultBaseClass userClass)
        {
            var nameSpaceName = $"{_domain}";
            var nameSpace = new CodeNamespace(nameSpaceName);

            var targetClass = new CodeTypeDeclaration(userClass.Name);
            targetClass.IsClass = true;
            targetClass.TypeAttributes = TypeAttributes.Public;

            nameSpace.Types.Add(targetClass);
            nameSpace.Imports.Add(new CodeNamespaceImport("System"));
            nameSpace.Imports.Add(new CodeNamespaceImport("System.Collections.Generic"));


            var constructor = _constBuilder.BuildPrivate(userClass.Properties);

            targetClass = _propertyBuilder.Build(targetClass, userClass.Properties);

            var buildOkResultConstructor = _staticConstructorBuilder.BuildOkResult(userClass.Properties,
                new List<Property> {userClass.Properties[1]}, userClass.Name);
            var errorResultConstructor = _staticConstructorBuilder.BuildErrorResult(userClass.Properties,
                new List<Property> {userClass.Properties[2]}, userClass.Name);

            targetClass.Members.Add(constructor);
            targetClass.Members.Add(buildOkResultConstructor);
            targetClass.Members.Add(errorResultConstructor);

            _fileWriter.WriteToFile(userClass.Name, "Base", nameSpace);
        }

        public void Write(CreationResultBaseClass userClass)
        {
            var nameSpaceName = $"{_domain}";
            var nameSpace = new CodeNamespace(nameSpaceName);

            var targetClass = new CodeTypeDeclaration(userClass.Name);
            targetClass.IsClass = true;
            targetClass.TypeAttributes = TypeAttributes.Public;
            var userClassGenericType = new CodeTypeParameter(userClass.GenericType);
            userClassGenericType.Constraints.Add(" class");
            targetClass.TypeParameters.Add(userClassGenericType);

            nameSpace.Types.Add(targetClass);
            nameSpace.Imports.Add(new CodeNamespaceImport("System"));
            nameSpace.Imports.Add(new CodeNamespaceImport("System.Collections.Generic"));


            var constructor = _constBuilder.BuildPrivate(userClass.Properties);

            targetClass = _propertyBuilder.Build(targetClass, userClass.Properties);

            var buildOkResultConstructor = _staticConstructorBuilder.BuildOkResult(userClass.Properties,
                new List<Property> {userClass.Properties[1], userClass.Properties[3]}, userClass.Name,
                userClass.GenericType, $"<{userClass.GenericType}>");
            var properties = userClass.Properties.Take(3).ToList();
            properties.Add(new Property {Name = "null"});
            var errorResultConstructor = _staticConstructorBuilder.BuildErrorResult(properties,
                new List<Property> {userClass.Properties[2]}, userClass.Name, userClass.GenericType,
                $"<{userClass.GenericType}>");

            targetClass.Members.Add(constructor);
            targetClass.Members.Add(buildOkResultConstructor);
            targetClass.Members.Add(errorResultConstructor);

            _fileWriter.WriteToFile(userClass.Name, "Base", nameSpace);
        }
    }
}