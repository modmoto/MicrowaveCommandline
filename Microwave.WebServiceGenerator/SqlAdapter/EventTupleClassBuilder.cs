using System.CodeDom;
using System.Linq;
using Microwave.LanguageModel;
using Microwave.WebServiceGenerator.Util;

namespace Microwave.WebServiceGenerator.SqlAdapter
{
    public class EventTupleClassBuilder
    {
        private readonly string _nameSpace;
        private readonly ClassBuilderUtil _classBuilderUtil;
        private readonly ConstructorBuilderUtil _constructorBuilderUtil;
        private readonly NameSpaceBuilderUtil _nameSpaceBuilderUtil;
        private readonly PropertyBuilderUtil _propertyBuilderUtil;

        public EventTupleClassBuilder(string nameSpace)
        {
            _nameSpace = nameSpace;
            _nameSpaceBuilderUtil = new NameSpaceBuilderUtil();
            _constructorBuilderUtil = new ConstructorBuilderUtil();
            _classBuilderUtil = new ClassBuilderUtil();
            _propertyBuilderUtil = new PropertyBuilderUtil();
        }

        public CodeNamespace Build(DomainClass domainClasses)
        {
            var nameSpace = _nameSpaceBuilderUtil.WithName($"{_nameSpace}").Build();
            var generatedClass = _classBuilderUtil.Build(domainClasses.Name);
            _propertyBuilderUtil.Build(generatedClass, domainClasses.Properties);
            var constructor = _constructorBuilderUtil.BuildPublic(domainClasses.Properties);
            generatedClass.Members.Add(constructor);
            nameSpace.Types.Add(generatedClass);
            return nameSpace;
        }
    }
}