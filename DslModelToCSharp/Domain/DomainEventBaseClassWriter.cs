using System.Collections.Generic;
using System.Linq;
using DslModel.Domain;

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

        public DomainEventBaseClassWriter(string domain, string basePath)
        {
            _propertyBuilder = new PropBuilder();
            _constBuilder = new ConstBuilder();
            _fileWriter = new FileWriter(basePath);
            _nameSpaceBuilder = new NameSpaceBuilder();
            _classBuilder = new ClassBuilder();
            _domain = domain;
        }

        public void Build(string name, IList<Property> properties)
        {
            var nameSpace = _nameSpaceBuilder.BuildWithListImport(_domain);

            var generatedClass = _classBuilder.Build(name);

            generatedClass = _propertyBuilder.Build(generatedClass, properties);

            nameSpace.Types.Add(generatedClass);

            var constructor = _constBuilder.BuildPublicWithIdCreateInBody(properties.Skip(1).ToList(), properties[0].Name);
            generatedClass.Members.Add(constructor);

            _fileWriter.WriteToFile(name, "Base", nameSpace);
        }
    }
}