using System.CodeDom;
using Microwave.LanguageModel;
using Microwave.WebServiceGenerator.Util;
using Microwave.WebServiceModel.Domain;

namespace Microwave.WebServiceGenerator.Domain
{
    public class DomainClassFirstBuilder
    {
        private readonly ClassBuilderUtil _classBuilder;
        private readonly string _domain;
        private readonly NameSpaceBuilderUtil _nameSpaceBuilderUtil;

        public DomainClassFirstBuilder(string domainNameSpace)
        {
            _classBuilder = new ClassBuilderUtil();
            _nameSpaceBuilderUtil = new NameSpaceBuilderUtil();
            _domain = domainNameSpace;
        }

        public CodeNamespace Build(DomainClass domainClass)
        {
            _nameSpaceBuilderUtil.WithName($"{_domain}.{domainClass.Name}s").WithList();

            foreach (var childHookMethod in domainClass.ChildHookMethods)
            {
                _nameSpaceBuilderUtil.WithDomainEntityNameSpace(childHookMethod.OriginEntity);
            }

            var nameSpaceRealClass = _nameSpaceBuilderUtil.Build();

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
                method.Attributes = MemberAttributes.Public | MemberAttributes.Override;
                method.Statements.Add(new CodeSnippetExpression($"return ValidationResult.ErrorResult(new List<string>{{\"The Method \\\"{domainMethod.Name}\\\" in Class \\\"{domainClass.Name}\\\" that is not implemented was called, aborting...\"}})"));
                targetClassReal.Members.Add(method);
            }

            foreach (var domainMethod in domainClass.ChildHookMethods)
            {
                var method = new CodeMemberMethod
                {
                    Name = domainMethod.Name,
                    ReturnType = new CodeTypeReference(domainMethod.ReturnType)
                };
                method.Parameters.Add(new CodeParameterDeclarationExpression { Type = new CodeTypeReference($"{domainMethod.Parameters[0].Name}"), Name = "hookEvent" });
                method.Attributes = MemberAttributes.Public | MemberAttributes.Override;
                method.Statements.Add(new CodeSnippetExpression($"return ValidationResult.ErrorResult(new List<string>{{\"The Method \\\"{domainMethod.Name}\\\" in Class \\\"{domainClass.Name}\\\" that is not implemented was called, aborting...\"}})"));
                targetClassReal.Members.Add(method);
            }

            nameSpaceRealClass.Types.Add(targetClassReal);

            return nameSpaceRealClass;
        }
    }
}