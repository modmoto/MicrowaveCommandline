using System.CodeDom;
using System.Collections.Generic;
using DslModel.Application;
using DslModel.Domain;
using DslModelToCSharp.Domain;
using DslModelToCSharp.Util;

namespace DslModelToCSharp.Application
{
    public class HookResultBuilder
    {
        private readonly IClassBuilder _classBuilder;
        private readonly ConstructorBuilderUtil _constructorBuilderUtil;
        private readonly string _nameSpace;
        private readonly NameSpaceBuilderUtil _nameSpaceBuilderUtil;
        private readonly PropertyBuilderUtil _propertyBuilderUtil;
        private readonly IStaticConstructorBuilder _staticConstructorBuilder;

        public HookResultBuilder(string nameSpace)
        {
            _nameSpace = nameSpace;
            _staticConstructorBuilder = new StaticConstructorBuilder();
            _propertyBuilderUtil = new PropertyBuilderUtil();
            _constructorBuilderUtil = new ConstructorBuilderUtil();
            _nameSpaceBuilderUtil = new NameSpaceBuilderUtil();
            _classBuilder = new ClassBuilderUtil();
        }

        public CodeNamespace Write(HookResultBaseClass userClass)
        {
            var targetClass = _classBuilder.Build(userClass.Name);

            var nameSpace = _nameSpaceBuilderUtil.BuildWithListImport(_nameSpace);

            var constructor = _constructorBuilderUtil.BuildPrivate(userClass.Properties);

            _propertyBuilderUtil.Build(targetClass, userClass.Properties);

            var buildOkResultConstructor = BuildOkResultConstructor(userClass);

            var errorResultConstructor = BuildErrorResultConstructor(userClass);

            targetClass.Members.Add(constructor);
            targetClass.Members.Add(buildOkResultConstructor);
            targetClass.Members.Add(errorResultConstructor);

            nameSpace.Types.Add(targetClass);

            return nameSpace;
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