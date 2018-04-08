using Microwave.LanguageModel;
using Microwave.LanguageParser.Lexer;

namespace Microwave.LanguageParser.ParseAutomat.Members.EventHooks
{
    internal class AsyncDomainHookFoundState : ParseState
    {
        public AsyncDomainHookFoundState(MicrowaveLanguageParser microwaveLanguageParser) : base(microwaveLanguageParser)
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
            return new AsyncDomainHookOnEventFoundState(MicrowaveLanguageParser);
        }
    }
}