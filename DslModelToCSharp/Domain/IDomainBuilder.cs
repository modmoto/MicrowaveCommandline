using DslModel.Domain;

namespace DslModelToCSharp
{
    public interface IDomainBuilder
    {
        void Build(DomainTree domainTree, string domainNameSpace, string basePath);
    }

    public class DomainBuilder : IDomainBuilder
    {
        private readonly DomainClassWriter _classWriter;
        private readonly IDomainEventWriter _domainEventWriter;
        private readonly DomainEventBaseClassWriter _domainEventBaseClassBuilder;
        private readonly ValidationResultBaseClassBuilder _validationResultBaseClassBuilder;

        public DomainBuilder(DomainClassWriter classWriter, IDomainEventWriter domainEventWriter,
            DomainEventBaseClassWriter domainEventBaseClassBuilder, ValidationResultBaseClassBuilder validationResultBaseClassBuilder)
        {
            _classWriter = classWriter;
            _domainEventWriter = domainEventWriter;
            _domainEventBaseClassBuilder = domainEventBaseClassBuilder;
            _validationResultBaseClassBuilder = validationResultBaseClassBuilder;
        }

        public void Build(DomainTree domainTree, string domainNameSpace, string basePath)
        {
            foreach (var domainClass in domainTree.Classes)
            {
                foreach (var domainEvent in domainClass.Events)
                    _domainEventWriter.Write(domainEvent, $"{domainNameSpace}.{domainClass.Name}s");
                _classWriter.Write(domainClass);
            }

            _validationResultBaseClassBuilder.Write(new ValidationResultBaseClass());
            _classWriter.Write(new CreationResultBaseClass());
            _domainEventBaseClassBuilder.Build(new DomainEventBaseClass().Name, new DomainEventBaseClass().Properties);

            new PrivateSetPropertyHackCleaner().ReplaceHackPropertyNames(basePath);
        }
    }
}