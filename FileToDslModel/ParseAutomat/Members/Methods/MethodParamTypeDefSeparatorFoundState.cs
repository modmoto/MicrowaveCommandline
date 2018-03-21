using FileToDslModel.Lexer;

namespace FileToDslModel.ParseAutomat.Members.Methods
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
                case TokenType.LoadToken:
                    return LoadTypeFound(token);
                default:
                    throw new NoTransitionException(token);
            }
        }

        private ParseState LoadTypeFound(DslToken token)
        {
            Parser.CurrentParamIsLoadParam = true;
            return new MethodParamTypeDefSeparatorFoundState(Parser);
        }

        private ParseState MethodParamTypeFound(DslToken token)
        {
            Parser.CurrentParam.Type = token.Value;
            if (Parser.CurrentParamIsLoadParam)
            {
                Parser.CurrentMethod.LoadParameters.Add(Parser.CurrentParam);
            }
            else
            {
                Parser.CurrentMethod.Parameters.Add(Parser.CurrentParam);
            }
            return new MethodSingleParamFinishedState(Parser);
        }
    }
}