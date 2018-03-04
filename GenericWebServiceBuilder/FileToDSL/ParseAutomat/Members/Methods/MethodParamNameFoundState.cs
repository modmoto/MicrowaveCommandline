using GenericWebServiceBuilder.DomainSpecificGrammar;
using GenericWebServiceBuilder.FileToDSL.Lexer;

namespace GenericWebServiceBuilder.FileToDSL.ParseAutomat.Members.Methods
{
    public class MethodParamNameFoundState : ParseState
    {
        public MethodParamNameFoundState(Parser parser) : base(parser)
        {
        }

        public override ParseState Parse(DslToken token)
        {
            switch (token.TokenType)
            {
                case TokenType.TypeDefSeparator:
                    return MethodParamTypeDefSeparatorFound(token);
                default:
                    throw new NoTransitionException(token);
            }
        }

        private ParseState MethodParamTypeDefSeparatorFound(DslToken token)
        {
            return new MethodParamTypeDefSeparatorFoundState(Parser);
        }
    }
}