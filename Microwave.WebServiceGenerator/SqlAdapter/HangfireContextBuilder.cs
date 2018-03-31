using System.CodeDom;
using System.Collections.Generic;
using Microwave.LanguageModel;
using Microwave.WebServiceGenerator.Util;
using Microwave.WebServiceModel.Application;
using Microwave.WebServiceModel.SqlAdapter;

namespace Microwave.WebServiceGenerator.SqlAdapter
{
    public class HangfireContextBuilder
    {
        private readonly string _nameSpace;
        private readonly ClassBuilderUtil _classBuilderUtil;
        private readonly ConstructorBuilderUtil _constructorBuilderUtil;
        private readonly NameSpaceBuilderUtil _nameSpaceBuilderUtil;
        private readonly PropertyBuilderUtil _propertyBuilderUtil;
        private NameBuilderUtil _nameBuilderUtil;

        public HangfireContextBuilder(string nameSpace)
        {
            _nameSpace = nameSpace;
            _nameSpaceBuilderUtil = new NameSpaceBuilderUtil();
            _constructorBuilderUtil = new ConstructorBuilderUtil();
            _classBuilderUtil = new ClassBuilderUtil();
            _propertyBuilderUtil = new PropertyBuilderUtil();
            _nameBuilderUtil = new NameBuilderUtil();
        }

        public CodeNamespace Build(List<DomainClass> domainClasses)
        {
            _nameSpaceBuilderUtil.WithName($"{_nameSpace}").WithApplication().WithEfCore();
            foreach (var domainClass in domainClasses)
                _nameSpaceBuilderUtil.WithDomainEntityNameSpace(domainClass.Name);

            var nameSpace = _nameSpaceBuilderUtil.Build();

            var eventStore = _classBuilderUtil.Build($"HangfireContext");

            eventStore.BaseTypes.Add(new CodeTypeReference($"DbContext"));

            var properties = new List<Property>();
            properties.Add(new Property {Name = "EventAndJobQueue", Type = $"DbSet<{new EventAndJobClass().Name}>"});
            foreach (var domainClass in domainClasses)
            {
                foreach (var method in domainClass.Methods)
                    properties.Add(new Property
                    {
                        Name = $"{_nameBuilderUtil.EventName(domainClass, method)}s",
                        Type = $"DbSet<{_nameBuilderUtil.EventName(domainClass, method)}>"
                    });

                foreach (var method in domainClass.CreateMethods)
                    properties.Add(new Property
                    {
                        Name = $"{_nameBuilderUtil.EventName(domainClass, method)}s",
                        Type = $"DbSet<{_nameBuilderUtil.EventName(domainClass, method)}>"
                    });
            }

            _propertyBuilderUtil.Build(eventStore, properties);
            var codeConstructor = _constructorBuilderUtil.BuildPublicWithBaseCall(new List<Property>(),
                new List<Property> {new Property {Name = "options", Type = "DbContextOptions<HangfireContext>" } });
            eventStore.Members.Add(codeConstructor);

            nameSpace.Types.Add(eventStore);

            return nameSpace;
        }
    }
}