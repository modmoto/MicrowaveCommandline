using System.CodeDom;
using System.Collections.Generic;
using Microwave.LanguageModel;
using Microwave.WebServiceGenerator.Util;
using Microwave.WebServiceModel.Domain;

namespace Microwave.WebServiceGenerator.Domain
{
    public class ValidationResultBaseClassBuilder : IPlainDataObjectBuilder
    {
        private readonly ValidationResultBaseClass _resultBaseClass;
        private readonly ClassBuilderUtil _classBuilder;
        private readonly ConstructorBuilderUtil _constructorBuilderUtil;
        private readonly NameSpaceBuilderUtil _nameSpaceBuilderUtil;
        private readonly PropertyBuilderUtil _propertyBuilderUtil;
        private readonly StaticConstructorBuilderUtil _staticConstructorBuilder;
        private CodeNamespace _nameSpace;
        private CodeTypeDeclaration _targetClass;


        public ValidationResultBaseClassBuilder(ValidationResultBaseClass resultBaseClass)
        {
            _resultBaseClass = resultBaseClass;
            _staticConstructorBuilder = new StaticConstructorBuilderUtil();
            _propertyBuilderUtil = new PropertyBuilderUtil();
            _constructorBuilderUtil = new ConstructorBuilderUtil();
            _nameSpaceBuilderUtil = new NameSpaceBuilderUtil();
            _classBuilder = new ClassBuilderUtil();
        }

        private CodeMemberMethod BuildErrorResultConstructor(ValidationResultBaseClass userClass)
        {
            var errorResultConstructor = _staticConstructorBuilder.BuildErrorResult(
                new List<string> {$"new {userClass.Properties[1].Type}()", userClass.Properties[2].Name},
                new List<Property> {userClass.Properties[2]}, userClass.Name);
            return errorResultConstructor;
        }

        private CodeMemberMethod BuildOkResultConstructor(ValidationResultBaseClass userClass)
        {
            var buildOkResultConstructor = _staticConstructorBuilder.BuildOkResult(
                new List<string> {userClass.Properties[1].Name, $"new {userClass.Properties[2].Type}()"},
                new List<Property> {userClass.Properties[1]}, userClass.Name);
            return buildOkResultConstructor;
        }

        public void AddNameSpace()
        {
            _nameSpace = _nameSpaceBuilderUtil.WithName("Domain").WithList().Build();
        }

        public void AddClassType()
        {
            _targetClass = _classBuilder.Build(_resultBaseClass.Name);
        }

        public void AddClassProperties()
        {
            _propertyBuilderUtil.Build(_targetClass, _resultBaseClass.Properties);
        }

        public void AddConstructor()
        {
            var constructor = _constructorBuilderUtil.BuildPrivate(_resultBaseClass.Properties);

            var buildOkResultConstructor = BuildOkResultConstructor(_resultBaseClass);

            var errorResultConstructor = BuildErrorResultConstructor(_resultBaseClass);

            _targetClass.Members.Add(constructor);
            _targetClass.Members.Add(buildOkResultConstructor);
            _targetClass.Members.Add(errorResultConstructor);
        }

        public void AddBaseTypes()
        {
        }

        public CodeNamespace Build()
        {
            _nameSpace.Types.Add(_targetClass);
            return _nameSpace;
        }
    }
}