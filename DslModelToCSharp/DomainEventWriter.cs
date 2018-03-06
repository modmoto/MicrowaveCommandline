using System.CodeDom;
using DslModel;

namespace DslModelToCSharp
{
    public interface IDomainEventWriter
    {
        void Write(DomainEvent domainEvent, string nameSpaceName);
    }

    public class DomainEventWriter : IDomainEventWriter
    {
        private readonly IClassParser _classParser;
        private readonly IFileWriter _fileWriter;
        private readonly IPropertyParser _propertyParser;

        public DomainEventWriter(IPropertyParser propertyParser, IFileWriter fileWriter, IClassParser classParser)
        {
            _propertyParser = propertyParser;
            _fileWriter = fileWriter;
            _classParser = classParser;
        }

        public void Write(DomainEvent domainEvent, string nameSpaceName)
        {
            var nameSpace = new CodeNamespace(nameSpaceName);

            var targetClass = _classParser.Parse(domainEvent);

            nameSpace.Types.Add(targetClass);
            nameSpace.Imports.Add(new CodeNamespaceImport("System"));

            var constructor = new CodeConstructor();
            constructor.Attributes = MemberAttributes.Public | MemberAttributes.Final;
            var emptyConstructor = new CodeConstructor();

            foreach (var proptery in domainEvent.Properties)
            {
                var autoProperty = _propertyParser.Parse(proptery);
                targetClass.Members.Add(autoProperty.Field);
                targetClass.Members.Add(autoProperty.Property);
                constructor.Parameters.Add(new CodeParameterDeclarationExpression(proptery.Type, proptery.Name));
                var body = new CodeAssignStatement
                {
                    Left = new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), $"_{proptery.Name}"),
                    Right = new CodeFieldReferenceExpression(null, proptery.Name)
                };
                constructor.Statements.Add(body);
            }

            targetClass.BaseTypes.Add(new CodeTypeReference(DomainEventBaseClass.Name));
            targetClass.Members.Add(emptyConstructor);
            targetClass.Members.Add(constructor);
            _fileWriter.WriteToFile(domainEvent.Name, nameSpaceName.Split(".")[1], nameSpace);
        }
    }
}