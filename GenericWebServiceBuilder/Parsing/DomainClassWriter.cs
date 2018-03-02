using System.CodeDom;
using System.CodeDom.Compiler;
using System.IO;
using System.Reflection;
using GenericWebServiceBuilder.DSL;
using Microsoft.CSharp;

namespace GenericWebServiceBuilder.Parsing
{
    public class DomainClassWriter
    {
        private readonly IInterfaceParser _interfaceParser;
        private readonly IPropertyParser _propertyParser;
        private readonly IClassParser _classParser;
        private readonly string _domain;

        public DomainClassWriter(IInterfaceParser interfaceParser, IPropertyParser propertyParser, IClassParser classParser, string domainNameSpace = "Domain")
        {
            _interfaceParser = interfaceParser;
            _propertyParser = propertyParser;
            _classParser = classParser;
            _domain = domainNameSpace;
        }
        public void Write(DomainClass userClass)
        {
            CodeNamespace nameSpace = new CodeNamespace(_domain);

            var iface = _interfaceParser.Parse(userClass);
            nameSpace.Types.Add(iface);

            var targetClass = _classParser.Parse(userClass);
            targetClass.BaseTypes.Add(iface.Name);

            nameSpace.Types.Add(targetClass);

            foreach (var proptery in userClass.Propteries)
            {
                var property = _propertyParser.Parse(proptery);
                targetClass.Members.Add(property.Field);
                targetClass.Members.Add(property.Property);
            }

            var targetUnit = new CodeCompileUnit();
            targetUnit.Namespaces.Add(nameSpace);

            CSharpCodeProvider provider = new CSharpCodeProvider();
            CodeGeneratorOptions options = new CodeGeneratorOptions();
            options.BracingStyle = "C";
            using (StreamWriter sourceWriter = new StreamWriter($"Domain/Generated/{userClass.Name}.g.cs"))
            {
                provider.GenerateCodeFromCompileUnit(targetUnit, sourceWriter, options);
            }
        }
    }
}