using DslModel.Application;
using DslModel.Domain;
using DslModelToCSharp.SqlAdapter;

namespace DslModelToCSharp.Application
{
    public class SqlAdapterWriter
    {
        private readonly IFileWriter _fileWriter;
        private RepositoryBuilder _repositoryBuilder;

        public SqlAdapterWriter(string sqlAdapterNameSpace, string basePath)
        {
            _fileWriter = new FileWriter(basePath);
            _repositoryBuilder = new RepositoryBuilder(sqlAdapterNameSpace);
        }

        public void Write(DomainTree domainTree, string basePath)
        {
            foreach (var domainClass in domainTree.Classes)
            {
                var repo = _repositoryBuilder.Build(domainClass);
                _fileWriter.WriteToFile(repo.Types[0].Name, $"{domainClass.Name}s", repo);
            }

            new PrivateSetPropertyHackCleaner().ReplaceHackPropertyNames(basePath);
        }
    }
}