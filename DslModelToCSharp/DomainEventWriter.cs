using System.CodeDom;
using System.Collections.Generic;
using DslModel;

namespace DslModelToCSharp
{
    public interface IDomainEventWriter
    {
        void Write(DomainEvent domainEvent, string nameSpaceName);
    }

    public class DomainEventWriter : IDomainEventWriter
    {
        private readonly IClassParser _classParser;
        private readonly IConstBuilder _constBuilder;
        private readonly IFileWriter _fileWriter;
        private readonly IPropertyBuilder _propertyBuilder;

        public DomainEventWriter(IPropertyBuilder propertyBuilder, IFileWriter fileWriter, IClassParser classParser,
            IConstBuilder constBuilder)
        {
            _propertyBuilder = propertyBuilder;
            _fileWriter = fileWriter;
            _classParser = classParser;
            _constBuilder = constBuilder;
        }

        public void Write(DomainEvent domainEvent, string nameSpaceName)
        {
            var nameSpace = new CodeNamespace(nameSpaceName);

            var targetClass = _classParser.Build(domainEvent);

            nameSpace.Types.Add(targetClass);
            nameSpace.Imports.Add(new CodeNamespaceImport("System"));

            var constructor = _constBuilder.BuildPublicWithBaseCall(domainEvent.Properties, new DomainEventBaseClass().Properties);

            foreach (var proptery in domainEvent.Properties)
            {
                var autoProperty = _propertyBuilder.Build(proptery);
                targetClass.Members.Add(autoProperty.Field);
                targetClass.Members.Add(autoProperty.Property);
            }

            targetClass.BaseTypes.Add(new CodeTypeReference(new DomainEventBaseClass().Name));
            targetClass.Members.Add(constructor);

            _fileWriter.WriteToFile(domainEvent.Name, nameSpaceName.Split(".")[1], nameSpace);
        }
    }
}