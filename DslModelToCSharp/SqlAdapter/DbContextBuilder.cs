using System.CodeDom;
using System.Collections.Generic;
using DslModel.Domain;
using DslModelToCSharp.Application;
using DslModelToCSharp.Domain;

namespace DslModelToCSharp.SqlAdapter
{
    public class DbContextBuilder
    {
        private readonly string _nameSpace;
        private readonly ClassBuilder _classBuilder;
        private readonly ConstBuilder _constBuilder;
        private readonly NameSpaceBuilder _nameSpaceBuilder;
        private readonly PropBuilder _propBuilder;
        private NameBuilder _nameBuilder;

        public DbContextBuilder(string nameSpace)
        {
            _nameSpace = nameSpace;
            _nameSpaceBuilder = new NameSpaceBuilder();
            _constBuilder = new ConstBuilder();
            _classBuilder = new ClassBuilder();
            _propBuilder = new PropBuilder();
            _nameBuilder = new NameBuilder();
        }

        public CodeNamespace Build(List<DomainClass> domainClasses)
        {
            var nameSpace = _nameSpaceBuilder.BuildWithDbImport($"{_nameSpace}");
            foreach (var domainClass in domainClasses)
                nameSpace.Imports.Add(new CodeNamespaceImport($"Domain.{domainClass.Name}s"));

            var eventStore = _classBuilder.Build($"EventStoreContext");

            eventStore.BaseTypes.Add(new CodeTypeReference($"DbContext"));

            var properties = new List<Property>();
            properties.Add(new Property {Name = "EventHistory", Type = $"DbSet<{new DomainEventBaseClass().Name}>"});
            foreach (var domainClass in domainClasses)
            {
                properties.Add(new Property {Name = $"{domainClass.Name}s", Type = $"DbSet<{domainClass.Name}>"});
                foreach (var method in domainClass.Methods)
                    properties.Add(new Property
                    {
                        Name = $"{_nameBuilder.EventName(domainClass, method)}s",
                        Type = $"DbSet<{_nameBuilder.EventName(domainClass, method)}>"
                    });

                foreach (var method in domainClass.CreateMethods)
                    properties.Add(new Property
                    {
                        Name = $"{_nameBuilder.EventName(domainClass, method)}s",
                        Type = $"DbSet<{_nameBuilder.EventName(domainClass, method)}>"
                    });
            }

            _propBuilder.Build(eventStore, properties);
            var codeConstructor = _constBuilder.BuildPublicWithBaseCall(new List<Property>(),
                new List<Property> {new Property {Name = "options", Type = "DbContextOptions<EventStoreContext>"}});
            eventStore.Members.Add(codeConstructor);

            nameSpace.Types.Add(eventStore);

            return nameSpace;
        }
    }
}