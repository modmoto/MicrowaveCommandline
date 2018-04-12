using Microwave.LanguageModel;
using Microwave.WebServiceModel.Domain;

namespace Microwave.WebServiceGenerator.Domain
{
    public class DomainWriter
    {
        private readonly DomainClassBuilder _classBuilder;
        private readonly ValidationResultBaseClassBuilder _validationResultBaseClassBuilder;
        private readonly FileWriter _fileWriter;
        private readonly ClassFactory _classFactory;
        private readonly CreationResultBaseClassBuilder _creationResultBaseClassBuilder;

        public DomainWriter(string domain, string basePath, string basePathSolution)
        {
            _classBuilder = new DomainClassBuilder(domain, basePath, basePathSolution);
            _validationResultBaseClassBuilder = new ValidationResultBaseClassBuilder(domain, basePath);
            _fileWriter = new FileWriter(basePath);
            _classFactory = new ClassFactory();
            _creationResultBaseClassBuilder = new CreationResultBaseClassBuilder(domain);
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
                var domainClassBuild = _classBuilder.Build(domainClass);
                _fileWriter.WriteToFile(domainClassBuild.Types[0].Name, $"{domainClass.Name}s", domainClassBuild);
            }

           var validationResult =  _validationResultBaseClassBuilder.Build(new ValidationResultBaseClass());
            _fileWriter.WriteToFile(new ValidationResultBaseClass().Name, "Base", validationResult);

            var codeNamespace = _creationResultBaseClassBuilder.Build(new CreationResultBaseClass());
            _fileWriter.WriteToFile(new CreationResultBaseClass().Name, "Base", codeNamespace);

            var domainEventBaseClass = _classFactory.BuildInstance(new DomainEventBaseClassBuilder());
            _fileWriter.WriteToFile(new DomainEventBaseClass().Name, "Base", domainEventBaseClass);

            new PrivateSetPropertyHackCleaner().ReplaceHackPropertyNames(basePath);
        }
    }
}