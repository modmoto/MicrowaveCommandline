using DslModel.Domain;

namespace DslModelToCSharp.Domain
{
    public class DomainWriter
    {
        private readonly DomainClassWriter _classWriter;
        private readonly IDomainEventWriter _domainEventWriter;
        private readonly ValidationResultBaseClassBuilder _validationResultBaseClassBuilder;
        private readonly FileWriter _fileWriter;
        private ClassFactory _classFactory;

        public DomainWriter(string domain, string basePath, string basePathSolution)
        {
            _classWriter = new DomainClassWriter(domain, basePath, basePathSolution);
            _domainEventWriter = new DomainEventWriter(basePath);
            _validationResultBaseClassBuilder = new ValidationResultBaseClassBuilder(domain, basePath);
            _fileWriter = new FileWriter(basePath);
            _classFactory = new ClassFactory();
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
            var domainEventBaseClass = _classFactory.BuildInstance(new DomainEventBaseClassBuilder());
            _fileWriter.WriteToFile(new DomainEventBaseClass().Name, "Base", domainEventBaseClass);

            new PrivateSetPropertyHackCleaner().ReplaceHackPropertyNames(basePath);
        }
    }
}