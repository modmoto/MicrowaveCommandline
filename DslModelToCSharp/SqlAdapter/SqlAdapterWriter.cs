using System.Linq;
using DslModel.Domain;

namespace DslModelToCSharp.SqlAdapter
{
    public class SqlAdapterWriter
    {
        private readonly string _sqlAdapterNameSpace;
        private readonly string _basePath;
        private readonly IFileWriter _fileWriter;
        private RepositoryBuilder _repositoryBuilder;
        private DbContextBuilder _dbContextBuilder;
        private ClassFactory _classFactory;

        public SqlAdapterWriter(string sqlAdapterNameSpace, string basePath)
        {
            _sqlAdapterNameSpace = sqlAdapterNameSpace;
            _basePath = basePath;
            _fileWriter = new FileWriter(basePath);
            _repositoryBuilder = new RepositoryBuilder(sqlAdapterNameSpace);
            _dbContextBuilder = new DbContextBuilder(sqlAdapterNameSpace);
            _classFactory = new ClassFactory();
        }

        public void Write(DomainTree domainTree)
        {
            foreach (var domainClass in domainTree.Classes)
            {
                var repo = _repositoryBuilder.Build(domainClass);
                _fileWriter.WriteToFile(repo.Types[0].Name, $"{domainClass.Name}s", repo);
            }

            var dbContext = _dbContextBuilder.Build(domainTree.Classes.ToList());
            _fileWriter.WriteToFile(dbContext.Types[0].Name, "Base", dbContext);

            var eventStoreRepo = _classFactory.BuildInstance(new EventStoreRepositoryBuilder(_sqlAdapterNameSpace));
            _fileWriter.WriteToFile(eventStoreRepo.Types[0].Name, "Base", eventStoreRepo);
            new PrivateSetPropertyHackCleaner().ReplaceHackPropertyNames(_basePath);
        }
    }
}