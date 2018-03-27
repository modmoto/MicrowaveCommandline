using Microwave.LanguageParser.Lexer;

namespace Microwave.LanguageParser.ParseAutomat.DomainClasses
{
    public class DomainClassNameFoundState : ParseState
    {
        public DomainClassNameFoundState(MicrowaveLanguageParser microwaveLanguageParser) : base(microwaveLanguageParser)
        {
        }

        private ParseState BracketOpeneFound()
        {
            return new DomainClassOpenedState(MicrowaveLanguageParser);
        }

        public override ParseState Parse(DslToken token)
        {
            switch (token.TokenType)
            {
                case TokenType.ObjectBracketOpen:
                    return BracketOpeneFound();
                default:
                    throw new NoTransitionException(token);
            }
        }
    }
}