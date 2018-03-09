using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using DslModel.Domain;

namespace DslModelToCSharp.Application
{
    public class CommandBuilder
    {
        private readonly ClassBuilder _classBuilder;
        private readonly ConstBuilder _constBuilder;
        private readonly NameSpaceBuilder _nameSpaceBuilder;
        private PropBuilder _propBuilder;

        public CommandBuilder()
        {
            _classBuilder = new ClassBuilder();
            _constBuilder = new ConstBuilder();
            _nameSpaceBuilder = new NameSpaceBuilder();
            _propBuilder = new PropBuilder();
        }

        public List<CodeNamespace> Build(DomainClass domainClass)
        {
            var commandList = new List<CodeNamespace>();
            foreach (var method in domainClass.Methods)
            {
                var commandNameSpace = _nameSpaceBuilder.BuildWithListImport($"Application.{domainClass.Name}s.Commands");
                var commandName = domainClass.Name + method.Name + "Command";
                var properties = method.Parameters.Select(param => new Property {Name = param.Name, Type = param.Type}).ToList();
                var command = _classBuilder.Build(commandName);
                var codeConstructor = _constBuilder.BuildPublic(properties);
                _propBuilder.Build(command, properties);
                command.Members.Add(codeConstructor);
                commandNameSpace.Types.Add(command);
                commandList.Add(commandNameSpace);
            }

            foreach (var method in domainClass.CreateMethods)
            {
                var commandNameSpace = _nameSpaceBuilder.BuildWithListImport($"Application.{domainClass.Name}s.Commands");
                var commandName = domainClass.Name + method.Name + "Command";
                var properties = method.Parameters.Select(param => new Property { Name = param.Name, Type = param.Type }).ToList();
                var command = _classBuilder.Build(commandName);
                var codeConstructor = _constBuilder.BuildPublic(properties);
                _propBuilder.Build(command, properties);
                command.Members.Add(codeConstructor);
                commandNameSpace.Types.Add(command);
                commandList.Add(commandNameSpace);
            }

            return commandList;
        }
    }
}