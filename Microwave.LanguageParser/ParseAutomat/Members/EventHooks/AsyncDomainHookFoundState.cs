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
                case TokenType.Value:
                    return AsyncDomainHookNameFound(token);
                default:
                    throw new NoTransitionException(token);
            }
        }

        private ParseState AsyncDomainHookNameFound(DslToken token)
        {
            MicrowaveLanguageParser.CurrentAsyncDomainHook = new AsyncDomainHook
            {
                Name = token.Value
            };


            return new AsyncDomainHookNameFoundState(MicrowaveLanguageParser);
        }
    }
}