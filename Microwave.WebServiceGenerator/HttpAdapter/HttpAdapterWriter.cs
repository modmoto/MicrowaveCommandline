using Microwave.LanguageModel;

namespace Microwave.WebServiceGenerator.HttpAdapter
{
    public class HttpAdapterWriter
    {
        private readonly string _basePath;
        private readonly IFileWriter _fileWriter;
        private ControllerBuilder _repositoryBuilder;

        public HttpAdapterWriter(string sqlAdapterNameSpace, string basePath)
        {
            _basePath = basePath;
            _fileWriter = new FileWriter(basePath);
            _repositoryBuilder = new ControllerBuilder(sqlAdapterNameSpace);
        }

        public void Write(DomainTree domainTree)
        {
            foreach (var domainClass in domainTree.Classes)
            {
                var controller = _repositoryBuilder.Build(domainClass);
                _fileWriter.WriteToFile(controller.Types[0].Name, $"{domainClass.Name}s", controller);
            }

            new PrivateSetPropertyHackCleaner().ReplaceHackPropertyNames(_basePath);
        }
    }
}