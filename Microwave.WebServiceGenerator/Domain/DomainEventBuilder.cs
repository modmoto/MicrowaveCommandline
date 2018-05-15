using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using Microwave.LanguageModel;
using Microwave.WebServiceGenerator.Util;
using Microwave.WebServiceModel.Domain;

namespace Microwave.WebServiceGenerator.Domain
{
    public class DomainEventBuilder : IPlainDataObjectBuilder
    {
        private readonly DomainClass _classModell;
        private readonly DomainEvent _domainEvent;
        private readonly ClassBuilderUtil _classBuilder;
        private readonly ConstructorBuilderUtil _constructorBuilderUtil;
        private readonly PropertyBuilderUtil _propertyBuilderUtil;
        private CodeNamespace _nameSpace;
        private CodeTypeDeclaration _targetClass;

        public DomainEventBuilder(DomainClass classModell, DomainEvent domainEvent)
        {
            _classModell = classModell;
            _domainEvent = domainEvent;
            _propertyBuilderUtil = new PropertyBuilderUtil();
            _classBuilder = new ClassBuilderUtil();
            _constructorBuilderUtil = new ConstructorBuilderUtil();
        }

        private static bool IsCreateEvent(DomainEvent domainEvent)
        {
            return domainEvent.Name.StartsWith(domainEvent.Properties[0].Type + new CreateMethod().Name);
        }

        private void BuildPropertiesWithoutSelfLink(DomainEvent domainEvent)
        {
            var first = domainEvent.Properties.First();
            _targetClass = _propertyBuilderUtil.BuildWithoutSet(_targetClass, new List<Property> {first});
            var properties = domainEvent.Properties.Except(new List<Property> {first}).ToList();
            _propertyBuilderUtil.Build(_targetClass, properties);
        }

        private void BuildProperties(DomainEvent domainEvent)
        {
            _propertyBuilderUtil.Build(_targetClass, domainEvent.Properties);
        }

        public void AddNameSpace()
        {
            _nameSpace = new CodeNamespace($"Domain.{_classModell.Name}s");
            _nameSpace.Imports.Add(new CodeNamespaceImport("System"));
        }

        public void AddClassType()
        {
            _targetClass = _classBuilder.Build(_domainEvent.Name);
        }

        public void AddClassProperties()
        {
            if (!IsCreateEvent(_domainEvent))
            {
                BuildProperties(_domainEvent);
            }
            else
            {
                BuildPropertiesWithoutSelfLink(_domainEvent);
            }
        }

        public void AddConstructor()
        {
            var constructor = _constructorBuilderUtil.BuildPublicWithBaseCall(_domainEvent.Properties,
                new DomainEventBaseClass().Properties.Skip(2).ToList());
            var constructorPrivate = _constructorBuilderUtil.BuildPrivateWithEmptyGuidBaseCall(new List<Property>());
            _targetClass.Members.Add(constructorPrivate);
            _targetClass.Members.Add(constructor);
        }

        public void AddBaseTypes()
        {
            _targetClass.BaseTypes.Add(new CodeTypeReference(new DomainEventBaseClass().Name));
        }

        public CodeNamespace Build()
        {
            _nameSpace.Types.Add(_targetClass);
            return _nameSpace;
        }
    }
}