using GenericWebServiceBuilder.DslModel;
using GenericWebServiceBuilder.FileToDSL.Lexer;

namespace GenericWebServiceBuilder.FileToDSL.ParseAutomat.Members.Methods
{
    internal class AdditionalParamState : ParseState
    {
        public AdditionalParamState(Parser parser) : base(parser)
        {
        }

        public override ParseState Parse(DslToken token)
        {
            switch (token.TokenType)
            {
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
    }
}