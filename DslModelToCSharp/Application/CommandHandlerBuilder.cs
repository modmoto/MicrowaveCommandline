using System.CodeDom;
using DslModel.Domain;
using DslModelToCSharp.Domain;

namespace DslModelToCSharp.Application
{
    public class CommandHandlerBuilder
    {
        private readonly string _nameSpace;
        private readonly ClassBuilder _classBuilder;
        private readonly CommandHandlerPropBuilder _commandHandlerPropBuilder;
        private readonly ConstBuilder _constBuilder;
        private readonly NameBuilder _nameBuilder;
        private readonly NameSpaceBuilder _nameSpaceBuilder;
        private PropBuilder _propBuilder;
        private CommandHandlerMethodBuilder _commandHandlerMethodBuilder;

        public CommandHandlerBuilder(string nameSpace)
        {
            _nameSpaceBuilder = new NameSpaceBuilder();
            _nameSpace = nameSpace;
            _constBuilder = new ConstBuilder();
            _classBuilder = new ClassBuilder();
            _commandHandlerPropBuilder = new CommandHandlerPropBuilder();
            _commandHandlerMethodBuilder = new CommandHandlerMethodBuilder();
            _propBuilder = new PropBuilder();
            _nameBuilder = new NameBuilder();
        }

        public CodeNamespace Build(DomainClass domainClass)
        {

            var nameSpace = _nameSpaceBuilder.BuildWithMvcImport($"{_nameSpace}.{domainClass.Name}s", domainClass.Name);

            var commandHandler = _classBuilder.BuildPartial(_nameBuilder.BuildCommandHandlerName(domainClass));
            var codeConstructor = _constBuilder.BuildPublic(_commandHandlerPropBuilder.Build(domainClass));
            _propBuilder.Build(commandHandler, _commandHandlerPropBuilder.Build(domainClass));
            commandHandler.Members.Add(codeConstructor);

            var allMethod = _commandHandlerMethodBuilder.BuildGetAllMethod(domainClass);
            var byIdMethod = _commandHandlerMethodBuilder.BuildGetByIdMethod(domainClass);

            commandHandler.Members.Add(allMethod);
            commandHandler.Members.Add(byIdMethod);
            foreach (var createMethod in domainClass.CreateMethods)
            {
                var method = _commandHandlerMethodBuilder.BuildCreateMethod(createMethod, domainClass);
                commandHandler.Members.Add(method);
            }

            foreach (var method in domainClass.Methods)
            {
                var methodParsed = _commandHandlerMethodBuilder.BuildUpdateMethod(method, domainClass);
                commandHandler.Members.Add(methodParsed);
            }

            nameSpace.Types.Add(commandHandler);

            return nameSpace;
        }
    }
}