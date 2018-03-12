using DslModel.Application;
using DslModel.Domain;
using DslModelToCSharp.SqlAdapter;

namespace DslModelToCSharp.Application
{
    public class WebAdapterWriter
    {
        private readonly IFileWriter _fileWriter;
        private ControllerBuilder _repositoryBuilder;

        public WebAdapterWriter(string sqlAdapterNameSpace, string basePath)
        {
            _fileWriter = new FileWriter(basePath);
            _repositoryBuilder = new ControllerBuilder(sqlAdapterNameSpace);
        }

        public void Write(DomainTree domainTree, string basePath)
        {
            foreach (var domainClass in domainTree.Classes)
            {
                var controller = _repositoryBuilder.Build(domainClass);
                _fileWriter.WriteToFile(controller.Types[0].Name, $"{domainClass.Name}s", controller);
            }

            new PrivateSetPropertyHackCleaner().ReplaceHackPropertyNames(basePath);
        }
    }
}