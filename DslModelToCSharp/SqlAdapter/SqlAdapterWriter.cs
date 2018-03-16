using System.Linq;
using DslModel.Domain;

namespace DslModelToCSharp.SqlAdapter
{
    public class SqlAdapterWriter
    {
        private readonly IFileWriter _fileWriter;
        private RepositoryBuilder _repositoryBuilder;
        private DbContextBuilder _dbContextBuilder;

        public SqlAdapterWriter(string sqlAdapterNameSpace, string basePath)
        {
            _fileWriter = new FileWriter(basePath);
            _repositoryBuilder = new RepositoryBuilder(sqlAdapterNameSpace);
            _dbContextBuilder = new DbContextBuilder(sqlAdapterNameSpace);
        }

        public void Write(DomainTree domainTree, string basePath)
        {
            foreach (var domainClass in domainTree.Classes)
            {
                var repo = _repositoryBuilder.Build(domainClass);
                _fileWriter.WriteToFile(repo.Types[0].Name, $"{domainClass.Name}s", repo);
            }

            var dbContext = _dbContextBuilder.Build(domainTree.Classes.ToList());
            _fileWriter.WriteToFile(dbContext.Types[0].Name, "Base", dbContext);

            new PrivateSetPropertyHackCleaner().ReplaceHackPropertyNames(basePath);
        }
    }
}