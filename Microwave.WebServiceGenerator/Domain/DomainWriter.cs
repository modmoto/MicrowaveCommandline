using Microwave.LanguageModel;
using Microwave.WebServiceModel.Domain;

namespace Microwave.WebServiceGenerator.Domain
{
    public class DomainWriter
    {
        private readonly DomainClassWriter _classWriter;
        private readonly ValidationResultBaseClassBuilder _validationResultBaseClassBuilder;
        private readonly FileWriter _fileWriter;
        private ClassFactory _classFactory;

        public DomainWriter(string domain, string basePath, string basePathSolution)
        {
            _classWriter = new DomainClassWriter(domain, basePath, basePathSolution);
            _validationResultBaseClassBuilder = new ValidationResultBaseClassBuilder(domain, basePath);
            _fileWriter = new FileWriter(basePath);
            _classFactory = new ClassFactory();
        }

        public void Build(DomainTree domainTree, string basePath)
        {
            foreach (var domainClass in domainTree.Classes)
            {
                foreach (var domainEvent in domainClass.Events)
                {
                    var domainEventClass = _classFactory.BuildInstance(new DomainEventBuilder(domainClass, domainEvent));
                    _fileWriter.WriteToFile(domainEventClass.Types[0].Name, $"{domainClass.Name}s", domainEventClass);
                }
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