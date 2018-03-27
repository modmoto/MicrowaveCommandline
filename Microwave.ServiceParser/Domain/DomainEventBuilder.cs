using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using Microwave.LanguageModel.Domain;
using Microwave.ServiceParser.Util;

namespace Microwave.ServiceParser.Domain
{
    public class DomainEventBuilder : ICSharpClassBuilder
    {
        private readonly DomainClass _targetClass;
        private readonly DomainEvent _domainEvent;
        private readonly IClassBuilder _classBuilder;
        private readonly ConstructorBuilderUtil _constructorBuilderUtil;
        private readonly PropertyBuilderUtil _propertyBuilderUtil;

        public DomainEventBuilder(DomainClass targetClass, DomainEvent domainEvent)
        {
            _targetClass = targetClass;
            _domainEvent = domainEvent;
            _propertyBuilderUtil = new PropertyBuilderUtil();
            _classBuilder = new ClassBuilderUtil();
            _constructorBuilderUtil = new ConstructorBuilderUtil();
        }

        private static bool IsCreateEvent(DomainEvent domainEvent)
        {
            return domainEvent.Name.StartsWith(domainEvent.Properties[0].Type + new CreateMethod().Name);
        }

        private void BuildPropertiesWithoutSelfLink(DomainEvent domainEvent,
            CodeTypeDeclaration targetClass)
        {
            var first = domainEvent.Properties.First();
            targetClass = _propertyBuilderUtil.BuildWithoutSet(targetClass, new List<Property> {first});
            var properties = domainEvent.Properties.Except(new List<Property> {first}).ToList();
            _propertyBuilderUtil.Build(targetClass, properties);
        }

        private void BuildProperties(DomainEvent domainEvent, CodeTypeDeclaration targetClass)
        {
            _propertyBuilderUtil.Build(targetClass, domainEvent.Properties);
        }

        public CodeNamespace BuildNameSpace()
        {
            var nameSpace = new CodeNamespace($"Domain.{_targetClass.Name}s");
            nameSpace.Imports.Add(new CodeNamespaceImport("System"));
            return nameSpace;
        }

        public CodeTypeDeclaration BuildClassType()
        {
            var targetClass = _classBuilder.Build(_domainEvent.Name);
            return targetClass;
        }

        public void AddClassProperties(CodeTypeDeclaration targetClass)
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

        public void AddConstructor(CodeTypeDeclaration targetClass)
        {
            var constructor = _constructorBuilderUtil.BuildPublicWithBaseCall(_domainEvent.Properties,
                new DomainEventBaseClass().Properties.Skip(1).ToList());
            targetClass.Members.Add(constructor);
        }

        public void AddBaseTypes(CodeTypeDeclaration targetClass)
        {
            targetClass.BaseTypes.Add(new CodeTypeReference(new DomainEventBaseClass().Name));
        }

        public void AddConcreteMethods(CodeTypeDeclaration targetClass)
        {
        }
    }
}