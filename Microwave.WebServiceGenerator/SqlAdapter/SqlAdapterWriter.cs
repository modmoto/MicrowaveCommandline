using System.Linq;
using Microwave.LanguageModel;
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
        private ClassFactory _classFactory;
        private HangfireContextBuilder _hangfireContextBuilder;
        private EventTupleClassBuilder _eventTupleClassBuilder;
        private EventTupleClassBuilder _tupleClassBuilder;

        public SqlAdapterWriter(string sqlAdapterNameSpace, string basePath)
        {
            _sqlAdapterNameSpace = sqlAdapterNameSpace;
            _basePath = basePath;
            _fileWriter = new FileWriter(basePath);
            _repositoryBuilder = new RepositoryBuilder(sqlAdapterNameSpace);
            _eventStoreContextBuilder = new EventStoreContextBuilder(sqlAdapterNameSpace);
            _classFactory = new ClassFactory();
            _hangfireContextBuilder = new HangfireContextBuilder(sqlAdapterNameSpace);
            _tupleClassBuilder = new EventTupleClassBuilder(sqlAdapterNameSpace);
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

            var eventStoreRepo = _classFactory.BuildInstance(new EventStoreRepositoryBuilder(_sqlAdapterNameSpace));
            _fileWriter.WriteToFile(eventStoreRepo.Types[0].Name, "Base", eventStoreRepo);

            var eventTuple = _tupleClassBuilder.Build(new EventTupleClass());
            _fileWriter.WriteToFile(eventTuple.Types[0].Name, "Base", eventTuple);

            new PrivateSetPropertyHackCleaner().ReplaceHackPropertyNames(_basePath);
        }
    }
}