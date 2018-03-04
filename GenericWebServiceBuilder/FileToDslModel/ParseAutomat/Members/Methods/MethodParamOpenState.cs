using GenericWebServiceBuilder.DslModel;
using GenericWebServiceBuilder.FileToDslModel.Lexer;

namespace GenericWebServiceBuilder.FileToDslModel.ParseAutomat.Members.Methods
{
    internal class MethodParamOpenState : ParseState
    {
        public MethodParamOpenState(Parser parser) : base(parser)
        {
        }

        public override ParseState Parse(DslToken token)
        {
            switch (token.TokenType)
            {
                case TokenType.ParameterBracketClose:
                    return MethodParamClosedStateFound();
                case TokenType.Value:
                    return MethodParamNameFound(token);
                default:
                    throw new NoTransitionException(token);
            }
        }

        private ParseState MethodParamNameFound(DslToken token)
        {
            Parser.CurrentParam = new Parameter {Name = token.Value};
            return new MethodParamNameFoundState(Parser);
        }

        private ParseState MethodParamClosedStateFound()
        {
            return new MethodParamClosedState(Parser);
        }
    }
}