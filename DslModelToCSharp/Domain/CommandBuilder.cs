using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using DslModel.Domain;
using DslModelToCSharp.Application;

namespace DslModelToCSharp.Domain
{
    public class CommandBuilder
    {
        private readonly ClassBuilder _classBuilder;
        private readonly ConstBuilder _constBuilder;
        private readonly NameSpaceBuilder _nameSpaceBuilder;
        private PropBuilder _propBuilder;
        private NameBuilder _nameBuilder;

        public CommandBuilder()
        {
            _classBuilder = new ClassBuilder();
            _constBuilder = new ConstBuilder();
            _nameSpaceBuilder = new NameSpaceBuilder();
            _propBuilder = new PropBuilder();
            _nameBuilder = new NameBuilder();
        }

        public List<CodeNamespace> Build(DomainClass domainClass)
        {
            var commandList = new List<CodeNamespace>();
            foreach (var method in domainClass.Methods)
            {
                var commandNameSpace = _nameSpaceBuilder.BuildWithListImport($"Domain.{domainClass.Name}s");
                var commandName = _nameBuilder.UpdateCommandName(domainClass, method);
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
                var commandNameSpace = _nameSpaceBuilder.BuildWithListImport($"Domain.{domainClass.Name}s");
                var commandName = _nameBuilder.CreateCommandName(domainClass, method);
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