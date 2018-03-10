using DslModel.Application;
using DslModel.Domain;

namespace DslModelToCSharp.Application
{
    public class ApplicationWriter
    {
        private readonly IFileWriter _fileWriter;
        private readonly CommandBuilder _commandBuilder;
        private HookResultBuilder _hookResultBuilder;

        public ApplicationWriter(string basePath, string applicationNameSpace)
        {
            _fileWriter = new FileWriter(basePath);
            _commandBuilder = new CommandBuilder();
            _hookResultBuilder = new HookResultBuilder(applicationNameSpace);
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

            var hookResult = _hookResultBuilder.Write(new HookResultBaseClass());
            _fileWriter.WriteToFile(new HookResultBaseClass().Name, "Base", hookResult);

            new PrivateSetPropertyHackCleaner().ReplaceHackPropertyNames(basePath);
        }
    }
}