using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using Microwave.LanguageModel;
using Microwave.WebServiceGenerator.Util;

namespace Microwave.WebServiceGenerator.Domain
{
    public class DomainClassBuilder
    {
        private readonly ClassBuilderUtil _classBuilder;
        private readonly ConstructorBuilderUtil _constructorBuilderUtil;
        private readonly string _domain;
        private readonly IInterfaceBuilder _interfaceBuilder;
        private readonly NameSpaceBuilderUtil _nameSpaceBuilderUtil;
        private readonly PropertyBuilderUtil _propertyBuilderUtil;
        private readonly ListPropBuilderUtil _listPropBuilderUtil;

        public DomainClassBuilder(string domainNameSpace)
        {
            _interfaceBuilder = new InterfaceBuilderUtil();
            _propertyBuilderUtil = new PropertyBuilderUtil();
            _classBuilder = new ClassBuilderUtil();
            _constructorBuilderUtil = new ConstructorBuilderUtil();
            _nameSpaceBuilderUtil = new NameSpaceBuilderUtil();
            _domain = domainNameSpace;
            _listPropBuilderUtil = new ListPropBuilderUtil();
        }

        public CodeNamespace Build(DomainClass domainClass)
        {
            _nameSpaceBuilderUtil.WithName($"{_domain}.{domainClass.Name}s").WithList();

            foreach (var childHookMethod in domainClass.ChildHookMethods)
            {
                _nameSpaceBuilderUtil.WithDomainEntityNameSpace(childHookMethod.OriginEntity);
            }

            var nameSpace = _nameSpaceBuilderUtil.Build();

            var iface = _interfaceBuilder.BuildForCommand(domainClass);

            var targetClass = _classBuilder.BuildPartial(domainClass.Name);
            targetClass.BaseTypes.Add(iface.Name);

            nameSpace.Types.Add(iface);

            foreach (var createMethod in domainClass.CreateMethods)
            {
                var properties = createMethod.Parameters.Select(param => new Property {Name = param.Name, Type = param.Type}).ToList();
                var constructor = _constructorBuilderUtil.BuildPrivateForCreateMethod(properties, $"{domainClass.Name}{createMethod.Name}Command");
                targetClass.Members.Add(constructor);
            }

            targetClass = _listPropBuilderUtil.Build(targetClass, domainClass.ListProperties);

            foreach (var listProperty in domainClass.ListProperties)
            {
                nameSpace.Imports.Add(new CodeNamespaceImport($"Domain.{listProperty.Type}s"));
            }


            var emptyConstructor = _constructorBuilderUtil.BuildPrivate(new List<Property>());

            var propertiesWithDefaultId = domainClass.Properties;
            propertiesWithDefaultId.Add(new Property {Name = "Id", Type = "Guid"});
            _propertyBuilderUtil.Build(targetClass, propertiesWithDefaultId);
            targetClass.Members.Add(emptyConstructor);

            nameSpace.Types.Add(targetClass);

            return nameSpace;
        }
    }
}