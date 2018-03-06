using System.CodeDom;
using DslModel;

namespace DslModelToCSharp
{
    public class DomainClassWriter
    {
        private readonly IClassParser _classParser;
        private readonly IDomainEventWriter _domainEventWriter;
        private readonly IFileWriter _fileWriter;
        private readonly string _domain;
        private readonly IInterfaceParser _interfaceParser;
        private readonly IPropertyParser _propertyParser;

        public DomainClassWriter(IInterfaceParser interfaceParser, IPropertyParser propertyParser,
            IClassParser classParser, IDomainEventWriter domainEventWriter, IFileWriter fileWriter, string domainNameSpace = "Domain")
        {
            _interfaceParser = interfaceParser;
            _propertyParser = propertyParser;
            _classParser = classParser;
            _domainEventWriter = domainEventWriter;
            _fileWriter = fileWriter;
            _domain = domainNameSpace;
        }

        public void Write(DomainClass userClass)
        {
            var nameSpaceName = $"{_domain}.{userClass.Name}s";
            var nameSpace = new CodeNamespace(nameSpaceName);

            var iface = _interfaceParser.Parse(userClass);
            nameSpace.Types.Add(iface);

            var targetClass = _classParser.Parse(userClass);
            targetClass.BaseTypes.Add(iface.Name);

            nameSpace.Types.Add(targetClass);
            nameSpace.Imports.Add(new CodeNamespaceImport("System"));

            var constructor = new CodeConstructor();
            var emptyConstructor = new CodeConstructor();

            foreach (var proptery in userClass.Propteries)
            {
                var property = _propertyParser.Parse(proptery);
                targetClass.Members.Add(property.Field);
                targetClass.Members.Add(property.Property);
                constructor.Parameters.Add(new CodeParameterDeclarationExpression(proptery.Type, proptery.Name));
                CodeAssignStatement body = new CodeAssignStatement
                {
                    Left = new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), $"_{proptery.Name}"),
                    Right = new CodeFieldReferenceExpression(null, proptery.Name)
                };
                constructor.Statements.Add(body);
            }

            targetClass.Members.Add(constructor);
            targetClass.Members.Add(emptyConstructor);

            foreach (var domainEvent in userClass.Events)
            {
                _domainEventWriter.Write(domainEvent, nameSpaceName);
            }

            _fileWriter.WriteToFile(userClass.Name, nameSpaceName.Split(".")[1], nameSpace);
        }
    }
}