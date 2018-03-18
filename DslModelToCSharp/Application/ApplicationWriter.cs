using System.IO;
using DslModel.Application;
using DslModel.Domain;

namespace DslModelToCSharp.Application
{
    public class ApplicationWriter
    {
        private readonly string _basePath;
        private readonly string _applicationBasePathRealClasses;
        private readonly IFileWriter _fileWriter;
        private HookResultBuilder _hookResultBuilder;
        private CommandHandlerBuilder _commandHandlerBuilder;
        private RepositoryInterfaceBuilder _repositoryInterfaceBuilder;
        private SynchronousHookBuilder _synchronousHookBuilder;
        private HookBaseClassBuilder _hookBaseClassBuilder;
        private EventStoreRepositoryInterfaceBuilder _eventStoreRepositoryInterfaceBuilder;
        private FileWriter _fileWriterRealClasses;
        private EventStoreBuilder _eventStoreBuilder;

        public ApplicationWriter(string applicationNameSpace, string basePath,string applicationBasePathRealClasses)
        {
            _basePath = basePath;
            _applicationBasePathRealClasses = applicationBasePathRealClasses;
            _fileWriterRealClasses = new FileWriter(_applicationBasePathRealClasses);
            _fileWriter = new FileWriter(basePath);
            _hookResultBuilder = new HookResultBuilder(applicationNameSpace);
            _commandHandlerBuilder = new CommandHandlerBuilder(applicationNameSpace);
            _repositoryInterfaceBuilder = new RepositoryInterfaceBuilder(applicationNameSpace);
            _synchronousHookBuilder = new SynchronousHookBuilder(applicationNameSpace);
            _hookBaseClassBuilder = new HookBaseClassBuilder(applicationNameSpace);
            _eventStoreRepositoryInterfaceBuilder = new EventStoreRepositoryInterfaceBuilder(applicationNameSpace);
            _eventStoreBuilder = new EventStoreBuilder(applicationNameSpace);
        }

        public void Write(DomainTree domainTree)
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
                var formattableString = $"{_applicationBasePathRealClasses}{hook.ClassType}s/{hook.Name}Hook.cs";
                if (!File.Exists(formattableString))
                {
                    var buildReplacementClass = _synchronousHookBuilder.BuildReplacementClass(hook);
                    _fileWriterRealClasses.WriteToFile($"{hook.Name}Hook", $"{hook.ClassType}s/", buildReplacementClass, false);
                }
            }

            var hookResult = _hookResultBuilder.Write(new HookResultBaseClass());
            _fileWriter.WriteToFile(new HookResultBaseClass().Name, "Base", hookResult);

            var hookBase = _hookBaseClassBuilder.Build(new DomainHookBaseClass());
            _fileWriter.WriteToFile(new DomainHookBaseClass().Name, "Base", hookBase);

            var eventStoreRepoInterface = _eventStoreRepositoryInterfaceBuilder.Build(new EventStoreRepositoryInterface());
            _fileWriter.WriteToFile(new EventStoreRepositoryInterface().Name, "Base", eventStoreRepoInterface);

            var eventStoreRepo= _eventStoreBuilder.Build(new EventStore(), domainTree.SynchronousDomainHooks);
            _fileWriter.WriteToFile(new EventStore().Name, "Base", eventStoreRepo);

            new PrivateSetPropertyHackCleaner().ReplaceHackPropertyNames(_basePath);
        }
    }
}