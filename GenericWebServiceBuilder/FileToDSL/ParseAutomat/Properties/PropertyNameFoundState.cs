using GenericWebServiceBuilder.DomainSpecificGrammar;
using GenericWebServiceBuilder.FileToDSL.Lexer;

namespace GenericWebServiceBuilder.FileToDSL.ParseAutomat.Properties
{
    public class PropertyNameFoundState : ParseState
    {
        public PropertyNameFoundState(Parser parser) : base(parser)
        {
        }

        public override ParseState Parse(DslToken token)
        {
            switch (token.TokenType)
            {
                case TokenType.TypeDefSeparator:
                    return PropertySeparatorFound(token);
                default:
                    throw new NoTransitionException(token);
            }
        }

        private ParseState PropertySeparatorFound(DslToken token)
        {
            return new PropertySeparatorFoundState(Parser);
        }
    }
}