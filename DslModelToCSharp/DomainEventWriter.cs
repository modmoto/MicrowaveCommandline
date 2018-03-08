using System.CodeDom;
using DslModel;

namespace DslModelToCSharp
{
    public interface IDomainEventWriter
    {
        void Write(DomainEvent domainEvent, string nameSpaceName, string basePath);
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

        public void Write(DomainEvent domainEvent, string nameSpaceName, string basePath)
        {
            var nameSpace = new CodeNamespace(nameSpaceName);

            var targetClass = _classParser.Build(domainEvent);

            nameSpace.Types.Add(targetClass);
            nameSpace.Imports.Add(new CodeNamespaceImport("System"));

            var constructor = _constBuilder.BuildPublicWithBaseCall(domainEvent.Properties, new DomainEventBaseClass().Properties);

            targetClass = _propertyBuilder.Build(targetClass, domainEvent.Properties);

            targetClass.BaseTypes.Add(new CodeTypeReference(new DomainEventBaseClass().Name));
            targetClass.Members.Add(constructor);

            _fileWriter.WriteToFile(domainEvent.Name, nameSpaceName.Split(".")[1], nameSpace);
        }
    }
}