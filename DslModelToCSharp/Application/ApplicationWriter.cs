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
        private SynchronousHookBuilder _synchronousHookBuilder;
        private HookBaseClassBuilder _hookBaseClassBuilder;
        private EventStoreRepositoryInterfaceBuilder _eventStoreRepositoryInterfaceBuilder;

        public ApplicationWriter(string applicationNameSpace, string basePath)
        {
            _fileWriter = new FileWriter(basePath);
            _hookResultBuilder = new HookResultBuilder(applicationNameSpace);
            _commandHandlerBuilder = new CommandHandlerBuilder(applicationNameSpace);
            _repositoryInterfaceBuilder = new RepositoryInterfaceBuilder(applicationNameSpace);
            _synchronousHookBuilder = new SynchronousHookBuilder(applicationNameSpace);
            _hookBaseClassBuilder = new HookBaseClassBuilder(applicationNameSpace);
            _eventStoreRepositoryInterfaceBuilder = new EventStoreRepositoryInterfaceBuilder(applicationNameSpace);
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

            foreach (var hook in domainTree.SynchronousDomainHooks)
            {
                var createdHook = _synchronousHookBuilder.Build(hook);
                _fileWriter.WriteToFile($"{hook.Name}Hook", $"{hook.ClassType}s/Hooks/", createdHook);
            }

            var hookResult = _hookResultBuilder.Write(new HookResultBaseClass());
            _fileWriter.WriteToFile(new HookResultBaseClass().Name, "Base", hookResult);

            var hookBase = _hookBaseClassBuilder.Build(new DomainHookBaseClass());
            _fileWriter.WriteToFile(new DomainHookBaseClass().Name, "Base", hookBase);

            var eventStoreRepoInterface = _eventStoreRepositoryInterfaceBuilder.Build(new EventStoreRepositoryInterface());
            _fileWriter.WriteToFile(new EventStoreRepositoryInterface().Name, "Base", eventStoreRepoInterface);

            new PrivateSetPropertyHackCleaner().ReplaceHackPropertyNames(basePath);
        }
    }
}