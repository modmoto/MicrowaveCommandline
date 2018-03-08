using System.CodeDom;
using System.Collections.Generic;
using System.Reflection;
using DslModel;

namespace DslModelToCSharp
{
    public class DomainEventBaseClassBuilder
    {
        private readonly string _domain;
        private readonly IPropertyBuilder _propertyBuilder;
        private readonly IConstBuilder _constBuilder;
        private readonly IFileWriter _fileWriter;

        public DomainEventBaseClassBuilder(IPropertyBuilder propertyBuilder, IConstBuilder constBuilder, IFileWriter fileWriter, string domain)
        {
            _propertyBuilder = propertyBuilder;
            _constBuilder = constBuilder;
            _fileWriter = fileWriter;
            _domain = domain;
        }

        public void Build(string name, IList<Property> properties)
        {
            var nameSpace = NameSpaceBuilder();

            var generatedClass = ClassBuilder(name);

            generatedClass = _propertyBuilder.Build(generatedClass, properties);

            nameSpace.Types.Add(generatedClass);

            var constructor = _constBuilder.BuildPublic(properties);
            generatedClass.Members.Add(constructor);

            _fileWriter.WriteToFile(name, "Base", nameSpace);
        }

        private CodeNamespace NameSpaceBuilder()
        {
            var nameSpaceName = _domain;
            var nameSpace = new CodeNamespace(nameSpaceName);
            nameSpace.Imports.Add(new CodeNamespaceImport("System"));
            nameSpace.Imports.Add(new CodeNamespaceImport("System.Collections.Generic"));
            return nameSpace;
        }

        private static CodeTypeDeclaration ClassBuilder(string name)
        {
            var targetClass = new CodeTypeDeclaration(name);
            targetClass.IsClass = true;
            targetClass.TypeAttributes = TypeAttributes.Public;
            return targetClass;
        }
    }
}