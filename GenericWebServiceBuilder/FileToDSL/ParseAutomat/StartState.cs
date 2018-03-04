using GenericWebServiceBuilder.FileToDSL.Lexer;
using GenericWebServiceBuilder.FileToDSL.ParseAutomat.DomainClasses;

namespace GenericWebServiceBuilder.FileToDSL.ParseAutomat
{
    public class StartState : ParseState
    {
        public StartState(Parser parser) : base(parser)
        {
        }

        private ParseState DomainClassIdentifierFound()
        {
            return new DomainClassIdentifierFoundState(Parser);
        }

        public override ParseState Parse(DslToken token)
        {
            switch (token.TokenType)
            {
                case TokenType.DomainClass:
                    return DomainClassIdentifierFound();
                default:
                    throw new NoTransitionException(token);
            }
        }
    }
}