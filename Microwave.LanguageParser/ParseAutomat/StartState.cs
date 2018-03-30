using Microwave.LanguageParser.Lexer;
using Microwave.LanguageParser.ParseAutomat.DomainClasses;
using Microwave.LanguageParser.ParseAutomat.Members.EventHooks;

namespace Microwave.LanguageParser.ParseAutomat
{
    public class StartState : ParseState
    {
        public StartState(MicrowaveLanguageParser microwaveLanguageParser) : base(microwaveLanguageParser)
        {
        }

        public override ParseState Parse(DslToken token)
        {
            switch (token.TokenType)
            {
                case TokenType.DomainClass:
                    return DomainClassIdentifierFound();
                case TokenType.SynchronousDomainHook:
                    return SynchronousDomainHookFound();
                case TokenType.AsyncDomainHook:
                    return AsyncDomainHookFound();
                default:
                    throw new NoTransitionException(token);
            }
        }

        private ParseState SynchronousDomainHookFound()
        {
            return new SynchronousDomainHookFoundState(MicrowaveLanguageParser);
        }

        private ParseState AsyncDomainHookFound()
        {
            return new AsyncDomainHookFoundState(MicrowaveLanguageParser);
        }

        private ParseState DomainClassIdentifierFound()
        {
            return new DomainClassIdentifierFoundState(MicrowaveLanguageParser);
        }
    }
}