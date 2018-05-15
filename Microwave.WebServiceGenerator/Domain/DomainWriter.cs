using System.IO;
using Microwave.LanguageModel;
using Microwave.WebServiceModel.Domain;

namespace Microwave.WebServiceGenerator.Domain
{
    public class DomainWriter
    {
        private readonly string _basePathRealClasses;
        private readonly DomainClassBuilder _classBuilder;
        private readonly FileWriter _fileWriter;
        private readonly ClassBuilderDirector _classBuilderDirector;
        private readonly CreationResultBaseClassBuilder _creationResultBaseClassBuilder;
        private CommandBuilder _commandBuilder;
        private FileWriter _fileWriterOneTime;
        private DomainClassFirstBuilder _domainClassFirstBuilder;

        public DomainWriter(string domain, string basePath, string basePathRealClasses)
        {
            _basePathRealClasses = basePathRealClasses;
            _classBuilder = new DomainClassBuilder(domain);
            _fileWriter = new FileWriter(basePath);
            _fileWriterOneTime = new FileWriter(basePathRealClasses);
            _classBuilderDirector = new ClassBuilderDirector();
            _creationResultBaseClassBuilder = new CreationResultBaseClassBuilder(domain);
            _commandBuilder = new CommandBuilder();
            _domainClassFirstBuilder = new DomainClassFirstBuilder(domain);
        }

        public void Write(DomainTree domainTree, string basePath)
        {
            foreach (var domainClass in domainTree.Classes)
            {
                foreach (var domainEvent in domainClass.Events)
                {
                    var domainEventClass = _classBuilderDirector.BuildInstance(new DomainEventBuilder(domainClass, domainEvent));
                    _fileWriter.WriteToFile(domainEventClass.Types[0].Name, $"{domainClass.Name}s", domainEventClass);
                }

                var domainClassBuild = _classBuilder.Build(domainClass);
                _fileWriter.WriteToFile(domainClassBuild.Types[1].Name, $"{domainClass.Name}s", domainClassBuild);

                var domainClassFirst = _domainClassFirstBuilder.Build(domainClass);
                if (!ClassIsAllreadyExisting(domainClass))
                {
                    _fileWriterOneTime.WriteToFile(domainClassFirst.Types[0].Name, "", domainClassFirst, false);
                }

                var commands = _commandBuilder.Build(domainClass);
                foreach (var command in commands)
                {
                    _fileWriter.WriteToFile(command.Types[0].Name, $"{domainClass.Name}s/Commands", command);
                }
            }

            var validationResult =  _classBuilderDirector.BuildInstance(new ValidationResultBaseClassBuilder(new ValidationResultBaseClass()));
            _fileWriter.WriteToFile(new ValidationResultBaseClass().Name, "Base", validationResult);

            var codeNamespace = _creationResultBaseClassBuilder.Build(new CreationResultBaseClass());
            _fileWriter.WriteToFile(new CreationResultBaseClass().Name, "Base", codeNamespace);

            var domainEventBaseClass = _classBuilderDirector.BuildInstance(new DomainEventBaseClassBuilder(new DomainEventBaseClass()));
            _fileWriter.WriteToFile(new DomainEventBaseClass().Name, "Base", domainEventBaseClass);

            new PrivateSetPropertyHackCleaner().ReplaceHackPropertyNames(basePath);
        }


        private bool ClassIsAllreadyExisting(DomainClass domainClass)
        {
            var formattableString = $"{_basePathRealClasses}/{domainClass.Name}.cs";
            return File.Exists(formattableString);
        }
    }
}