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
                case TokenType.Value:
                    return ValueFound(token);
                default:
                    throw new NoTransitionException(token);
            }
        }

        private ParseState ValueFound(DslToken token)
        {
            MicrowaveLanguageParser.CurrentFoundValue = token.Value;
            return new ValueFoundState(MicrowaveLanguageParser);
        }

        private ParseState DomainClassIdentifierFound()
        {
            return new DomainClassIdentifierFoundState(MicrowaveLanguageParser);
        }
    }
}