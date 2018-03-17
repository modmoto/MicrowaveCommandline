using System.CodeDom;
using System.Collections.Generic;
using DslModel.Application;
using DslModel.Domain;
using DslModelToCSharp.Domain;

namespace DslModelToCSharp.Application
{
    public class EventStoreBuilder
    {
        private readonly ClassBuilder _classBuilder;
        private readonly ConstBuilder _constBuilder;
        private readonly ListPropBuilder _listPropBuilder;
        private readonly string _nameSpace;
        private readonly NameSpaceBuilder _nameSpaceBuilder;
        private readonly PropBuilder _propBuilder;

        public EventStoreBuilder(string nameSpace)
        {
            _nameSpace = nameSpace;
            _nameSpaceBuilder = new NameSpaceBuilder();
            _classBuilder = new ClassBuilder();
            _propBuilder = new PropBuilder();
            _listPropBuilder = new ListPropBuilder();
            _constBuilder = new ConstBuilder();
        }

        public CodeNamespace Build(EventStore eventStore, IList<SynchronousDomainHook> hooks)
        {
            var targetClass = _classBuilder.Build(eventStore.Name);
            var nameSpace = _nameSpaceBuilder.BuildWithLinq(_nameSpace);

            _propBuilder.BuildWithoutSet(targetClass, eventStore.Properties);
            _listPropBuilder.Build(targetClass, eventStore.ListProperties);
            var constructor = _constBuilder.BuildPublic(eventStore.Properties);

            foreach (var hook in hooks)
            {
                var hookName = hook.Name + "Hook";
                nameSpace.Imports.Add(new CodeNamespaceImport($"Application.{hook.ClassType}s.Hooks"));
                constructor.Parameters.Add(new CodeParameterDeclarationExpression(hookName, hookName));
                constructor.Statements.Add(new CodeSnippetExpression($"DomainHooks.Add({hookName})"));
            }

            targetClass.Members.Add(constructor);

            var methodList = new List<CodeMemberMethod>();
            foreach (var eventStoreMethod in eventStore.Methods)
            {
                var method = new CodeMemberMethod();
                method.Name = eventStoreMethod.Name;
                method.ReturnType = new CodeTypeReference(eventStoreMethod.ReturnType);
                method.Attributes = MemberAttributes.Public | MemberAttributes.Final;
                foreach (var parameter in eventStoreMethod.Parameters)
                    method.Parameters.Add(new CodeParameterDeclarationExpression(new CodeTypeReference(parameter.Type),
                        parameter.Name));

                methodList.Add(method);
            }

            var codeMemberMethod = methodList[0];
            codeMemberMethod.Statements.Add(new CodeSnippetExpression("var enumerator = domainEvents.GetEnumerator()"));

            var codeWhile = new CodeIterationStatement();
            codeWhile.IncrementStatement = new CodeSnippetStatement("");
            codeWhile.InitStatement = new CodeSnippetStatement("");

            codeWhile.TestExpression = new CodeSnippetExpression("enumerator.MoveNext()");
            codeWhile.Statements.Add(new CodeSnippetExpression("var domainEvent = enumerator.Current"));
            codeWhile.Statements.Add(new CodeSnippetExpression(
                "var domainHooks = DomainHooks.Where(hook => hook.EventType == domainEvent.GetType())"));
            codeWhile.Statements.Add(new CodeSnippetExpression("var enumeratorHook = domainHooks.GetEnumerator()"));

            var codeInnerWhile = CreateInnerWhile();

            codeWhile.Statements.Add(codeInnerWhile);

            codeMemberMethod.Statements.Add(codeWhile);
            codeMemberMethod.Statements.Add(
                new CodeSnippetExpression("await EventStoreRepository.AddEvents(domainEvents)"));
            codeMemberMethod.Statements.Add(
                new CodeSnippetExpression($"return {new HookResultBaseClass().Name}.OkResult()"));

            targetClass.Members.Add(methodList[0]);

            nameSpace.Types.Add(targetClass);
            return nameSpace;
        }

        private static CodeIterationStatement CreateInnerWhile()
        {
            var codeInnerWhile = new CodeIterationStatement();
            codeInnerWhile.TestExpression = new CodeSnippetExpression("enumeratorHook.MoveNext()");
            codeInnerWhile.IncrementStatement = new CodeSnippetStatement("");
            codeInnerWhile.InitStatement = new CodeSnippetStatement("");
            codeInnerWhile.Statements.Add(new CodeSnippetExpression("var domainHook = enumeratorHook.Current"));
            codeInnerWhile.Statements.Add(new CodeSnippetExpression(
                $"var validationResult = domainHook.{new DomainHookBaseClass().Methods[0].Name}(domainEvent)"));

            codeInnerWhile.Statements.Add(CreateIfState());
            return codeInnerWhile;
        }

        private static CodeConditionStatement CreateIfState()
        {
            var conditionalStatement = new CodeConditionStatement(
                new CodeSnippetExpression("!validationResult.Ok"),
                new CodeExpressionStatement(new CodeSnippetExpression("return validationResult")));
            return conditionalStatement;
        }
    }
}