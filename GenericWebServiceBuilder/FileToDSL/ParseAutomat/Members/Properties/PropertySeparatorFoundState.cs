using GenericWebServiceBuilder.FileToDSL.Lexer;

namespace GenericWebServiceBuilder.FileToDSL.ParseAutomat.Properties
{
    public class PropertySeparatorFoundState : ParseState
    {
        public PropertySeparatorFoundState(Parser parser) : base(parser)
        {
        }

        public override ParseState Parse(DslToken token)
        {
            switch (token.TokenType)
            {
                case TokenType.Value:
                    return PropertyTypeDefFound(token);
                default:
                    throw new NoTransitionException(token);
            }
        }

        private ParseState PropertyTypeDefFound(DslToken token)
        {
            Parser.CurrentProperty.Type = token.Value;
            Parser.CurrentClass.Propteries.Add(Parser.CurrentProperty);
            return new DomainClassOpenedState(Parser);
        }
    }
}