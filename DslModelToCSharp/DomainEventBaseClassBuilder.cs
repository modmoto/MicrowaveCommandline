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
        private readonly INameSpaceBuilder _nameSpaceBuilder;

        public DomainEventBaseClassBuilder(IPropertyBuilder propertyBuilder, IConstBuilder constBuilder, IFileWriter fileWriter, INameSpaceBuilder nameSpaceBuilder, string domain)
        {
            _propertyBuilder = propertyBuilder;
            _constBuilder = constBuilder;
            _fileWriter = fileWriter;
            _nameSpaceBuilder = nameSpaceBuilder;
            _domain = domain;
        }

        public void Build(string name, IList<Property> properties)
        {
            var nameSpace = _nameSpaceBuilder.BuildWithListImport(_domain);

            var generatedClass = ClassBuilder(name);

            generatedClass = _propertyBuilder.Build(generatedClass, properties);

            nameSpace.Types.Add(generatedClass);

            var constructor = _constBuilder.BuildPublic(properties);
            generatedClass.Members.Add(constructor);

            _fileWriter.WriteToFile(name, "Base", nameSpace);
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