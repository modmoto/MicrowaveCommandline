using GenericWebServiceBuilder.DomainSpecificGrammar;
using GenericWebServiceBuilder.FileToDSL.Lexer;
using GenericWebServiceBuilder.FileToDSL.ParseAutomat.Properties;

namespace GenericWebServiceBuilder.FileToDSL.ParseAutomat
{
    public class DomainClassOpenedState : ParseState
    {
        public DomainClassOpenedState(Parser parser) : base(parser)
        {
        }

        private ParseState DomainClassClosed()
        {
            Parser.Classes.Add(Parser.CurrentClass);
            return new StartState(Parser);
        }

        public override ParseState Parse(DslToken token)
        {
            switch (token.TokenType)
            {
                case TokenType.ObjectBracketClose:
                    return DomainClassClosed();
                case TokenType.Value:
                    return PropertyStarted(token);
                default:
                    throw new NoTransitionException(token);
            }
        }

        private ParseState PropertyStarted(DslToken token)
        {
            Parser.CurrentProperty = new Property { Name = token.Value };
            return new PropertyNameFoundState(Parser);
        }
    }
}