using DslModel.Domain;

namespace DslModelToCSharp.Application
{
    public class ApplicationWriter
    {
        private readonly IFileWriter _fileWriter;
        private readonly CommandBuilder _commandBuilder;

        public ApplicationWriter(IFileWriter fileWriter)
        {
            _fileWriter = fileWriter;
            _commandBuilder = new CommandBuilder();
        }
        public void Write(DomainTree domainTree, string basePath)
        {
            foreach (var domainClass in domainTree.Classes)
            {
                var commands = _commandBuilder.Build(domainClass);

                foreach (var command in commands)
                {
                    _fileWriter.WriteToFile(command.Types[0].Name, $"{domainClass.Name}s/Commands", command);
                }
            }

            new PrivateSetPropertyHackCleaner().ReplaceHackPropertyNames(basePath);
        }
    }
}