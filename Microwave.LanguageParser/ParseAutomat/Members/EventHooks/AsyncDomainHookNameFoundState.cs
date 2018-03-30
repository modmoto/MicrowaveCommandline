using Microwave.LanguageParser.Lexer;

namespace Microwave.LanguageParser.ParseAutomat.Members.EventHooks
{
    internal class AsyncDomainHookNameFoundState : ParseState
    {
        public AsyncDomainHookNameFoundState(MicrowaveLanguageParser microwaveLanguageParser) : base(microwaveLanguageParser)
        {
        }

        public override ParseState Parse(DslToken token)
        {
            switch (token.TokenType)
            {
                case TokenType.DomainHookOn:
                    return AsyncDomainHookOnEventFound();
                default:
                    throw new NoTransitionException(token);
            }
        }

        private ParseState AsyncDomainHookOnEventFound()
        {
            return new AsyncDomainHookOnEventFoundState(MicrowaveLanguageParser);
        }
    }
}