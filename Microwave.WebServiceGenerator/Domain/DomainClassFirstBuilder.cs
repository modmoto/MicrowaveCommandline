using System.CodeDom;
using Microwave.LanguageModel;
using Microwave.WebServiceGenerator.Util;
using Microwave.WebServiceModel.Domain;

namespace Microwave.WebServiceGenerator.Domain
{
    public class DomainClassFirstBuilder : IConcreteClassBuilder
    {
        private readonly ClassBuilderUtil _classBuilder;
        private readonly NameSpaceBuilderUtil _nameSpaceBuilderUtil;
        private readonly NameBuilderUtil _nameBuilderUtil;
        private readonly DomainClass _domainClass;
        private CodeTypeDeclaration _targetClassReal;
        private CodeNamespace _nameSpaceRealClass;

        public DomainClassFirstBuilder(DomainClass domainClass)
        {
            _domainClass = domainClass;
            _classBuilder = new ClassBuilderUtil();
            _nameSpaceBuilderUtil = new NameSpaceBuilderUtil();
            _nameBuilderUtil = new NameBuilderUtil();
        }

        public void AddNameSpace()
        {
            _nameSpaceBuilderUtil.WithName($"Domain.{_domainClass.Name}s").WithList();

            foreach (var childHookMethod in _domainClass.ChildHookMethods)
            {
                _nameSpaceBuilderUtil.WithDomainEntityNameSpace(childHookMethod.OriginEntity);
            }

            _nameSpaceRealClass = _nameSpaceBuilderUtil.Build();
        }

        public void AddClassType()
        {
            _targetClassReal = _classBuilder.BuildPartial(_domainClass.Name);
        }

        public void AddClassProperties()
        {
        }

        public void AddConstructor()
        {
        }

        public void AddBaseTypes()
        {
        }

        public void AddConcreteMethods()
        {
            foreach (var createMethod in _domainClass.CreateMethods)
            {
                var method = new CodeMemberMethod
                {
                    Name = createMethod.Name,
                    ReturnType = new CodeTypeReference($"{new CreationResultBaseClass().Name}<{_domainClass.Name}>")
                };

                method.Parameters.Add(new CodeParameterDeclarationExpression { Type = new CodeTypeReference($"{_domainClass.Name}{createMethod.Name}Command"), Name = "command" });

                method.Statements.Add(new CodeSnippetExpression("// TODO: Implement this method"));
                method.Statements.Add(new CodeSnippetExpression("var newGuid = Guid.NewGuid()"));
                method.Statements.Add(new CodeSnippetExpression($"var entity = new {_domainClass.Name}(newGuid, command)"));
                method.Statements.Add(new CodeSnippetExpression($"return CreationResult<{_domainClass.Name}>.OkResult(new List<DomainEventBase> {{ new {_domainClass.Name}CreateEvent(entity, newGuid) }}, entity)"));
                method.Attributes = MemberAttributes.Final | MemberAttributes.Public | MemberAttributes.Static;
                _targetClassReal.Members.Add(method);
            }

            foreach (var domainMethod in _domainClass.Methods)
            {
                var method = new CodeMemberMethod
                {
                    Name = domainMethod.Name,
                    ReturnType = new CodeTypeReference(domainMethod.ReturnType)
                };
                method.Parameters.Add(new CodeParameterDeclarationExpression { Type = new CodeTypeReference($"{_domainClass.Name}{domainMethod.Name}Command"), Name = "command" });
                method.Attributes = MemberAttributes.Public | MemberAttributes.Override;

                method.Statements.Add(new CodeSnippetExpression("// TODO: Implement this method"));
                method.Statements.Add(new CodeSnippetExpression($"return ValidationResult.ErrorResult(new List<string>{{\"The Method \\\"{domainMethod.Name}\\\" in Class \\\"{_domainClass.Name}\\\" that is not implemented was called, aborting...\"}})"));
                _targetClassReal.Members.Add(method);
            }

            foreach (var domainMethod in _domainClass.ChildHookMethods)
            {
                var method = new CodeMemberMethod
                {
                    Name = _nameBuilderUtil.OnChildHookMethodName(domainMethod),
                    ReturnType = new CodeTypeReference(domainMethod.ReturnType)
                };
                method.Parameters.Add(new CodeParameterDeclarationExpression { Type = new CodeTypeReference($"{domainMethod.Parameters[0].Name}"), Name = "hookEvent" });
                method.Attributes = MemberAttributes.Public | MemberAttributes.Override;

                method.Statements.Add(new CodeSnippetExpression("// TODO: Implement this method"));
                method.Statements.Add(new CodeSnippetExpression($"return ValidationResult.ErrorResult(new List<string>{{\"The Method \\\"{_nameBuilderUtil.OnChildHookMethodName(domainMethod)}\\\" in Class \\\"{_domainClass.Name}\\\" that is not implemented was called, aborting...\"}})"));
                _targetClassReal.Members.Add(method);
            }
        }

        public CodeNamespace Build()
        {
            _nameSpaceRealClass.Types.Add(_targetClassReal);
            return _nameSpaceRealClass;
        }
    }
}