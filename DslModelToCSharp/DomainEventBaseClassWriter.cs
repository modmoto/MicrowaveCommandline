using System.Collections.Generic;
using DslModel;

namespace DslModelToCSharp
{
    public class DomainEventBaseClassWriter
    {
        private readonly IConstBuilder _constBuilder;
        private readonly string _domain;
        private readonly IFileWriter _fileWriter;
        private readonly INameSpaceBuilder _nameSpaceBuilder;
        private readonly IClassBuilder _classBuilder;
        private readonly IPropertyBuilder _propertyBuilder;

        public DomainEventBaseClassWriter(IPropertyBuilder propertyBuilder, IConstBuilder constBuilder,
            IFileWriter fileWriter, INameSpaceBuilder nameSpaceBuilder, IClassBuilder classBuilder, string domain)
        {
            _propertyBuilder = propertyBuilder;
            _constBuilder = constBuilder;
            _fileWriter = fileWriter;
            _nameSpaceBuilder = nameSpaceBuilder;
            _classBuilder = classBuilder;
            _domain = domain;
        }

        public void Build(string name, IList<Property> properties)
        {
            var nameSpace = _nameSpaceBuilder.BuildWithListImport(_domain);

            var generatedClass = _classBuilder.Build(name);

            generatedClass = _propertyBuilder.Build(generatedClass, properties);

            nameSpace.Types.Add(generatedClass);

            var constructor = _constBuilder.BuildPublic(properties);
            generatedClass.Members.Add(constructor);

            _fileWriter.WriteToFile(name, "Base", nameSpace);
        }
    }
}