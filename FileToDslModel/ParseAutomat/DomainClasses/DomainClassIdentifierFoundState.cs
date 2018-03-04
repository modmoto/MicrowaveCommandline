using DslModel;
using FileToDslModel.Lexer;

namespace FileToDslModel.ParseAutomat.DomainClasses
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