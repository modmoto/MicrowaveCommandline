using Microwave.LanguageModel;
using Microwave.LanguageParser.Lexer;

namespace Microwave.LanguageParser.ParseAutomat.DomainClasses
{
    public class DomainClassIdentifierFoundState : ParseState
    {
        public DomainClassIdentifierFoundState(MicrowaveLanguageParser microwaveLanguageParser) : base(microwaveLanguageParser)
        {
        }

        private ParseState DomainClassNameFound(DslToken token)
        {
            var domainClass = new DomainClass
            {
                Name = token.Value
            };
            MicrowaveLanguageParser.CurrentClass = domainClass;
            var domainClassNameFoundState = new DomainClassNameFoundState(MicrowaveLanguageParser);
            return domainClassNameFoundState;
        }

        public override ParseState Parse(DslToken token)
        {
            switch (token.TokenType)
            {
                case TokenType.Value:
                    return DomainClassNameFound(token);
                default:
                    throw new NoTransitionException(token);
            }
        }
    }
}