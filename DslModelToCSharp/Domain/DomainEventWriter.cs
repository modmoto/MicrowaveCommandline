using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using DslModel.Domain;

namespace DslModelToCSharp
{
    public interface IDomainEventWriter
    {
        void Write(DomainEvent domainEvent, string nameSpaceName);
    }

    public class DomainEventWriter : IDomainEventWriter
    {
        private readonly IClassBuilder _classBuilder;
        private readonly ConstBuilder _constBuilder;
        private readonly IFileWriter _fileWriter;
        private readonly PropBuilder _propertyBuilder;

        public DomainEventWriter(string basePath)
        {
            _propertyBuilder = new PropBuilder();
            _fileWriter = new FileWriter(basePath);
            _classBuilder = new ClassBuilder();
            _constBuilder = new ConstBuilder();
        }

        public void Write(DomainEvent domainEvent, string nameSpaceName)
        {
            var nameSpace = new CodeNamespace(nameSpaceName);

            var targetClass = _classBuilder.Build(domainEvent.Name);

            nameSpace.Types.Add(targetClass);
            nameSpace.Imports.Add(new CodeNamespaceImport("System"));

            targetClass = !IsCreateEvent(domainEvent)
                ? BuildProperties(domainEvent, targetClass)
                : BuildPropertiesWithoutSelfLink(domainEvent, targetClass);

            var constructor = _constBuilder.BuildPublicWithBaseCall(domainEvent.Properties,
                new DomainEventBaseClass().Properties.Skip(1).ToList());

            targetClass.BaseTypes.Add(new CodeTypeReference(new DomainEventBaseClass().Name));
            targetClass.Members.Add(constructor);

            _fileWriter.WriteToFile(domainEvent.Name, nameSpaceName.Split(".")[1], nameSpace);
        }

        private static bool IsCreateEvent(DomainEvent domainEvent)
        {
            return domainEvent.Name.StartsWith(domainEvent.Properties[0].Type + new CreateMethod().Name);
        }

        private CodeTypeDeclaration BuildPropertiesWithoutSelfLink(DomainEvent domainEvent,
            CodeTypeDeclaration targetClass)
        {
            var first = domainEvent.Properties.First();
            targetClass = _propertyBuilder.BuildWithoutSet(targetClass, new List<Property> {first});
            var properties = domainEvent.Properties.Except(new List<Property> {first}).ToList();
            _propertyBuilder.Build(targetClass, properties);
            return targetClass;
        }

        private CodeTypeDeclaration BuildProperties(DomainEvent domainEvent, CodeTypeDeclaration targetClass)
        {
            _propertyBuilder.Build(targetClass, domainEvent.Properties);
            return targetClass;
        }
    }
}