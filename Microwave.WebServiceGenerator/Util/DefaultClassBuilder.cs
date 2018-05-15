using System.CodeDom;
using Microwave.LanguageModel;

namespace Microwave.WebServiceGenerator.Util
{
    public class DefaultClassBuilder : IPlainDataObjectBuilder
    {
        private readonly string _nameSpace;
        private readonly DomainClass _generatedClass;
        private readonly ClassBuilderUtil _classBuilderUtil;
        private readonly ConstructorBuilderUtil _constructorBuilderUtil;
        private readonly NameSpaceBuilderUtil _nameSpaceBuilderUtil;
        private readonly PropertyBuilderUtil _propertyBuilderUtil;

        public DefaultClassBuilder(string nameSpace, DomainClass generatedClass)
        {
            _nameSpace = nameSpace;
            _generatedClass = generatedClass;
            _nameSpaceBuilderUtil = new NameSpaceBuilderUtil();
            _constructorBuilderUtil = new ConstructorBuilderUtil();
            _classBuilderUtil = new ClassBuilderUtil();
            _propertyBuilderUtil = new PropertyBuilderUtil();
        }

        public virtual CodeNamespace BuildNameSpace()
        {
            return _nameSpaceBuilderUtil.WithName($"{_nameSpace}").Build();
        }

        public virtual CodeTypeDeclaration BuildClassType()
        {
            return _classBuilderUtil.Build(_generatedClass.Name);
        }

        public virtual void AddClassProperties(CodeTypeDeclaration targetClass)
        {
            _propertyBuilderUtil.Build(targetClass, _generatedClass.Properties);
        }

        public virtual void AddConstructor(CodeTypeDeclaration targetClass)
        {
            var constructor = _constructorBuilderUtil.BuildPublic(_generatedClass.Properties);
            targetClass.Members.Add(constructor);
        }

        public virtual void AddBaseTypes(CodeTypeDeclaration targetClass)
        {
        }
    }
}