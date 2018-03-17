using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using DslModel.Domain;

namespace DslModelToCSharp.Domain
{
    public class DomainEventBuilder : CSharpClassBuilder
    {
        private readonly DomainClass _targetClass;
        private readonly DomainEvent _domainEvent;
        private readonly IClassBuilder _classBuilder;
        private readonly ConstBuilder _constBuilder;
        private readonly PropBuilder _propertyBuilder;

        public DomainEventBuilder(DomainClass targetClass, DomainEvent domainEvent)
        {
            _targetClass = targetClass;
            _domainEvent = domainEvent;
            _propertyBuilder = new PropBuilder();
            _classBuilder = new ClassBuilder();
            _constBuilder = new ConstBuilder();
        }

        private static bool IsCreateEvent(DomainEvent domainEvent)
        {
            return domainEvent.Name.StartsWith(domainEvent.Properties[0].Type + new CreateMethod().Name);
        }

        private void BuildPropertiesWithoutSelfLink(DomainEvent domainEvent,
            CodeTypeDeclaration targetClass)
        {
            var first = domainEvent.Properties.First();
            targetClass = _propertyBuilder.BuildWithoutSet(targetClass, new List<Property> {first});
            var properties = domainEvent.Properties.Except(new List<Property> {first}).ToList();
            _propertyBuilder.Build(targetClass, properties);
        }

        private void BuildProperties(DomainEvent domainEvent, CodeTypeDeclaration targetClass)
        {
            _propertyBuilder.Build(targetClass, domainEvent.Properties);
        }

        public override CodeNamespace BuildNameSpace()
        {
            var nameSpace = new CodeNamespace($"Domain.{_targetClass.Name}s");
            nameSpace.Imports.Add(new CodeNamespaceImport("System"));
            return nameSpace;
        }

        public override CodeTypeDeclaration BuildClassType()
        {
            var targetClass = _classBuilder.Build(_domainEvent.Name);

            return targetClass;
        }

        public override void AddClassProperties(CodeTypeDeclaration targetClass)
        {
            if (!IsCreateEvent(_domainEvent))
            {
                BuildProperties(_domainEvent, targetClass);
            }
            else
            {
                BuildPropertiesWithoutSelfLink(_domainEvent, targetClass);
            }
        }

        public override void AddConstructor(CodeTypeDeclaration targetClass)
        {
            var constructor = _constBuilder.BuildPublicWithBaseCall(_domainEvent.Properties,
                new DomainEventBaseClass().Properties.Skip(1).ToList());
            targetClass.Members.Add(constructor);
        }

        public override void AddBaseTypes(CodeTypeDeclaration targetClass)
        {
            targetClass.BaseTypes.Add(new CodeTypeReference(new DomainEventBaseClass().Name));
        }
    }
}