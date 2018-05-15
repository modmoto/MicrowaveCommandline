using System.CodeDom;
using Microwave.LanguageModel;

namespace Microwave.WebServiceGenerator.Util
{
    public class DefaultClassBuilder : IPlainDataObjectBuilder
    {
        private readonly string _nameSpaceName;
        private readonly DomainClass _generatedClass;
        private readonly ClassBuilderUtil _classBuilderUtil;
        private readonly ConstructorBuilderUtil _constructorBuilderUtil;
        private readonly NameSpaceBuilderUtil _nameSpaceBuilderUtil;
        private readonly PropertyBuilderUtil _propertyBuilderUtil;
        private CodeNamespace _nameSpace;
        private CodeTypeDeclaration _targetClass;

        public DefaultClassBuilder(string nameSpaceName, DomainClass generatedClass)
        {
            _nameSpaceName = nameSpaceName;
            _generatedClass = generatedClass;
            _nameSpaceBuilderUtil = new NameSpaceBuilderUtil();
            _constructorBuilderUtil = new ConstructorBuilderUtil();
            _classBuilderUtil = new ClassBuilderUtil();
            _propertyBuilderUtil = new PropertyBuilderUtil();
        }

        public void AddNameSpace()
        {
            _nameSpace = _nameSpaceBuilderUtil.WithName($"{_nameSpaceName}").Build();
        }

        public void AddClassType()
        {
            _targetClass = _classBuilderUtil.Build(_generatedClass.Name);
        }

        public void AddClassProperties()
        {
            _propertyBuilderUtil.Build(_targetClass, _generatedClass.Properties);
        }

        public void AddConstructor()
        {
            var constructor = _constructorBuilderUtil.BuildPublic(_generatedClass.Properties);
            _targetClass.Members.Add(constructor);
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