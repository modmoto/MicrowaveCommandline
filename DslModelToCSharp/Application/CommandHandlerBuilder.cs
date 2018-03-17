using System.CodeDom;
using DslModel.Domain;
using DslModelToCSharp.Domain;
using DslModelToCSharp.Util;

namespace DslModelToCSharp.Application
{
    public class CommandHandlerBuilder
    {
        private readonly string _nameSpace;
        private readonly ClassBuilderUtil _classBuilderUtil;
        private readonly CommandHandlerPropBuilderUtil _commandHandlerPropBuilderUtil;
        private readonly ConstructorBuilderUtil _constructorBuilderUtil;
        private readonly NameBuilderUtil _nameBuilderUtil;
        private readonly NameSpaceBuilderUtil _nameSpaceBuilderUtil;
        private PropertyBuilderUtil _propertyBuilderUtil;
        private CommandHandlerMethodBuilderUtil _commandHandlerMethodBuilderUtil;

        public CommandHandlerBuilder(string nameSpace)
        {
            _nameSpaceBuilderUtil = new NameSpaceBuilderUtil();
            _nameSpace = nameSpace;
            _constructorBuilderUtil = new ConstructorBuilderUtil();
            _classBuilderUtil = new ClassBuilderUtil();
            _commandHandlerPropBuilderUtil = new CommandHandlerPropBuilderUtil();
            _commandHandlerMethodBuilderUtil = new CommandHandlerMethodBuilderUtil();
            _propertyBuilderUtil = new PropertyBuilderUtil();
            _nameBuilderUtil = new NameBuilderUtil();
        }

        public CodeNamespace Build(DomainClass domainClass)
        {

            var nameSpace = _nameSpaceBuilderUtil.BuildWithMvcImport($"{_nameSpace}.{domainClass.Name}s", domainClass.Name);

            var commandHandler = _classBuilderUtil.BuildPartial(_nameBuilderUtil.BuildCommandHandlerName(domainClass));
            var codeConstructor = _constructorBuilderUtil.BuildPublic(_commandHandlerPropBuilderUtil.Build(domainClass));
            _propertyBuilderUtil.Build(commandHandler, _commandHandlerPropBuilderUtil.Build(domainClass));
            commandHandler.Members.Add(codeConstructor);

            var allMethod = _commandHandlerMethodBuilderUtil.BuildGetAllMethod(domainClass);
            var byIdMethod = _commandHandlerMethodBuilderUtil.BuildGetByIdMethod(domainClass);

            commandHandler.Members.Add(allMethod);
            commandHandler.Members.Add(byIdMethod);
            foreach (var createMethod in domainClass.CreateMethods)
            {
                var method = _commandHandlerMethodBuilderUtil.BuildCreateMethod(createMethod, domainClass);
                commandHandler.Members.Add(method);
            }

            foreach (var method in domainClass.Methods)
            {
                var methodParsed = _commandHandlerMethodBuilderUtil.BuildUpdateMethod(method, domainClass);
                commandHandler.Members.Add(methodParsed);
            }

            nameSpace.Types.Add(commandHandler);

            return nameSpace;
        }
    }
}