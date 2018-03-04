using GenericWebServiceBuilder.FileToDSL.Lexer;

namespace GenericWebServiceBuilder.FileToDSL.ParseAutomat.Members.Methods
{
    internal class MethodParamNameFoundState : ParseState
    {
        public MethodParamNameFoundState(Parser parser) : base(parser)
        {
        }

        public override ParseState Parse(DslToken token)
        {
            switch (token.TokenType)
            {
                case TokenType.TypeDefSeparator:
                    return MethodParamTypeDefSeparatorFound();
                default:
                    throw new NoTransitionException(token);
            }
        }

        private ParseState MethodParamTypeDefSeparatorFound()
        {
            return new MethodParamTypeDefSeparatorFoundState(Parser);
        }
    }
}