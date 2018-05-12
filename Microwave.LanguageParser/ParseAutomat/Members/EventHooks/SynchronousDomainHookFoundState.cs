using Microwave.LanguageParser.Lexer;

namespace Microwave.LanguageParser.ParseAutomat.Members.EventHooks
{
    internal class SynchronousDomainHookFoundState : ParseState
    {
        public SynchronousDomainHookFoundState(MicrowaveLanguageParser microwaveLanguageParser) : base(microwaveLanguageParser)
        {
        }

        public override ParseState Parse(DslToken token)
        {
            switch (token.TokenType)
            {
                case TokenType.DomainHookOn:
                    return AsyncDomainHookOnFound();
                default:
                    throw new NoTransitionException(token);
            }
        }

        private ParseState AsyncDomainHookOnFound()
        {
            return new SynchronousDomainHookOnEventFoundState(MicrowaveLanguageParser);
        }
    }
}