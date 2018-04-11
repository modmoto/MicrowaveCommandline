using Microwave.LanguageModel;
using Microwave.LanguageParser.Lexer;
using Microwave.LanguageParser.ParseAutomat.DomainClasses;

namespace Microwave.LanguageParser.ParseAutomat.Members.EventHooks
{
    internal class OnChildFoundState : ParseState
    {
        public OnChildFoundState(MicrowaveLanguageParser microwaveLanguageParser) : base(microwaveLanguageParser)
        {
        }

        public override ParseState Parse(DslToken token)
        {
            switch (token.TokenType)
            {
                case TokenType.DomainHookEventDefinition:
                    return DomainHookEventDefinitionFound(token);
                default:
                    throw new NoTransitionException(token);
            }
        }

        private ParseState DomainHookEventDefinitionFound(DslToken token)
        {
            MicrowaveLanguageParser.CurrentOnChildHook.ClassType = token.Value.Split('.')[0];
            MicrowaveLanguageParser.CurrentOnChildHook.MethodName = token.Value.Split('.')[1];

            MicrowaveLanguageParser.CurrentOnChildHookMethod.Name = $@"{MicrowaveLanguageParser.CurrentMemberName}_On{token.Value.Replace(".", "")}";
            MicrowaveLanguageParser.CurrentOnChildHookMethod.Parameters.Add(new Parameter {Name = token.Value.Replace(".", ""), Type = token.Value.Replace(".", "") });

            MicrowaveLanguageParser.CurrentClass.ChildHookMethods.Add(MicrowaveLanguageParser.CurrentOnChildHookMethod);
            MicrowaveLanguageParser.ChildHooks.Add(MicrowaveLanguageParser.CurrentOnChildHook);

            return new DomainClassOpenedState(MicrowaveLanguageParser);
        }
    }
}