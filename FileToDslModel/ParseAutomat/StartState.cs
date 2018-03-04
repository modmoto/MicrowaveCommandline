using FileToDslModel.Lexer;
using FileToDslModel.ParseAutomat.DomainClasses;

namespace FileToDslModel.ParseAutomat
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