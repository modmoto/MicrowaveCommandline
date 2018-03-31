using System.CodeDom;
using System.Collections.Generic;
using Microwave.LanguageModel;
using Microwave.WebServiceGenerator.Util;
using Microwave.WebServiceModel.Application;
using Microwave.WebServiceModel.Domain;

namespace Microwave.WebServiceGenerator.SqlAdapter
{
    public class HangfireQueueBuilder
    {
        private readonly ClassBuilderUtil _classBuilderUtil;
        private readonly ConstructorBuilderUtil _constructorBuilderUtil;
        private readonly ListPropBuilderUtil _listPropBuilderUtil;
        private readonly string _nameSpace;
        private readonly NameSpaceBuilderUtil _nameSpaceBuilderUtil;
        private readonly PropertyBuilderUtil _propertyBuilderUtil;
        private NameBuilderUtil _nameBuilderUtil;

        public HangfireQueueBuilder(string nameSpace)
        {
            _nameSpace = nameSpace;
            _nameSpaceBuilderUtil = new NameSpaceBuilderUtil();
            _classBuilderUtil = new ClassBuilderUtil();
            _propertyBuilderUtil = new PropertyBuilderUtil();
            _listPropBuilderUtil = new ListPropBuilderUtil();
            _constructorBuilderUtil = new ConstructorBuilderUtil();
            _nameBuilderUtil = new NameBuilderUtil();
        }

        public CodeNamespace Build(HangfireQueueClass hangfireQueue)
        {
            var targetClass = _classBuilderUtil.Build(hangfireQueue.Name);
            var codeNamespace = _nameSpaceBuilderUtil.WithName($"{_nameSpace}").WithApplication().WithTask().WithDomain().WithList().WithLinq().WithEfCore().Build();
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
                Name = "AddEvents",
                ReturnType = new CodeTypeReference("async Task"),
                Attributes = MemberAttributes.Public | MemberAttributes.Final
            };

            method.Parameters.Add(new CodeParameterDeclarationExpression(new CodeTypeReference($"List<{new DomainEventBaseClass().Name}>"), "domainEvents"));

            //to lazy right now...
            method.Statements.Add(new CodeSnippetExpression(@"foreach (var domainEvent in domainEvents)
            {
                var jobsTriggereByEvent = RegisteredJobs.EventJobs.Where(tuple => domainEvent.GetType().ToString() == tuple.DomainType);
                foreach (var job in jobsTriggereByEvent)
                {
                    var combination = new EventAndJob(domainEvent, job.JobName);
                    Context.EventAndJobQueue.Add(combination);
                }
            }

            await Context.SaveChangesAsync()"));

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

            method.Parameters.Add(new CodeParameterDeclarationExpression(new CodeTypeReference($"List<{new EventAndJobClass().Name}>"), "handledEvents"));

            method.Statements.Add(new CodeSnippetExpression(@"Context.EventAndJobQueue.RemoveRange(handledEvents)"));
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

            method.Statements.Add(new CodeSnippetExpression("var eventList = await Context.EventAndJobQueue.Include(queue => queue.DomainEvent).Where(eve => eve.JobName == jobName).ToListAsync()"));
            method.Statements.Add(new CodeSnippetExpression("return eventList"));

            return method;
        }

        private static CodeIterationStatement CreateLoopOverEvents(AsyncDomainHook hook)
        {
            var codeWhile = new CodeIterationStatement();
            codeWhile.IncrementStatement = new CodeSnippetStatement("");
            codeWhile.InitStatement = new CodeSnippetStatement("");

            codeWhile.TestExpression = new CodeSnippetExpression("enumerator.MoveNext()");
            codeWhile.Statements.Add(new CodeSnippetExpression("var eventWrapper = enumerator.Current"));
            codeWhile.Statements.Add(new CodeSnippetExpression($"var domainEvent = ({hook.ClassType}{hook.MethodName}Event) eventWrapper.DomainEvent"));
            if (hook.IsCreateHook) codeWhile.Statements.Add(new CodeSnippetExpression($"var entity = await {hook.ClassType}Repository.Get{hook.ClassType}(domainEvent.Id)"));
            if (hook.IsCreateHook) codeWhile.Statements.Add(new CodeSnippetExpression($"var newCreateEvent = new {hook.ClassType}{hook.MethodName}Event(entity, domainEvent.EntityId)"));
            if (hook.IsCreateHook)
            {
                codeWhile.Statements.Add(new CodeSnippetExpression($"var hookResult = AsyncHook.Execute(newCreateEvent)"));
            }
            else
            {
                codeWhile.Statements.Add(new CodeSnippetExpression($"var hookResult = AsyncHook.Execute(domainEvent)"));
            }
            codeWhile.Statements.Add(CreateIfState());

            return codeWhile;
        }

        private static CodeConditionStatement CreateIfState()
        {
            var conditionalStatement = new CodeConditionStatement(
                new CodeSnippetExpression("hookResult.Ok"),
                new CodeExpressionStatement(new CodeSnippetExpression("handledEvents.Add(eventWrapper)")));
            return conditionalStatement;
        }
    }

    public class HangfireQueueClass : DomainClass
    {
        public HangfireQueueClass()
        {
            Name = "HangfireQueue";
            Properties = new List<Property>
            {
                new Property
                {
                    Name = "RegisteredJobs",
                    Type = "EventJobRegistration"
                },
                new Property
                {
                    Name = "Context",
                    Type = "HangfireContext"
                }
            };
        }
    }
}