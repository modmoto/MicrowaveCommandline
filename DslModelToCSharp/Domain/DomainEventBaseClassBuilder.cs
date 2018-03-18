using System.CodeDom;
using System.Linq;
using DslModel.Domain;
using DslModelToCSharp.Util;

namespace DslModelToCSharp.Domain
{
    public class DomainEventBaseClassBuilder : ICSharpClassBuilder
    {
        private readonly ConstructorBuilderUtil _constructorBuilderUtil;
        private readonly NameSpaceBuilderUtil _nameSpaceBuilderUtil;
        private readonly IClassBuilder _classBuilder;
        private readonly PropertyBuilderUtil _propertyBuilderUtil;

        public DomainEventBaseClassBuilder()
        {
            _propertyBuilderUtil = new PropertyBuilderUtil();
            _constructorBuilderUtil = new ConstructorBuilderUtil();
            _nameSpaceBuilderUtil = new NameSpaceBuilderUtil();
            _classBuilder = new ClassBuilderUtil();
        }


        public CodeNamespace BuildNameSpace()
        {
            return _nameSpaceBuilderUtil.BuildWithListImport("Domain");
        }

        public CodeTypeDeclaration BuildClassType()
        {
            return _classBuilder.Build(new DomainEventBaseClass().Name);
        }

        public void AddClassProperties(CodeTypeDeclaration targetClass)
        {
            _propertyBuilderUtil.Build(targetClass, new DomainEventBaseClass().Properties);
        }

        public void AddConstructor(CodeTypeDeclaration targetClass)
        {
            var properties = new DomainEventBaseClass().Properties;
            var constructor = _constructorBuilderUtil.BuildPublicWithIdCreateInBody(properties.Skip(1).ToList(), properties[0].Name);
            targetClass.Members.Add(constructor);
        }

        public void AddBaseTypes(CodeTypeDeclaration targetClass)
        {
        }

        public void AddConcreteMethods(CodeTypeDeclaration targetClass)
        {
        }
    }
}