using FileToDslModel.Lexer;

namespace FileToDslModel.ParseAutomat.DomainClasses
{
    internal class ParamCreateMethodTypeDefFoundState : ParseState
    {
        public ParamCreateMethodTypeDefFoundState(Parser parser) : base(parser)
        {
        }

        public override ParseState Parse(DslToken token)
        {
            switch (token.TokenType)
            {
                case TokenType.Value:
                    return ParamCreateMethodTypeFound(token);
                default:
                    throw new NoTransitionException(token);
            }
        }

        private ParseState ParamCreateMethodTypeFound(DslToken token)
        {
            Parser.CurrentParam.Type = token.Value;
            Parser.CurrentCreateMethod.Parameters.Add(Parser.CurrentParam);
            return new PropertyCreateMethodTypeFoundState(Parser);
        }
    }
}