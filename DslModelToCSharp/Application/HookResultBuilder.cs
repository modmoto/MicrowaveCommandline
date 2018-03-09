using System.CodeDom;
using System.Collections.Generic;
using DslModel.Domain;

namespace DslModelToCSharp.Application
{
    public class HookResultBuilder
    {
        private readonly IClassBuilder _classBuilder;
        private readonly IConstBuilder _constBuilder;
        private readonly string _nameSpace;
        private readonly IFileWriter _fileWriter;
        private readonly INameSpaceBuilder _nameSpaceBuilder;
        private readonly IPropertyBuilder _propertyBuilder;
        private readonly IStaticConstructorBuilder _staticConstructorBuilder;

        public HookResultBuilder(string nameSpace, IFileWriter fileWriter,
            IStaticConstructorBuilder staticConstructorBuilder, IPropertyBuilder propertyBuilder,
            IConstBuilder constBuilder, INameSpaceBuilder nameSpaceBuilder, IClassBuilder classBuilder)
        {
            _nameSpace = nameSpace;
            _fileWriter = fileWriter;
            _staticConstructorBuilder = staticConstructorBuilder;
            _propertyBuilder = propertyBuilder;
            _constBuilder = constBuilder;
            _nameSpaceBuilder = nameSpaceBuilder;
            _classBuilder = classBuilder;
        }

        public void Write(HookResultBaseClass userClass)
        {
            var targetClass = _classBuilder.Build(userClass.Name);

            var nameSpace = _nameSpaceBuilder.BuildWithListImport(_nameSpace);

            var constructor = _constBuilder.BuildPrivate(userClass.Properties);

            targetClass = _propertyBuilder.Build(targetClass, userClass.Properties);

            var buildOkResultConstructor = BuildOkResultConstructor(userClass);

            var errorResultConstructor = BuildErrorResultConstructor(userClass);

            targetClass.Members.Add(constructor);
            targetClass.Members.Add(buildOkResultConstructor);
            targetClass.Members.Add(errorResultConstructor);

            nameSpace.Types.Add(targetClass);

            _fileWriter.WriteToFile(userClass.Name, "Base", nameSpace);
        }

        private CodeMemberMethod BuildErrorResultConstructor(HookResultBaseClass userClass)
        {
            var errorResultConstructor = _staticConstructorBuilder.BuildErrorResult(
                new List<string> {userClass.Properties[1].Name},
                new List<Property> {userClass.Properties[1]}, userClass.Name);
            return errorResultConstructor;
        }

        private CodeMemberMethod BuildOkResultConstructor(HookResultBaseClass userClass)
        {
            var buildOkResultConstructor = _staticConstructorBuilder.BuildOkResult(
                new List<string> { $"new {userClass.Properties[1].Type}()" },
                new List<Property>(), userClass.Name);
            return buildOkResultConstructor;
        }
    }
}