using GenericWebServiceBuilder.DomainSpecificGrammar;
using GenericWebServiceBuilder.FileToDSL.Lexer;

namespace GenericWebServiceBuilder.FileToDSL.ParseAutomat
{
    public class DomainClassIdentifierFoundState : ParseState
    {
        public DomainClassIdentifierFoundState(Parser parser) : base(parser)
        {
        }

        private ParseState DomainClassNameFound(DslToken token)
        {
            var domainClass = new DomainClass
            {
                Name = token.Value
            };
            Parser.CurrentClass = domainClass;
            var domainClassNameFoundState = new DomainClassNameFoundState(Parser);
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