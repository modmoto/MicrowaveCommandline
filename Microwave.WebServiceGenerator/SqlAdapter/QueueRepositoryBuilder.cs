using System.CodeDom;
using Microwave.WebServiceGenerator.Util;
using Microwave.WebServiceModel.Application;
using Microwave.WebServiceModel.SqlAdapter;

namespace Microwave.WebServiceGenerator.SqlAdapter
{
    public class QueueRepositoryBuilder
    {
        private readonly ClassBuilderUtil _classBuilderUtil;
        private readonly ConstructorBuilderUtil _constructorBuilderUtil;
        private readonly string _nameSpace;
        private readonly NameSpaceBuilderUtil _nameSpaceBuilderUtil;
        private readonly PropertyBuilderUtil _propertyBuilderUtil;

        public QueueRepositoryBuilder(string nameSpace)
        {
            _nameSpace = nameSpace;
            _nameSpaceBuilderUtil = new NameSpaceBuilderUtil();
            _classBuilderUtil = new ClassBuilderUtil();
            _propertyBuilderUtil = new PropertyBuilderUtil();
            _constructorBuilderUtil = new ConstructorBuilderUtil();
        }

        public CodeNamespace Build(QueueRepositoryClass hangfireQueue)
        {
            var targetClass = _classBuilderUtil.Build(hangfireQueue.Name);
            var codeNamespace = _nameSpaceBuilderUtil.WithName($"{_nameSpace}").WithApplication().WithTask()
                .WithDomain().WithList().WithLinq().WithEfCore().Build();
            codeNamespace.Types.Add(targetClass);

            targetClass.BaseTypes.Add(new CodeTypeReference($"I{hangfireQueue.Name}"));

            _propertyBuilderUtil.Build(targetClass, hangfireQueue.Properties);
            var constructor = _constructorBuilderUtil.BuildPublic(hangfireQueue.Properties);
            targetClass.Members.Add(constructor);

            var addEventsMethod = CreateAddEventsMethod();
            targetClass.Members.Add(addEventsMethod);

            var getEventsMethod = CreateGetEventsMethod();
            targetClass.Members.Add(getEventsMethod);

            var removeEventsMethod = CreateRemoveEventsMethod();
            targetClass.Members.Add(removeEventsMethod);

            return codeNamespace;
        }

        private CodeMemberMethod CreateAddEventsMethod()
        {
            var method = new CodeMemberMethod
            {
                Name = "AddEventForJob",
                ReturnType = new CodeTypeReference("async Task"),
                Attributes = MemberAttributes.Public | MemberAttributes.Final
            };

            method.Parameters.Add(new CodeParameterDeclarationExpression(
                new CodeTypeReference(new EventAndJobClass().Name), "eventAndJob"));

            method.Statements.Add(new CodeSnippetExpression("Context.EventAndJobQueue.Add(eventAndJob)"));
            method.Statements.Add(new CodeSnippetExpression("await Context.SaveChangesAsync()"));

            return method;
        }

        private CodeMemberMethod CreateRemoveEventsMethod()
        {
            var method = new CodeMemberMethod
            {
                Name = "RemoveEventsFromQueue",
                ReturnType = new CodeTypeReference("async Task"),
                Attributes = MemberAttributes.Public | MemberAttributes.Final
            };

            method.Parameters.Add(
                new CodeParameterDeclarationExpression(new CodeTypeReference($"List<{new EventAndJobClass().Name}>"),
                    "handledEvents"));

            method.Statements.Add(new CodeSnippetExpression("Context.EventAndJobQueue.RemoveRange(handledEvents)"));
            method.Statements.Add(new CodeSnippetExpression("await Context.SaveChangesAsync()"));

            return method;
        }

        private CodeMemberMethod CreateGetEventsMethod()
        {
            var method = new CodeMemberMethod
            {
                Name = "GetEvents",
                ReturnType = new CodeTypeReference($"async Task<List<{new EventAndJobClass().Name}>>"),
                Attributes = MemberAttributes.Public | MemberAttributes.Final
            };

            method.Parameters.Add(new CodeParameterDeclarationExpression(new CodeTypeReference(" string"), "jobName"));

            method.Statements.Add(new CodeSnippetExpression(
                "var eventList = await Context.EventAndJobQueue.Include(queue => queue.DomainEvent).Where(eve => eve.JobName == jobName).ToListAsync()"));
            method.Statements.Add(new CodeSnippetExpression("return eventList"));

            return method;
        }
    }
}