using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using Microwave.LanguageModel;
using Microwave.WebServiceGenerator.Util;
using Microwave.WebServiceModel.Domain;

namespace Microwave.WebServiceGenerator.Domain
{
    public class CreationResultBaseClassBuilder : IPlainDataObjectBuilder
    {
        private readonly PropertyBuilderUtil _propertyBuilderUtil;
        private readonly ClassBuilderUtil _classBuilder;
        private readonly ConstructorBuilderUtil _constructorBuilderUtil;
        private readonly StaticConstructorBuilder _staticConstructorBuilder;
        private readonly NameSpaceBuilderUtil _nameSpaceBuilderUtil;
        private CodeNamespace _nameSpace;
        private CodeTypeDeclaration _targetClass;
        private readonly CreationResultBaseClass _userClass;


        public CreationResultBaseClassBuilder(CreationResultBaseClass userClass)
        {
            _userClass = userClass;
            _propertyBuilderUtil = new PropertyBuilderUtil();
            _classBuilder = new ClassBuilderUtil();
            _constructorBuilderUtil = new ConstructorBuilderUtil();
            _staticConstructorBuilder = new StaticConstructorBuilder();
            _nameSpaceBuilderUtil = new NameSpaceBuilderUtil();
        }

        public CodeNamespace Build()
        {
            _nameSpace.Types.Add(_targetClass);
            return _nameSpace;
        }

        public void AddNameSpace()
        {
            _nameSpace = _nameSpaceBuilderUtil.WithName("Domain").WithList().Build();
        }

        public void AddClassType()
        {
            _targetClass = _classBuilder.Build(_userClass.Name);

            var userClassGenericType = new CodeTypeParameter(_userClass.GenericType);
            userClassGenericType.Constraints.Add(" class");
            _targetClass.TypeParameters.Add(userClassGenericType);

        }

        public void AddClassProperties()
        {
            _propertyBuilderUtil.Build(_targetClass, _userClass.Properties);
        }

        public void AddConstructor()
        {
            var constructor = _constructorBuilderUtil.BuildPrivate(_userClass.Properties);
            var buildOkResultConstructor = BuildOkConstructor(_userClass);
            var errorResultConstructor = BuildErrorConstructor(_userClass);

            _targetClass.Members.Add(constructor);
            _targetClass.Members.Add(buildOkResultConstructor);
            _targetClass.Members.Add(errorResultConstructor);
        }

        public void AddBaseTypes()
        {
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
