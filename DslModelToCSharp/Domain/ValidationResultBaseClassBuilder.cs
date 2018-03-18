using System.CodeDom;
using System.Collections.Generic;
using DslModel.Domain;
using DslModelToCSharp.Util;

namespace DslModelToCSharp.Domain
{
    public class ValidationResultBaseClassBuilder
    {
        private readonly IClassBuilder _classBuilder;
        private readonly ConstructorBuilderUtil _constructorBuilderUtil;
        private readonly string _domain;
        private readonly IFileWriter _fileWriter;
        private readonly NameSpaceBuilderUtil _nameSpaceBuilderUtil;
        private readonly PropertyBuilderUtil _propertyBuilderUtil;
        private readonly IStaticConstructorBuilder _staticConstructorBuilder;

        public ValidationResultBaseClassBuilder(string domain, string basePath)
        {
            _domain = domain;
            _fileWriter = new FileWriter(basePath);
            _staticConstructorBuilder = new StaticConstructorBuilder();
            _propertyBuilderUtil = new PropertyBuilderUtil();
            _constructorBuilderUtil = new ConstructorBuilderUtil();
            _nameSpaceBuilderUtil = new NameSpaceBuilderUtil();
            _classBuilder = new ClassBuilderUtil();
        }

        public void Write(ValidationResultBaseClass userClass)
        {
            var targetClass = _classBuilder.Build(userClass.Name);

            var nameSpace = _nameSpaceBuilderUtil.WithName(_domain).WithList().Build();

            var constructor = _constructorBuilderUtil.BuildPrivate(userClass.Properties);

            _propertyBuilderUtil.Build(targetClass, userClass.Properties);

            var buildOkResultConstructor = BuildOkResultConstructor(userClass);

            var errorResultConstructor = BuildErrorResultConstructor(userClass);

            targetClass.Members.Add(constructor);
            targetClass.Members.Add(buildOkResultConstructor);
            targetClass.Members.Add(errorResultConstructor);

            nameSpace.Types.Add(targetClass);

            _fileWriter.WriteToFile(userClass.Name, "Base", nameSpace);
        }

        private CodeMemberMethod BuildErrorResultConstructor(ValidationResultBaseClass userClass)
        {
            var errorResultConstructor = _staticConstructorBuilder.BuildErrorResult(
                new List<string> {$"new {userClass.Properties[1].Type}()", userClass.Properties[2].Name},
                new List<Property> {userClass.Properties[2]}, userClass.Name);
            return errorResultConstructor;
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