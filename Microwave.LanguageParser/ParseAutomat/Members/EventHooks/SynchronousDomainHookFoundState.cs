using Microwave.LanguageModel;
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
                case TokenType.Value:
                    return SynchronousDomainHookNameFound(token);
                default:
                    throw new NoTransitionException(token);
            }
        }

        private ParseState SynchronousDomainHookNameFound(DslToken token)
        {
            MicrowaveLanguageParser.CurrentSynchronousDomainHook = new SynchronousDomainHook
            {
                Name = token.Value
            };


            return new SynchronousDomainHookNameFoundState(MicrowaveLanguageParser);
        }
    }
}