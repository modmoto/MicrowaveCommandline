using System.CodeDom;
using System.Collections.Generic;
using Microwave.LanguageModel;
using Microwave.WebServiceGenerator.Util;
using Microwave.WebServiceModel.Application;

namespace Microwave.WebServiceGenerator.Application
{
    public class AsyncHookCreateEventHandlerBuilder
    {
        private readonly ClassBuilderUtil _classBuilderUtil;
        private readonly ConstructorBuilderUtil _constructorBuilderUtil;
        private readonly ListPropBuilderUtil _listPropBuilderUtil;
        private readonly string _nameSpace;
        private readonly NameSpaceBuilderUtil _nameSpaceBuilderUtil;
        private readonly PropertyBuilderUtil _propertyBuilderUtil;
        private NameBuilderUtil _nameBuilderUtil;

        public AsyncHookCreateEventHandlerBuilder(string nameSpace)
        {
            _nameSpace = nameSpace;
            _nameSpaceBuilderUtil = new NameSpaceBuilderUtil();
            _classBuilderUtil = new ClassBuilderUtil();
            _propertyBuilderUtil = new PropertyBuilderUtil();
            _listPropBuilderUtil = new ListPropBuilderUtil();
            _constructorBuilderUtil = new ConstructorBuilderUtil();
            _nameBuilderUtil = new NameBuilderUtil();
        }

        public CodeNamespace Build(AsyncDomainHook hook)
        {
            var targetClass = _classBuilderUtil.Build(_nameBuilderUtil.AsyncEventHookHandlerName(hook));
            var codeNamespace = _nameSpaceBuilderUtil.WithName($"{_nameSpace}.{hook.ClassType}s.AsyncHooks").WithApplication().WithTask().WithList().WithDomainEntityNameSpace(hook.ClassType).Build();
            codeNamespace.Types.Add(targetClass);

            var handler = new AsyncHookCreateEventHandler(hook);
            _propertyBuilderUtil.Build(targetClass, handler.Properties);
            var constructor = _constructorBuilderUtil.BuildPublic(handler.Properties);
            targetClass.Members.Add(constructor);

            var method = new CodeMemberMethod();
            method.Name = "Run";
            method.ReturnType = new CodeTypeReference("async Task");
            method.Attributes = MemberAttributes.Public | MemberAttributes.Final;

            method.Statements.Add(new CodeSnippetExpression($"var events = await {handler.Properties[1].Name}.GetEvents(\"{hook.Name}\")"));
            method.Statements.Add(new CodeSnippetExpression($"var handledEvents = new List<{new EventAndJobClass().Name}>()"));

            method.Statements.Add(new CodeSnippetExpression("var enumerator = events.GetEnumerator()"));

            var codeIterationStatement = CreateLoopOverEvents(hook);
            method.Statements.Add(codeIterationStatement);

            method.Statements.Add(
                new CodeSnippetExpression($"await {handler.Properties[1].Name}.RemoveEventsFromQueue(handledEvents)"));
            targetClass.Members.Add(method);
            return codeNamespace;
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
                codeWhile.Statements.Add(new CodeSnippetExpression($"var hookResult = await AsyncHook.Execute(newCreateEvent)"));
            }
            else
            {
                codeWhile.Statements.Add(new CodeSnippetExpression($"var hookResult = await AsyncHook.Execute(domainEvent)"));
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

    public class AsyncHookCreateEventHandler : DomainClass
    {
        public AsyncHookCreateEventHandler(AsyncDomainHook hook)
        {
            var nameBuilderUtil = new NameBuilderUtil();
            Properties = new List<Property>
            {
                new Property {Name = "AsyncHook", Type = nameBuilderUtil.AsyncEventHookName(hook)},
                new Property {Name = "HangfireQueue", Type = "IHangfireQueue"}
            };

            if (hook.IsCreateHook) Properties.Add(new Property { Name = $"{hook.ClassType}Repository", Type = $"I{hook.ClassType}Repository" });
        }
    }
}