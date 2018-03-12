using DslModel.Application;
using DslModel.Domain;

namespace DslModelToCSharp.Application
{
    public class ApplicationWriter
    {
        private readonly IFileWriter _fileWriter;
        private HookResultBuilder _hookResultBuilder;
        private CommandHandlerBuilder _commandHandlerBuilder;
        private RepositoryInterfaceBuilder _repositoryInterfaceBuilder;

        public ApplicationWriter(string applicationNameSpace, string basePath)
        {
            _fileWriter = new FileWriter(basePath);
            _hookResultBuilder = new HookResultBuilder(applicationNameSpace);
            _commandHandlerBuilder = new CommandHandlerBuilder(applicationNameSpace);
            _repositoryInterfaceBuilder = new RepositoryInterfaceBuilder(applicationNameSpace);
        }
        public void Write(DomainTree domainTree, string basePath)
        {
            foreach (var domainClass in domainTree.Classes)
            {
                var commandHandler = _commandHandlerBuilder.Build(domainClass);
                _fileWriter.WriteToFile(commandHandler.Types[0].Name, $"{domainClass.Name}s", commandHandler);
                var iRepository = _repositoryInterfaceBuilder.Build(domainClass);
                _fileWriter.WriteToFile(iRepository.Types[0].Name, $"{domainClass.Name}s", iRepository);
            }

            var hookResult = _hookResultBuilder.Write(new HookResultBaseClass());
            _fileWriter.WriteToFile(new HookResultBaseClass().Name, "Base", hookResult);

            new PrivateSetPropertyHackCleaner().ReplaceHackPropertyNames(basePath);
        }
    }
}