using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using DslModel.Domain;

namespace DslModelToCSharp.Domain
{
    public class DomainEventBaseClassBuilder : CSharpClassBuilder
    {
        private readonly ConstBuilder _constBuilder;
        private readonly NameSpaceBuilder _nameSpaceBuilder;
        private readonly IClassBuilder _classBuilder;
        private readonly PropBuilder _propertyBuilder;

        public DomainEventBaseClassBuilder()
        {
            _propertyBuilder = new PropBuilder();
            _constBuilder = new ConstBuilder();
            _nameSpaceBuilder = new NameSpaceBuilder();
            _classBuilder = new ClassBuilder();
        }


        public override CodeNamespace BuildNameSpace()
        {
            return _nameSpaceBuilder.BuildWithListImport("Domain");
        }

        public override CodeTypeDeclaration BuildClassType()
        {
            return _classBuilder.Build(new DomainEventBaseClass().Name);
        }

        public override void AddClassProperties(CodeTypeDeclaration targetClass)
        {
            _propertyBuilder.Build(targetClass, new DomainEventBaseClass().Properties);
        }

        public override void AddConstructor(CodeTypeDeclaration targetClass)
        {
            var properties = new DomainEventBaseClass().Properties;
            var constructor = _constBuilder.BuildPublicWithIdCreateInBody(properties.Skip(1).ToList(), properties[0].Name);
            targetClass.Members.Add(constructor);
        }
    }
}