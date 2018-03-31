using System.CodeDom;
using System.Collections.Generic;
using Microwave.LanguageModel;
using Microwave.WebServiceGenerator.Util;
using Microwave.WebServiceModel.SqlAdapter;

namespace Microwave.WebServiceGenerator.SqlAdapter
{
    public class EventJobRegistrationClassBuilder
    {
        private readonly string _nameSpace;
        private readonly ClassBuilderUtil _classBuilderUtil;
        private readonly NameSpaceBuilderUtil _nameSpaceBuilderUtil;
        private readonly PropertyBuilderUtil _propertyBuilderUtil;
        private NameBuilderUtil _nameBuilderUtil;

        public EventJobRegistrationClassBuilder(string nameSpace)
        {
            _nameSpace = nameSpace;
            _nameSpaceBuilderUtil = new NameSpaceBuilderUtil();
            _classBuilderUtil = new ClassBuilderUtil();
            _propertyBuilderUtil = new PropertyBuilderUtil();
            _nameBuilderUtil = new NameBuilderUtil();
        }

        public CodeNamespace Build(List<AsyncDomainHook> domainHooks)
        {
            var nameSpace = _nameSpaceBuilderUtil.WithName($"{_nameSpace}").WithList().Build();
            var generatedClass = _classBuilderUtil.Build("EventJobRegistration");
            var constructor = new CodeConstructor();
            constructor.Attributes = MemberAttributes.Public | MemberAttributes.Final;
            constructor.Statements.Add(new CodeSnippetExpression($"EventJobs = new List<{new EventTupleClass().Name}>()"));
            foreach (var domainHook in domainHooks)
            {
                nameSpace.Imports.Add(new CodeNamespaceImport($"Domain.{domainHook.ClassType}s"));
                constructor.Statements.Add(new CodeSnippetExpression(
                    $"EventJobs.Add(new EventTuple(typeof({_nameBuilderUtil.HooKEventName(domainHook)}).ToString(), \"{domainHook.Name}\"))"));
            }

            _propertyBuilderUtil.Build(generatedClass,new List<Property>{new Property {Name = "EventJobs", Type = $"List<{new EventTupleClass().Name}>"} });

            generatedClass.Members.Add(constructor);
            nameSpace.Types.Add(generatedClass);

            return nameSpace;
        }
    }
}