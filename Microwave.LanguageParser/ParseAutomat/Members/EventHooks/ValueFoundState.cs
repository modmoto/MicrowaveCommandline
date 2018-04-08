using Microwave.LanguageModel;
using Microwave.LanguageParser.Lexer;

namespace Microwave.LanguageParser.ParseAutomat.Members.EventHooks
{
    internal class ValueFoundState : ParseState
    {
        public ValueFoundState(MicrowaveLanguageParser microwaveLanguageParser) : base(microwaveLanguageParser)
        {
        }

        public override ParseState Parse(DslToken token)
        {
            switch (token.TokenType)
            {
                case TokenType.SynchronouslyToken:
                    return SynchronousDomainHookFound();
                case TokenType.AsynchronouslyToken:
                    return AsyncDomainHookFound();
                default:
                    throw new NoTransitionException(token);
            }
        }

        private ParseState SynchronousDomainHookFound()
        {
            MicrowaveLanguageParser.CurrentSynchronousDomainHook = new SynchronousDomainHook
            {
                Name = MicrowaveLanguageParser.CurrentFoundValue
            };

            return new SynchronousDomainHookFoundState(MicrowaveLanguageParser);
        }

        private ParseState AsyncDomainHookFound()
        {
            MicrowaveLanguageParser.CurrentAsyncDomainHook = new AsyncDomainHook
            {
                Name = MicrowaveLanguageParser.CurrentFoundValue
            };

            return new AsyncDomainHookFoundState(MicrowaveLanguageParser);
        }
    }
}