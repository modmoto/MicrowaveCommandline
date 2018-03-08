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
        private readonly INameSpaceBuilder _nameSpaceBuilder;

        public DomainClassWriter(IInterfaceBuilder interfaceBuilder, IPropertyBuilder propertyBuilder,
            IClassParser classParser, IDomainEventWriter domainEventWriter, IFileWriter fileWriter,
            IConstBuilder constBuilder, IStaticConstructorBuilder staticConstructorBuilder, INameSpaceBuilder nameSpaceBuilder,
            string domainNameSpace)
        {
            _interfaceBuilder = interfaceBuilder;
            _propertyBuilder = propertyBuilder;
            _classParser = classParser;
            _domainEventWriter = domainEventWriter;
            _fileWriter = fileWriter;
            _constBuilder = constBuilder;
            _staticConstructorBuilder = staticConstructorBuilder;
            _nameSpaceBuilder = nameSpaceBuilder;
            _domain = domainNameSpace;
        }

        public void Write(DomainClass userClass, string basePath)
        {
            var nameSpace = _nameSpaceBuilder.Build($"{_domain}.{userClass.Name}s");

            var iface = _interfaceBuilder.Build(userClass);

            var targetClass = _classParser.Build(userClass);
            targetClass.BaseTypes.Add(iface.Name);

            nameSpace.Types.Add(iface);

            var constructor = _constBuilder.BuildPrivate(userClass.Properties);
            var emptyConstructor = _constBuilder.BuildPrivate(new List<Property>());

            targetClass = _propertyBuilder.Build(targetClass, userClass.Properties);

            targetClass.Members.Add(constructor);
            targetClass.Members.Add(emptyConstructor);

            foreach (var domainEvent in userClass.Events)
                _domainEventWriter.Write(domainEvent, nameSpace.Name, basePath);

            nameSpace.Types.Add(targetClass);

            _fileWriter.WriteToFile(userClass.Name, nameSpace.Name.Split(".")[1], nameSpace);
        }

        public void Write(ValidationResultBaseClass userClass)
        {
            var targetClass = new CodeTypeDeclaration(userClass.Name);
            targetClass.IsClass = true;
            targetClass.TypeAttributes = TypeAttributes.Public;

            var nameSpace = _nameSpaceBuilder.BuildWithListImport(_domain);

            var constructor = _constBuilder.BuildPrivate(userClass.Properties);

            targetClass = _propertyBuilder.Build(targetClass, userClass.Properties);

            var buildOkResultConstructor = _staticConstructorBuilder.BuildOkResult(userClass.Properties,
                new List<Property> {userClass.Properties[1]}, userClass.Name);
            var errorResultConstructor = _staticConstructorBuilder.BuildErrorResult(userClass.Properties,
                new List<Property> {userClass.Properties[2]}, userClass.Name);

            targetClass.Members.Add(constructor);
            targetClass.Members.Add(buildOkResultConstructor);
            targetClass.Members.Add(errorResultConstructor);

            nameSpace.Types.Add(targetClass);

            _fileWriter.WriteToFile(userClass.Name, "Base", nameSpace);
        }

        public void Write(CreationResultBaseClass userClass)
        {
            var nameSpace = _nameSpaceBuilder.BuildWithListImport(_domain);

            var targetClass = new CodeTypeDeclaration(userClass.Name);
            targetClass.IsClass = true;
            targetClass.TypeAttributes = TypeAttributes.Public;
            var userClassGenericType = new CodeTypeParameter(userClass.GenericType);
            userClassGenericType.Constraints.Add(" class");
            targetClass.TypeParameters.Add(userClassGenericType);

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

            nameSpace.Types.Add(targetClass);

            _fileWriter.WriteToFile(userClass.Name, "Base", nameSpace);
        }
    }
}