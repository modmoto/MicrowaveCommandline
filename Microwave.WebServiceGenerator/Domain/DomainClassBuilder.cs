using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using Microwave.LanguageModel;
using Microwave.WebServiceGenerator.Util;

namespace Microwave.WebServiceGenerator.Domain
{
    public class DomainClassBuilder : IConcreteClassBuilder
    {
        private readonly ClassBuilderUtil _classBuilder;
        private readonly ConstructorBuilderUtil _constructorBuilderUtil;
        private readonly DomainClass _domainClass;
        private readonly InterfaceBuilderUtil _interfaceBuilder;
        private readonly NameSpaceBuilderUtil _nameSpaceBuilderUtil;
        private readonly PropertyBuilderUtil _propertyBuilderUtil;
        private readonly ListPropBuilderUtil _listPropBuilderUtil;
        private CodeNamespace _nameSpace;
        private CodeTypeDeclaration _targetClass;
        private CodeTypeDeclaration _baseClass;
        private readonly NameBuilderUtil _nameBuilderUtil;

        public DomainClassBuilder(DomainClass domainClass)
        {
            _interfaceBuilder = new InterfaceBuilderUtil();
            _propertyBuilderUtil = new PropertyBuilderUtil();
            _classBuilder = new ClassBuilderUtil();
            _constructorBuilderUtil = new ConstructorBuilderUtil();
            _nameSpaceBuilderUtil = new NameSpaceBuilderUtil();
            _domainClass = domainClass;
            _listPropBuilderUtil = new ListPropBuilderUtil();
            _nameBuilderUtil = new NameBuilderUtil();
        }

        public CodeNamespace Build()
        {
            _nameSpace.Types.Add(_targetClass);
            _nameSpace.Types.Add(_baseClass);

            return _nameSpace;
        }

        public void AddNameSpace()
        {
            _nameSpaceBuilderUtil.WithName($"Domain.{_domainClass.Name}s").WithList();

            foreach (var childHookMethod in _domainClass.ChildHookMethods)
            {
                var domainClassName = _nameBuilderUtil.GetClassName(childHookMethod, _domainClass.Properties, _domainClass.ListProperties);
                _nameSpaceBuilderUtil.WithDomainEntityNameSpace(domainClassName);
            }

            foreach (var listProperty in _domainClass.ListProperties)
            {
                _nameSpaceBuilderUtil.WithDomainEntityNameSpace(listProperty.Type);
            }

            _nameSpace = _nameSpaceBuilderUtil.Build();
        }

        public void AddClassType()
        {
            _targetClass = _classBuilder.BuildPartial(_domainClass.Name);
            _baseClass = _interfaceBuilder.BuildForCommand(_domainClass);
        }

        public void AddClassProperties()
        {
            _listPropBuilderUtil.Build(_targetClass, _domainClass.ListProperties);

            var propertiesWithDefaultId = _domainClass.Properties;
            propertiesWithDefaultId.Add(new Property {Name = "Id", Type = "Guid"});
            _propertyBuilderUtil.Build(_targetClass, propertiesWithDefaultId);
        }

        public void AddConstructor()
        {
            foreach (var createMethod in _domainClass.CreateMethods)
            {
                var properties = createMethod.Parameters.Select(param => new Property {Name = param.Name, Type = param.Type}).ToList();
                var constructor = _constructorBuilderUtil.BuildPrivateForCreateMethod(properties, $"{_domainClass.Name}{createMethod.Name}Command");
                _targetClass.Members.Add(constructor);
            }

            var emptyConstructor = _constructorBuilderUtil.BuildPrivate(new List<Property>());
            _targetClass.Members.Add(emptyConstructor);
        }

        public void AddBaseTypes()
        {
            _targetClass.BaseTypes.Add(_baseClass.Name);
        }

        public void AddConcreteMethods()
        {
        }
    }
}