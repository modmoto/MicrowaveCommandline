using System.CodeDom;
using DslModel;

namespace DslModelToCSharp
{
    public interface IDomainEventWriter
    {
        void Write(DomainEvent domainEvent, string nameSpaceName);
    }

    public class DomainEventWriter : IDomainEventWriter
    {
        private readonly IClassBuilder _classBuilder;
        private readonly IConstBuilder _constBuilder;
        private readonly IFileWriter _fileWriter;
        private readonly IPropertyBuilder _propertyBuilder;

        public DomainEventWriter(IPropertyBuilder propertyBuilder, IFileWriter fileWriter, IClassBuilder classBuilder,
            IConstBuilder constBuilder)
        {
            _propertyBuilder = propertyBuilder;
            _fileWriter = fileWriter;
            _classBuilder = classBuilder;
            _constBuilder = constBuilder;
        }

        public void Write(DomainEvent domainEvent, string nameSpaceName)
        {
            var nameSpace = new CodeNamespace(nameSpaceName);

            var targetClass = _classBuilder.Build(domainEvent.Name);

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