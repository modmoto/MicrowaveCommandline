using System.CodeDom;
using System.Linq;
using Microwave.LanguageModel;
using Microwave.WebServiceGenerator.Util;

namespace Microwave.WebServiceGenerator.SqlAdapter
{
    public class EventAndJobClassBuilder
    {
        private readonly string _nameSpace;
        private readonly ClassBuilderUtil _classBuilderUtil;
        private readonly ConstructorBuilderUtil _constructorBuilderUtil;
        private readonly NameSpaceBuilderUtil _nameSpaceBuilderUtil;
        private readonly PropertyBuilderUtil _propertyBuilderUtil;

        public EventAndJobClassBuilder(string nameSpace)
        {
            _nameSpace = nameSpace;
            _nameSpaceBuilderUtil = new NameSpaceBuilderUtil();
            _constructorBuilderUtil = new ConstructorBuilderUtil();
            _classBuilderUtil = new ClassBuilderUtil();
            _propertyBuilderUtil = new PropertyBuilderUtil();
        }

        public CodeNamespace Build(DomainClass domainClasses)
        {
            var nameSpace = _nameSpaceBuilderUtil.WithName($"{_nameSpace}").WithDomain().Build();
            var generatedClass = _classBuilderUtil.Build(domainClasses.Name);
            _propertyBuilderUtil.Build(generatedClass, domainClasses.Properties);
            var constructor = _constructorBuilderUtil.BuildPublicWithIdCreateInBody(domainClasses.Properties.Skip(1).ToList(), domainClasses.Properties[0].Name);
            generatedClass.Members.Add(constructor);
            nameSpace.Types.Add(generatedClass);
            return nameSpace;
        }
    }
}