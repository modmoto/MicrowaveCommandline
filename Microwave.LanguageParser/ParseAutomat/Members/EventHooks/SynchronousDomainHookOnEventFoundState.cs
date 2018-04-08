using Microwave.LanguageParser.Lexer;

namespace Microwave.LanguageParser.ParseAutomat.Members.EventHooks
{
    internal class SynchronousDomainHookOnEventFoundState : ParseState
    {
        public SynchronousDomainHookOnEventFoundState(MicrowaveLanguageParser microwaveLanguageParser) : base(microwaveLanguageParser)
        {
        }

        public override ParseState Parse(DslToken token)
        {
            switch (token.TokenType)
            {
                case TokenType.DomainHookEventDefinition:
                    return SynchronousDomainHookEventFound(token);
                default:
                    throw new NoTransitionException(token);
            }
        }

        private ParseState SynchronousDomainHookEventFound(DslToken token)
        {
            var strings = token.Value.Split(".");
            MicrowaveLanguageParser.CurrentSynchronousDomainHook.ClassType = strings[0];
            MicrowaveLanguageParser.CurrentSynchronousDomainHook.MethodName = strings[1];
           
            MicrowaveLanguageParser.SynchronousDomainHooks.Add(MicrowaveLanguageParser.CurrentSynchronousDomainHook);

            return new StartState(MicrowaveLanguageParser);
        }
    }
}