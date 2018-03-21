using FileToDslModel.Lexer;

namespace FileToDslModel.ParseAutomat.DomainClasses
{
    internal class ParamCreateMethodStartedState : ParseState
    {
        public ParamCreateMethodStartedState(Parser parser) : base(parser)
        {
        }

        public override ParseState Parse(DslToken token)
        {
            switch (token.TokenType)
            {
                case TokenType.TypeDefSeparator:
                    return ParamCreateMethodTypeDefFound();
                default:
                    throw new NoTransitionException(token);
            }
        }

        private ParseState ParamCreateMethodTypeDefFound()
        {
            return new ParamCreateMethodTypeDefFoundState(Parser);
        }
    }
}