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
        private readonly RepositoryBuilder _repositoryBuilder;
        private readonly EventStoreContextBuilder _eventStoreContextBuilder;
        private readonly ClassBuilderDirector _classBuilderDirector;
        private readonly HangfireContextBuilder _hangfireContextBuilder;
        private readonly EventJobRegistrationClassBuilder _eventJobRegistrationClassBuilder;
        private readonly HangfireQueueBuilder _hangfireQueueBuilder;
        private readonly QueueRepositoryBuilder _queueRepositoryBuilder;

        public SqlAdapterWriter(string sqlAdapterNameSpace, string basePath)
        {
            _sqlAdapterNameSpace = sqlAdapterNameSpace;
            _basePath = basePath;
            _fileWriter = new FileWriter(basePath);
            _repositoryBuilder = new RepositoryBuilder(sqlAdapterNameSpace);
            _eventStoreContextBuilder = new EventStoreContextBuilder(sqlAdapterNameSpace);
            _classBuilderDirector = new ClassBuilderDirector();
            _hangfireContextBuilder = new HangfireContextBuilder(sqlAdapterNameSpace);
            _eventJobRegistrationClassBuilder = new EventJobRegistrationClassBuilder(sqlAdapterNameSpace);
            _hangfireQueueBuilder = new HangfireQueueBuilder(sqlAdapterNameSpace);
            _queueRepositoryBuilder = new QueueRepositoryBuilder(sqlAdapterNameSpace);
        }

        public void Write(DomainTree domainTree)
        {
            foreach (var domainClass in domainTree.Classes)
            {
                var repo = _repositoryBuilder.Build(domainClass);
                _fileWriter.WriteToFile($"{domainClass.Name}s", repo);
            }

            var dbContext = _eventStoreContextBuilder.Build(domainTree.Classes.ToList());
            _fileWriter.WriteToFile("Base", dbContext);

            var hangfireContext = _hangfireContextBuilder.Build(domainTree.Classes.ToList());
            _fileWriter.WriteToFile("Base", hangfireContext);

            var eventStoreRepo = _classBuilderDirector.BuildInstance(new EventStoreRepositoryBuilder(new EventStoreRepository()));
            _fileWriter.WriteToFile("Base", eventStoreRepo);

            var eventTuple = _classBuilderDirector.BuildInstance(new DefaultClassBuilder(_sqlAdapterNameSpace, new EventTupleClass()));
            _fileWriter.WriteToFile("Base", eventTuple);

            var eventJobRegistration = _eventJobRegistrationClassBuilder.Build(domainTree.AsyncDomainHooks);
            _fileWriter.WriteToFile("Base", eventJobRegistration);

            var hangfireQueue = _hangfireQueueBuilder.Build(new HangfireQueueClass());
            _fileWriter.WriteToFile("Base", hangfireQueue);

            var queueRepo = _queueRepositoryBuilder.Build(new QueueRepositoryClass());
            _fileWriter.WriteToFile("Base", queueRepo);

            new PrivateSetPropertyHackCleaner().ReplaceHackPropertyNames(_basePath);
        }
    }
}