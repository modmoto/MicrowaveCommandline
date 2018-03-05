using System.CodeDom;
using System.CodeDom.Compiler;
using System.IO;
using DslModel;
using Microsoft.CSharp;

namespace DslModelToCSharp
{
    public class DomainClassWriter
    {
        private readonly IClassParser _classParser;
        private readonly string _domain;
        private readonly IInterfaceParser _interfaceParser;
        private readonly IPropertyParser _propertyParser;

        public DomainClassWriter(IInterfaceParser interfaceParser, IPropertyParser propertyParser,
            IClassParser classParser, string domainNameSpace = "Domain")
        {
            _interfaceParser = interfaceParser;
            _propertyParser = propertyParser;
            _classParser = classParser;
            _domain = domainNameSpace;
        }

        public void Write(DomainEvent domainEvent)
        {
            var nameSpace = new CodeNamespace(_domain);

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
                CodeAssignStatement body = new CodeAssignStatement
                {
                    Left = new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), $"_{proptery.Name}"),
                    Right = new CodeFieldReferenceExpression(null, proptery.Name)
                };
                constructor.Statements.Add(body);
            }

            targetClass.Members.Add(emptyConstructor);
            targetClass.Members.Add(constructor);
            WriteToFile(domainEvent.Name, nameSpace);
        }

        private static void WriteToFile(string fileName, CodeNamespace nameSpace)
        {
            var targetUnit = new CodeCompileUnit();
            targetUnit.Namespaces.Add(nameSpace);

            var provider = new CSharpCodeProvider();
            var options = new CodeGeneratorOptions();
            options.BracingStyle = "C";
            using (var sourceWriter = new StreamWriter($"../GenericWebservice/Domain/Generated/{fileName}.g.cs"))
            {
                provider.GenerateCodeFromCompileUnit(targetUnit, sourceWriter, options);
            }
        }

        public void Write(DomainClass userClass)
        {
            var nameSpace = new CodeNamespace(_domain);

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
            WriteToFile(userClass.Name, nameSpace);
        }
    }
}