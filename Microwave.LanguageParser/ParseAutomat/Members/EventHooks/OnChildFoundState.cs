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
            MicrowaveLanguageParser.CurrentOnChildHookMethod.Name = MicrowaveLanguageParser.CurrentMemberName;
            MicrowaveLanguageParser.CurrentOnChildHookMethod.ContainingClassName = MicrowaveLanguageParser.CurrentClass.Name;
            MicrowaveLanguageParser.CurrentOnChildHookMethod.OriginFieldName = token.Value.Split('.')[0];
            MicrowaveLanguageParser.CurrentOnChildHookMethod.MethodName = token.Value.Split('.')[1];

            MicrowaveLanguageParser.CurrentClass.ChildHookMethods.Add(MicrowaveLanguageParser.CurrentOnChildHookMethod);

            return new DomainClassOpenedState(MicrowaveLanguageParser);
        }
    }
}