using GenericWebServiceBuilder.FileToDslModel.Lexer;

namespace GenericWebServiceBuilder.FileToDslModel.ParseAutomat.Members.Methods
{
    internal class MethodParamTypeDefSeparatorFoundState : ParseState
    {
        public MethodParamTypeDefSeparatorFoundState(Parser parser) : base(parser)
        {
        }

        public override ParseState Parse(DslToken token)
        {
            switch (token.TokenType)
            {
                case TokenType.Value:
                    return MethodParamTypeFound(token);
                default:
                    throw new NoTransitionException(token);
            }
        }

        private ParseState MethodParamTypeFound(DslToken token)
        {
            Parser.CurrentParam.Type = token.Value;
            Parser.CurrentMethod.Parameters.Add(Parser.CurrentParam);
            return new MethodSingleParamFinishedState(Parser);
        }
    }
}