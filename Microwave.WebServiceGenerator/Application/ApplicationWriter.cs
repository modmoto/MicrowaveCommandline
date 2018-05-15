using System.IO;
using Microwave.LanguageModel;
using Microwave.WebServiceGenerator.Util;
using Microwave.WebServiceModel.Application;

namespace Microwave.WebServiceGenerator.Application
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
        private ApiCommandBuilder _apiCommandBuilder;
        private EventStoreInterfaceBuilder _eventStoreInterfaceBuilder;
        private HangfireQueueInterfaceBuilder _hangfireQueueInterfaceBuilder;
        private AsyncHookBuilder _asyncHookBuilder;
        private EventAndJobClassBuilder _eventAndJobClassBuilder;
        private NameBuilderUtil _nameBuilderUtil;
        private AsyncHookCreateEventHandlerBuilder _asyncHookCreateEventHandlerBuilder;
        private QueueRepositoryInterfaceBuilder _queueRepositoryInterfaceBuilder;

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
            _apiCommandBuilder = new ApiCommandBuilder();
            _eventStoreInterfaceBuilder = new EventStoreInterfaceBuilder(applicationNameSpace);
            _hangfireQueueInterfaceBuilder = new HangfireQueueInterfaceBuilder(applicationNameSpace);
            _asyncHookBuilder = new AsyncHookBuilder(applicationNameSpace);
            _eventAndJobClassBuilder = new EventAndJobClassBuilder(applicationNameSpace);
            _nameBuilderUtil = new NameBuilderUtil();
            _asyncHookCreateEventHandlerBuilder = new AsyncHookCreateEventHandlerBuilder(applicationNameSpace);
            _queueRepositoryInterfaceBuilder = new QueueRepositoryInterfaceBuilder(applicationNameSpace);
        }

        public void Write(DomainTree domainTree)
        {
            foreach (var domainClass in domainTree.Classes)
            {
                var commandHandler = _commandHandlerBuilder.Build(domainClass);
                _fileWriter.WriteToFile($"{domainClass.Name}s", commandHandler);
                var iRepository = _repositoryInterfaceBuilder.Build(domainClass);
                _fileWriter.WriteToFile($"{domainClass.Name}s", iRepository);
                foreach (var loadMethod in domainClass.LoadMethods)
                {
                    var apiCommand = _apiCommandBuilder.Build(loadMethod, domainClass);
                    _fileWriter.WriteToFile($"{domainClass.Name}s", apiCommand);
                }
            }

            foreach (var hook in domainTree.SynchronousDomainHooks)
            {
                var createdHook = _synchronousHookBuilder.Build(hook);
                _fileWriter.WriteToFile($"{hook.ClassType}s/Hooks/", createdHook);
                var formattableString = $"{_applicationBasePathRealClasses}{hook.ClassType}s/Hooks/{hook.Name}Hook.cs";
                if (!File.Exists(formattableString))
                {
                    var buildReplacementClass = _synchronousHookBuilder.BuildReplacementClass(hook);
                    _fileWriterRealClasses.WriteToFile($"{hook.ClassType}s/Hooks", buildReplacementClass, false);
                }
            }

            foreach (var hook in domainTree.OnChildHooks)
            {
                var createdHook = _synchronousHookBuilder.Build(hook);
                _fileWriter.WriteToFile($"{hook.OriginEntity}s/Hooks/", createdHook);
            }

            foreach (var hook in domainTree.AsyncDomainHooks)
            {
                var createdHookEventHandler = _asyncHookCreateEventHandlerBuilder.Build(hook);
                _fileWriter.WriteToFile($"{hook.ClassType}s/AsyncHooks/", createdHookEventHandler);

                var formattableString = $"{_applicationBasePathRealClasses}{hook.ClassType}s/AsyncHooks/{_nameBuilderUtil.AsyncEventHookName(hook)}.cs";
                if (!File.Exists(formattableString))
                {
                    var buildReplacementClass = _asyncHookBuilder.BuildReplacementClass(hook);
                    _fileWriterRealClasses.WriteToFile($"{hook.ClassType}s/AsyncHooks", buildReplacementClass, false);
                }
            }

            var hookResult = _hookResultBuilder.Write(new HookResultBaseClass());
            _fileWriter.WriteToFile("Base", hookResult);

            var hookBase = _hookBaseClassBuilder.Build(new DomainHookBaseClass());
            _fileWriter.WriteToFile("Base", hookBase);

            var eventStoreRepoInterface = _eventStoreRepositoryInterfaceBuilder.Build(new EventStoreRepositoryInterface());
            _fileWriter.WriteToFile("Base", eventStoreRepoInterface);

            var eventStore = _eventStoreBuilder.Build(new EventStore());
            _fileWriter.WriteToFile("Base", eventStore);

            var eventStoreInterface = _eventStoreInterfaceBuilder.Build(new EventStoreInterface());
            _fileWriter.WriteToFile("Base", eventStoreInterface);

            var hangfireQueue = _hangfireQueueInterfaceBuilder.Build(new HangfireQueueInterface());
            _fileWriter.WriteToFile("Base", hangfireQueue);

            var jobAndEventClass = _eventAndJobClassBuilder.Build(new EventAndJobClass());
            _fileWriter.WriteToFile("Base", jobAndEventClass);

            var queueRepo = _queueRepositoryInterfaceBuilder.Build(new QueueRepositoryInterface());
            _fileWriter.WriteToFile("Base", queueRepo);

            new PrivateSetPropertyHackCleaner().ReplaceHackPropertyNames(_basePath);
        }
    }
}