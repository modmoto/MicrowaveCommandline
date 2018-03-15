using DslModel.Domain;
using DslModelToCSharp.Application;

namespace DslModelToCSharp
{
    public interface IDomainWriter
    {
        void Build(DomainTree domainTree, string domainNameSpace, string basePath);
    }

    public class DomainWriter : IDomainWriter
    {
        private readonly DomainClassWriter _classWriter;
        private readonly IDomainEventWriter _domainEventWriter;
        private readonly DomainEventBaseClassWriter _domainEventBaseClassBuilder;
        private readonly ValidationResultBaseClassBuilder _validationResultBaseClassBuilder;

        public DomainWriter(string domain, string basePath, string basePathSolution)
        {
            _classWriter = new DomainClassWriter(domain, basePath, basePathSolution);
            _domainEventWriter = new DomainEventWriter(basePath);
            _domainEventBaseClassBuilder = new DomainEventBaseClassWriter(domain, basePath);
            _validationResultBaseClassBuilder = new ValidationResultBaseClassBuilder(domain, basePath);
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