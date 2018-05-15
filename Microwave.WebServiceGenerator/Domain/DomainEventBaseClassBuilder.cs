using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using Microwave.LanguageModel;
using Microwave.WebServiceGenerator.Util;
using Microwave.WebServiceModel.Domain;

namespace Microwave.WebServiceGenerator.Domain
{
    public class DomainEventBaseClassBuilder : IConcreteClassBuilder
    {
        private readonly DomainEventBaseClass _domainEventBaseClass;
        private readonly ConstructorBuilderUtil _constructorBuilderUtil;
        private readonly NameSpaceBuilderUtil _nameSpaceBuilderUtil;
        private readonly ClassBuilderUtil _classBuilder;
        private readonly PropertyBuilderUtil _propertyBuilderUtil;

        public DomainEventBaseClassBuilder(DomainEventBaseClass domainEventBaseClass)
        {
            _domainEventBaseClass = domainEventBaseClass;
            _propertyBuilderUtil = new PropertyBuilderUtil();
            _constructorBuilderUtil = new ConstructorBuilderUtil();
            _nameSpaceBuilderUtil = new NameSpaceBuilderUtil();
            _classBuilder = new ClassBuilderUtil();
        }


        public CodeNamespace BuildNameSpace()
        {
            return _nameSpaceBuilderUtil.WithName("Domain").WithList().Build();
        }

        public CodeTypeDeclaration BuildClassType()
        {
            return _classBuilder.Build(new DomainEventBaseClass().Name);
        }

        public void AddClassProperties(CodeTypeDeclaration targetClass)
        {
            _propertyBuilderUtil.Build(targetClass, _domainEventBaseClass.Properties);
        }

        public void AddConstructor(CodeTypeDeclaration targetClass)
        {
            var properties = _domainEventBaseClass.Properties;
            var constructor = _constructorBuilderUtil.BuildPublicWithIdAndTimeStampCreateInBody(properties.Skip(2).ToList(), properties[0].Name, properties[1].Name);
            var constructorPrivate = _constructorBuilderUtil.BuildPrivate(new List<Property>());
            targetClass.Members.Add(constructor);
            targetClass.Members.Add(constructorPrivate);
        }

        public void AddBaseTypes(CodeTypeDeclaration targetClass)
        {
        }

        public void AddConcreteMethods(CodeTypeDeclaration targetClass)
        {
        }
    }
}