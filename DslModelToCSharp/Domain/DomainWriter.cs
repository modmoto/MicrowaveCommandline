using DslModel.Domain;

namespace DslModelToCSharp.Domain
{
    public class DomainWriter
    {
        private readonly DomainClassWriter _classWriter;
        private readonly IDomainEventWriter _domainEventWriter;
        private readonly DomainEventBaseClassWriter _domainEventBaseClassBuilder;
        private readonly ValidationResultBaseClassBuilder _validationResultBaseClassBuilder;
        private readonly FileWriter _fileWriter;

        public DomainWriter(string domain, string basePath, string basePathSolution)
        {
            _classWriter = new DomainClassWriter(domain, basePath, basePathSolution);
            _domainEventWriter = new DomainEventWriter(basePath);
            _domainEventBaseClassBuilder = new DomainEventBaseClassWriter(domain);
            _validationResultBaseClassBuilder = new ValidationResultBaseClassBuilder(domain, basePath);
            _fileWriter = new FileWriter(basePath);
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
            var domainEventBaseClass = _domainEventBaseClassBuilder.Build(new DomainEventBaseClass().Name, new DomainEventBaseClass().Properties);
            _fileWriter.WriteToFile(new DomainEventBaseClass().Name, "Base", domainEventBaseClass);

            new PrivateSetPropertyHackCleaner().ReplaceHackPropertyNames(basePath);
        }
    }
}