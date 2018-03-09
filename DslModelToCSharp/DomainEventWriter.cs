using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
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

            if (!domainEvent.Name.StartsWith(new CreateMethod().Name))
            {
                targetClass = _propertyBuilder.Build(targetClass, domainEvent.Properties);
            }
            else
            {
                var first = domainEvent.Properties.First();
                targetClass = _propertyBuilder.BuildWithoutSet(targetClass, new List<Property> { first });
                var properties = domainEvent.Properties.Except(new List<Property> { first }).ToList();
                targetClass = _propertyBuilder.Build(targetClass, properties);
            }

            var constructor = _constBuilder.BuildPublicWithBaseCall(domainEvent.Properties, new DomainEventBaseClass().Properties.Skip(1).ToList());

            targetClass.BaseTypes.Add(new CodeTypeReference(new DomainEventBaseClass().Name));
            targetClass.Members.Add(constructor);

            _fileWriter.WriteToFile(domainEvent.Name, nameSpaceName.Split(".")[1], nameSpace);
        }
    }
}