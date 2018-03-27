using Microwave.LanguageParser.Lexer;

namespace Microwave.LanguageParser.ParseAutomat.Members.EventHooks
{
    internal class SynchronousDomainHookNameFoundState : ParseState
    {
        public SynchronousDomainHookNameFoundState(MicrowaveLanguageParser microwaveLanguageParser) : base(microwaveLanguageParser)
        {
        }

        public override ParseState Parse(DslToken token)
        {
            switch (token.TokenType)
            {
                case TokenType.DomainHookOn:
                    return SynchronousDomainHookOnEventFound();
                default:
                    throw new NoTransitionException(token);
            }
        }

        private ParseState SynchronousDomainHookOnEventFound()
        {
            return new SynchronousDomainHookOnEventFoundState(MicrowaveLanguageParser);
        }
    }
}