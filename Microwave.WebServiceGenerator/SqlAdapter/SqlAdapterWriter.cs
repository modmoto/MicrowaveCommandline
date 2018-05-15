using System.Linq;
using Microwave.LanguageModel;
using Microwave.WebServiceGenerator.Util;
using Microwave.WebServiceModel.SqlAdapter;

namespace Microwave.WebServiceGenerator.SqlAdapter
{
    public class SqlAdapterWriter
    {
        private readonly string _sqlAdapterNameSpace;
        private readonly string _basePath;
        private readonly IFileWriter _fileWriter;
        private RepositoryBuilder _repositoryBuilder;
        private EventStoreContextBuilder _eventStoreContextBuilder;
        private ClassBuilderDirector _classBuilderDirector;
        private HangfireContextBuilder _hangfireContextBuilder;
        private DefaultClassBuilder _defaultClassBuilder;
        private DefaultClassBuilder _tupleClassBuilder;
        private EventJobRegistrationClassBuilder _eventJobRegistrationClassBuilder;
        private HangfireQueueBuilder _hangfireQueueBuilder;
        private QueueRepositoryBuilder _queueRepositoryBuilder;

        public SqlAdapterWriter(string sqlAdapterNameSpace, string basePath)
        {
            _sqlAdapterNameSpace = sqlAdapterNameSpace;
            _basePath = basePath;
            _fileWriter = new FileWriter(basePath);
            _repositoryBuilder = new RepositoryBuilder(sqlAdapterNameSpace);
            _eventStoreContextBuilder = new EventStoreContextBuilder(sqlAdapterNameSpace);
            _classBuilderDirector = new ClassBuilderDirector();
            _hangfireContextBuilder = new HangfireContextBuilder(sqlAdapterNameSpace);
            _tupleClassBuilder = new DefaultClassBuilder(sqlAdapterNameSpace, new EventTupleClass());
            _eventJobRegistrationClassBuilder = new EventJobRegistrationClassBuilder(sqlAdapterNameSpace);
            _hangfireQueueBuilder = new HangfireQueueBuilder(sqlAdapterNameSpace);
            _queueRepositoryBuilder = new QueueRepositoryBuilder(sqlAdapterNameSpace);
        }

        public void Write(DomainTree domainTree)
        {
            foreach (var domainClass in domainTree.Classes)
            {
                var repo = _repositoryBuilder.Build(domainClass);
                _fileWriter.WriteToFile(repo.Types[0].Name, $"{domainClass.Name}s", repo);
            }

            var dbContext = _eventStoreContextBuilder.Build(domainTree.Classes.ToList());
            _fileWriter.WriteToFile(dbContext.Types[0].Name, "Base", dbContext);

            var hangfireContext = _hangfireContextBuilder.Build(domainTree.Classes.ToList());
            _fileWriter.WriteToFile(hangfireContext.Types[0].Name, "Base", hangfireContext);

            var eventStoreRepo = _classBuilderDirector.BuildInstance(new EventStoreRepositoryBuilder(new EventStoreRepository()));
            _fileWriter.WriteToFile(eventStoreRepo.Types[0].Name, "Base", eventStoreRepo);

            var eventTuple = _classBuilderDirector.BuildInstance(new DefaultClassBuilder(_sqlAdapterNameSpace, new EventTupleClass()));
            _fileWriter.WriteToFile(eventTuple.Types[0].Name, "Base", eventTuple);

            var eventJobRegistration = _eventJobRegistrationClassBuilder.Build(domainTree.AsyncDomainHooks);
            _fileWriter.WriteToFile(eventJobRegistration.Types[0].Name, "Base", eventJobRegistration);

            var hangfireQueue = _hangfireQueueBuilder.Build(new HangfireQueueClass());
            _fileWriter.WriteToFile(hangfireQueue.Types[0].Name, "Base", hangfireQueue);

            var queueRepo = _queueRepositoryBuilder.Build(new QueueRepositoryClass());
            _fileWriter.WriteToFile(queueRepo.Types[0].Name, "Base", queueRepo);

            new PrivateSetPropertyHackCleaner().ReplaceHackPropertyNames(_basePath);
        }
    }
}