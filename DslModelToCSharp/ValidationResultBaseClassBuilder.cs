using System.CodeDom;
using System.Collections.Generic;
using DslModel;

namespace DslModelToCSharp
{
    public class ValidationResultBaseClassBuilder
    {
        private readonly IClassBuilder _classBuilder;
        private readonly IConstBuilder _constBuilder;
        private readonly string _domain;
        private readonly IFileWriter _fileWriter;
        private readonly INameSpaceBuilder _nameSpaceBuilder;
        private readonly IPropertyBuilder _propertyBuilder;
        private readonly IStaticConstructorBuilder _staticConstructorBuilder;

        public ValidationResultBaseClassBuilder(string domain, IFileWriter fileWriter,
            IStaticConstructorBuilder staticConstructorBuilder, IPropertyBuilder propertyBuilder,
            IConstBuilder constBuilder, INameSpaceBuilder nameSpaceBuilder, IClassBuilder classBuilder)
        {
            _domain = domain;
            _fileWriter = fileWriter;
            _staticConstructorBuilder = staticConstructorBuilder;
            _propertyBuilder = propertyBuilder;
            _constBuilder = constBuilder;
            _nameSpaceBuilder = nameSpaceBuilder;
            _classBuilder = classBuilder;
        }

        public void Write(ValidationResultBaseClass userClass)
        {
            var targetClass = _classBuilder.Build(userClass.Name);

            var nameSpace = _nameSpaceBuilder.BuildWithListImport(_domain);

            var constructor = _constBuilder.BuildPrivate(userClass.Properties);

            targetClass = _propertyBuilder.Build(targetClass, userClass.Properties);

            var buildOkResultConstructor = BuildOkResultConstructor(userClass);

            var errorResultConstructor = _staticConstructorBuilder.BuildErrorResult(
                new List<string> { $"new {userClass.Properties[1].Type}()", userClass.Properties[2].Name},
                new List<Property> { userClass.Properties[2] }, userClass.Name);

            targetClass.Members.Add(constructor);
            targetClass.Members.Add(buildOkResultConstructor);
            targetClass.Members.Add(errorResultConstructor);

            nameSpace.Types.Add(targetClass);

            _fileWriter.WriteToFile(userClass.Name, "Base", nameSpace);
        }

        private CodeMemberMethod BuildOkResultConstructor(ValidationResultBaseClass userClass)
        {
            var buildOkResultConstructor = _staticConstructorBuilder.BuildOkResult(
                new List<string> {userClass.Properties[1].Name, $"new {userClass.Properties[2].Type}()"},
                new List<Property> {userClass.Properties[1]}, userClass.Name);
            return buildOkResultConstructor;
        }
    }
}