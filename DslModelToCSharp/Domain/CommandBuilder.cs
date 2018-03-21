using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using DslModel.Domain;
using DslModelToCSharp.Util;

namespace DslModelToCSharp.Domain
{
    public class CommandBuilder
    {
        private readonly ClassBuilderUtil _classBuilderUtil;
        private readonly ConstructorBuilderUtil _constructorBuilderUtil;
        private readonly NameSpaceBuilderUtil _nameSpaceBuilderUtil;
        private PropertyBuilderUtil _propertyBuilderUtil;
        private NameBuilderUtil _nameBuilderUtil;

        public CommandBuilder()
        {
            _classBuilderUtil = new ClassBuilderUtil();
            _constructorBuilderUtil = new ConstructorBuilderUtil();
            _nameSpaceBuilderUtil = new NameSpaceBuilderUtil();
            _propertyBuilderUtil = new PropertyBuilderUtil();
            _nameBuilderUtil = new NameBuilderUtil();
        }

        public List<CodeNamespace> Build(DomainClass domainClass)
        {
            var commandList = new List<CodeNamespace>();
            foreach (var method in domainClass.Methods)
            {
                var commandNameSpace = _nameSpaceBuilderUtil.WithName($"Domain.{domainClass.Name}s").WithList().Build();
                var commandName = _nameBuilderUtil.UpdateCommandName(domainClass, method);
                var properties = method.Parameters.Select(param => new Property {Name = param.Name, Type = param.Type}).ToList();
                var loadProperties = method.LoadParameters.Select(param => new Property {Name = $"{param.Type}Id", Type = "Guid"}).ToList();
                properties.AddRange(loadProperties);
                var command = _classBuilderUtil.Build(commandName);
                var codeConstructor = _constructorBuilderUtil.BuildPublic(properties);
                _propertyBuilderUtil.Build(command, properties);
                command.Members.Add(codeConstructor);
                commandNameSpace.Types.Add(command);
                commandList.Add(commandNameSpace);
            }

            foreach (var method in domainClass.CreateMethods)
            {
                var commandNameSpace = _nameSpaceBuilderUtil.WithName($"Domain.{domainClass.Name}s").WithList().Build();
                var commandName = _nameBuilderUtil.CreateCommandName(domainClass, method);
                var properties = method.Parameters.Select(param => new Property { Name = param.Name, Type = param.Type }).ToList();
                var command = _classBuilderUtil.Build(commandName);
                var codeConstructor = _constructorBuilderUtil.BuildPublic(properties);
                _propertyBuilderUtil.Build(command, properties);
                command.Members.Add(codeConstructor);
                commandNameSpace.Types.Add(command);
                commandList.Add(commandNameSpace);
            }

            return commandList;
        }
    }
}