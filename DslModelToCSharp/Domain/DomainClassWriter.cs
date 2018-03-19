using System.CodeDom;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DslModel.Domain;
using DslModelToCSharp.Util;

namespace DslModelToCSharp.Domain
{
    public class DomainClassWriter
    {
        private readonly IClassBuilder _classBuilder;
        private readonly ConstructorBuilderUtil _constructorBuilderUtil;
        private readonly string _domain;
        private readonly string _basePathRealClasses;
        private readonly IFileWriter _fileWriter;
        private readonly IInterfaceBuilder _interfaceBuilder;
        private readonly NameSpaceBuilderUtil _nameSpaceBuilderUtil;
        private readonly PropertyBuilderUtil _propertyBuilderUtil;
        private readonly IStaticConstructorBuilder _staticConstructorBuilder;
        private CommandBuilder _commandBuilder;
        private ListPropBuilderUtil _listPropBuilderUtil;
        private FileWriter _fileWriterRealClasses;

        public DomainClassWriter(string domainNameSpace, string basePath, string basePathRealClasses)
        {
            _interfaceBuilder = new InterfaceBuilderUtil();
            _propertyBuilderUtil = new PropertyBuilderUtil();
            _classBuilder = new ClassBuilderUtil();
            _fileWriter = new FileWriter(basePath);
            _fileWriterRealClasses = new FileWriter(basePathRealClasses);
            _constructorBuilderUtil = new ConstructorBuilderUtil();
            _staticConstructorBuilder = new StaticConstructorBuilder();
            _nameSpaceBuilderUtil = new NameSpaceBuilderUtil();
            _domain = domainNameSpace;
            _basePathRealClasses = basePathRealClasses;
            _commandBuilder = new CommandBuilder();
            _listPropBuilderUtil = new ListPropBuilderUtil();
        }

        public void Write(DomainClass domainClass)
        {
            var nameSpace = _nameSpaceBuilderUtil.WithName($"{_domain}.{domainClass.Name}s").WithList().Build();
            var iface = _interfaceBuilder.BuildForCommand(domainClass);

            var targetClass = _classBuilder.BuildPartial(domainClass.Name);
            targetClass.BaseTypes.Add(iface.Name);

            nameSpace.Types.Add(iface);

            foreach (var createMethod in domainClass.CreateMethods)
            {
                var properties = createMethod.Parameters.Select(param => new Property {Name = param.Name, Type = param.Type}).ToList();
                var constructor = _constructorBuilderUtil.BuildPrivateForCreateMethod(properties, $"{domainClass.Name}{createMethod.Name}Command");
                targetClass.Members.Add(constructor);
            }

            targetClass = _listPropBuilderUtil.Build(targetClass, domainClass.ListProperties);

            foreach (var listProperty in domainClass.ListProperties)
            {
                nameSpace.Imports.Add(new CodeNamespaceImport($"Domain.{listProperty.Type}s"));
            }

            var commands = _commandBuilder.Build(domainClass);

            foreach (var command in commands)
            {
                _fileWriter.WriteToFile(command.Types[0].Name, $"{domainClass.Name}s/Commands", command);
            }

            var emptyConstructor = _constructorBuilderUtil.BuildPrivate(new List<Property>());

            var propertiesWithDefaultId = domainClass.Properties;
            propertiesWithDefaultId.Add(new Property {Name = "Id", Type = "Guid"});
            _propertyBuilderUtil.Build(targetClass, propertiesWithDefaultId);
            targetClass.Members.Add(emptyConstructor);

            nameSpace.Types.Add(targetClass);

            _fileWriter.WriteToFile(domainClass.Name, nameSpace.Name.Split(".")[1], nameSpace);

            if (!ClassIsAllreadyExisting(domainClass))
            {
                var nameSpaceRealClass = _nameSpaceBuilderUtil.WithName($"{_domain}.{domainClass.Name}s").WithList().Build();
                var targetClassReal = _classBuilder.BuildPartial(domainClass.Name);
                foreach (var createMethod in domainClass.CreateMethods)
                {
                    var method = new CodeMemberMethod
                    {
                        Name = createMethod.Name,
                        ReturnType = new CodeTypeReference($"{new CreationResultBaseClass().Name}<{domainClass.Name}>")
                    };

                    method.Parameters.Add(new CodeParameterDeclarationExpression { Type = new CodeTypeReference($"{domainClass.Name}{createMethod.Name}Command"), Name = "command" });

                    method.Statements.Add(new CodeSnippetExpression("var newGuid = Guid.NewGuid()"));
                    method.Statements.Add(new CodeSnippetExpression($"var entity = new {domainClass.Name}(newGuid, command)"));
                    method.Statements.Add(new CodeSnippetExpression($"return CreationResult<{domainClass.Name}>.OkResult(new List<DomainEventBase> {{ new {domainClass.Name}CreateEvent(entity, newGuid) }}, entity)"));
                    method.Attributes = MemberAttributes.Final | MemberAttributes.Public | MemberAttributes.Static;
                    targetClassReal.Members.Add(method);
                }

                foreach (var domainMethod in domainClass.Methods)
                {
                    var method = new CodeMemberMethod
                    {
                        Name = domainMethod.Name,
                        ReturnType = new CodeTypeReference(domainMethod.ReturnType)
                    };
                    method.Parameters.Add(new CodeParameterDeclarationExpression { Type = new CodeTypeReference($"{domainClass.Name}{domainMethod.Name}Command"), Name = "command" });
                    method.Attributes = MemberAttributes.Final | MemberAttributes.Public;
                    method.Statements.Add(new CodeSnippetExpression("return ValidationResult.ErrorResult(new List<string>{\"A generated Domain Update Method that is not implemented was called, aborting...\"})"));
                    targetClassReal.Members.Add(method);
                }
                nameSpaceRealClass.Types.Add(targetClassReal);

                _fileWriterRealClasses.WriteToFile(domainClass.Name, "Domain", nameSpaceRealClass, false);
            }
           
        }

        private bool ClassIsAllreadyExisting(DomainClass domainClass)
        {
            var formattableString = $"{_basePathRealClasses}/Domain/{domainClass.Name}.cs";
            return File.Exists(formattableString);
        }

        public void Write(CreationResultBaseClass userClass)
        {
            var nameSpace = _nameSpaceBuilderUtil.WithName(_domain).WithList().Build();

            var targetClass = _classBuilder.Build(userClass.Name);

            var userClassGenericType = new CodeTypeParameter(userClass.GenericType);
            userClassGenericType.Constraints.Add(" class");
            targetClass.TypeParameters.Add(userClassGenericType);

            var constructor = _constructorBuilderUtil.BuildPrivate(userClass.Properties);

            _propertyBuilderUtil.Build(targetClass, userClass.Properties);

            var buildOkResultConstructor = BuildOkConstructor(userClass);
            var errorResultConstructor = BuildErrorConstructor(userClass);

            targetClass.Members.Add(constructor);
            targetClass.Members.Add(buildOkResultConstructor);
            targetClass.Members.Add(errorResultConstructor);

            nameSpace.Types.Add(targetClass);

            _fileWriter.WriteToFile(userClass.Name, "Base", nameSpace);
        }

        private CodeMemberMethod BuildErrorConstructor(CreationResultBaseClass userClass)
        {
            var properties = userClass.Properties.Take(3).ToList();
            properties.Add(new Property {Name = "null"});
            var errorResultConstructor = _staticConstructorBuilder.BuildErrorResultGeneric(new List<string>
                {
                    $"new {userClass.Properties[1].Type}()",
                    userClass.Properties[2].Name,
                    "null"
                },
                new List<Property> { userClass.Properties[2] }, userClass.Name,
                userClass.GenericType);
            return errorResultConstructor;
        }

        private CodeMemberMethod BuildOkConstructor(CreationResultBaseClass userClass)
        {
            var buildOkResultConstructor = _staticConstructorBuilder.BuildOkResultGeneric(
                new List<string>
                {
                    userClass.Properties[1].Name,
                    $"new {userClass.Properties[2].Type}()",
                    userClass.Properties[3].Name
                },
                new List<Property> {userClass.Properties[1], userClass.Properties[3]}, userClass.Name,
                userClass.GenericType);
            return buildOkResultConstructor;
        }
    }
}