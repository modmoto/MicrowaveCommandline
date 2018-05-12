using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using Microwave.LanguageModel;
using Microwave.WebServiceGenerator.Util;
using Microwave.WebServiceModel.Domain;

namespace Microwave.WebServiceGenerator.Domain
{
    public class CreationResultBaseClassBuilder
    {
        private readonly PropertyBuilderUtil _propertyBuilderUtil;
        private ClassBuilderUtil _classBuilder;
        private ConstructorBuilderUtil _constructorBuilderUtil;
        private StaticConstructorBuilder _staticConstructorBuilder;
        private NameSpaceBuilderUtil _nameSpaceBuilderUtil;
        private string _domain;

        public CreationResultBaseClassBuilder(string domainNameSpace)
        {
            _propertyBuilderUtil = new PropertyBuilderUtil();
            _classBuilder = new ClassBuilderUtil();
            _constructorBuilderUtil = new ConstructorBuilderUtil();
            _staticConstructorBuilder = new StaticConstructorBuilder();
            _nameSpaceBuilderUtil = new NameSpaceBuilderUtil();
            _domain = domainNameSpace;
        }

        public CodeNamespace Build(CreationResultBaseClass userClass)
        {
            var nameSpace = _nameSpaceBuilderUtil.WithName(_domain).WithList().Build();

            var targetClass = _classBuilder.Build(userClass.Name);

            var userClassGenericType = new CodeTypeParameter(userClass.GenericType);
            userClassGenericType.Constraints.Add(" class");
            targetClass.TypeParameters.Add(userClassGenericType);

            var constructor = _constructorBuilderUtil.BuildPrivate(userClass.Properties);

            _propertyBuilderUtil.Build(targetClass, userClass.Properties);

            var buildOkResultConstructor = BuildOkConstructor(userClass);
            var errorResultConstructor = BuildErrorConstructor(userClass);

            targetClass.Members.Add(constructor);
            targetClass.Members.Add(buildOkResultConstructor);
            targetClass.Members.Add(errorResultConstructor);

            nameSpace.Types.Add(targetClass);
            return nameSpace;
        }

        private CodeMemberMethod BuildErrorConstructor(CreationResultBaseClass userClass)
        {
            var properties = userClass.Properties.Take(3).ToList();
            properties.Add(new Property { Name = "null" });
            var errorResultConstructor = _staticConstructorBuilder.BuildErrorResultGeneric(new List<string>
                {
                    $"new {userClass.Properties[1].Type}()",
                    userClass.Properties[2].Name,
                    "null"
                },
                new List<Property> { userClass.Properties[2] }, userClass.Name,
                userClass.GenericType);
            return errorResultConstructor;
        }

        private CodeMemberMethod BuildOkConstructor(CreationResultBaseClass userClass)
        {
            var buildOkResultConstructor = _staticConstructorBuilder.BuildOkResultGeneric(
                new List<string>
                {
                    userClass.Properties[1].Name,
                    $"new {userClass.Properties[2].Type}()",
                    userClass.Properties[3].Name
                },
                new List<Property> { userClass.Properties[1], userClass.Properties[3] }, userClass.Name,
                userClass.GenericType);
            return buildOkResultConstructor;
        }
    }
}
