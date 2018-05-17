using System.IO;
using Microwave.LanguageModel;
using Microwave.WebServiceModel.Domain;

namespace Microwave.WebServiceGenerator.Domain
{
    public class DomainWriter
    {
        private readonly string _basePathRealClasses;
        private readonly FileWriter _fileWriter;
        private readonly ClassBuilderDirector _classBuilderDirector;
        private readonly CommandBuilder _commandBuilder;
        private readonly FileWriter _fileWriterOneTime;

        public DomainWriter(string basePath, string basePathRealClasses)
        {
            _basePathRealClasses = basePathRealClasses;
            _fileWriter = new FileWriter(basePath);
            _fileWriterOneTime = new FileWriter(basePathRealClasses);
            _classBuilderDirector = new ClassBuilderDirector();
            _commandBuilder = new CommandBuilder();
        }

        public void Write(DomainTree domainTree, string basePath)
        {
            foreach (var domainClass in domainTree.Classes)
            {
                foreach (var domainEvent in domainClass.Events)
                {
                    var domainEventClass = _classBuilderDirector.BuildInstance(new DomainEventBuilder(domainClass, domainEvent));
                    _fileWriter.WriteToFile($"{domainClass.Name}s", domainEventClass);
                }

                var domainClassBuild = _classBuilderDirector.BuildInstance(new DomainClassBuilder(domainClass));
                _fileWriter.WriteToFile($"{domainClass.Name}s", domainClassBuild);

                var domainClassFirst = _classBuilderDirector.BuildInstance(new DomainClassFirstBuilder(domainClass));
                if (!ClassIsAllreadyExisting(domainClass))
                {
                    _fileWriterOneTime.WriteToFile("", domainClassFirst, false);
                }

                var commands = _commandBuilder.Build(domainClass);
                foreach (var command in commands)
                {
                    _fileWriter.WriteToFile($"{domainClass.Name}s/Commands", command);
                }
            }

            var validationResult =  _classBuilderDirector.BuildInstance(new ValidationResultBaseClassBuilder(new ValidationResultBaseClass()));
            _fileWriter.WriteToFile("Base", validationResult);

            var codeNamespace = _classBuilderDirector.BuildInstance(new CreationResultBaseClassBuilder(new CreationResultBaseClass()));
            _fileWriter.WriteToFile("Base", codeNamespace);

            var domainEventBaseClass = _classBuilderDirector.BuildInstance(new DomainEventBaseClassBuilder(new DomainEventBaseClass()));
            _fileWriter.WriteToFile("Base", domainEventBaseClass);

            new PrivateSetPropertyHackCleaner().ReplaceHackPropertyNames(basePath);
        }


        private bool ClassIsAllreadyExisting(DomainClass domainClass)
        {
            var formattableString = $"{_basePathRealClasses}/{domainClass.Name}.cs";
            return File.Exists(formattableString);
        }
    }
}