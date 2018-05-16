using System.CodeDom;
using System.Linq;
using Microwave.LanguageModel;
using Microwave.WebServiceGenerator.Util;

namespace Microwave.WebServiceGenerator.Application
{
    public class ApiCommandBuilder
    {
        private readonly ConstructorBuilderUtil _constructorBuilderUtil;
        private readonly PropertyBuilderUtil _propertyBuilderUtil;
        private readonly NameSpaceBuilderUtil _nameSpaceBuilderUtil;
        private readonly NameBuilderUtil _nameBuilderUtil;
        private readonly ClassBuilderUtil _classBuilderUtil;

        public ApiCommandBuilder()
        {
            _constructorBuilderUtil = new ConstructorBuilderUtil();
            _propertyBuilderUtil = new PropertyBuilderUtil();
            _nameSpaceBuilderUtil = new NameSpaceBuilderUtil();
            _nameBuilderUtil = new NameBuilderUtil();
            _classBuilderUtil = new ClassBuilderUtil();
        }

        public CodeNamespace Build(DomainMethod method, DomainClass domainClass)
        {
            var commandNameSpace = _nameSpaceBuilderUtil.WithName($"Application.{domainClass.Name}s").WithList().Build();
            var commandName = _nameBuilderUtil.UpdateApiCommandName(domainClass, method);
            var properties = method.Parameters.Select(param => new Property { Name = param.Name, Type = param.Type }).ToList();
            var loadProperties = method.LoadParameters.Select(param => new Property { Name = $"{param.Name}Id", Type = "Guid" }).ToList();
            properties.AddRange(loadProperties);
            var command = _classBuilderUtil.Build(commandName);
            var codeConstructor = _constructorBuilderUtil.BuildPublic(properties);
            _propertyBuilderUtil.Build(command, properties);
            command.Members.Add(codeConstructor);
            commandNameSpace.Types.Add(command);
            return commandNameSpace;
        }
    }
}