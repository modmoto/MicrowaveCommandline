using FileToDslModel.Lexer;

namespace FileToDslModel.ParseAutomat.DomainClasses
{
    internal class CreateMethodFoundState : ParseState
    {
        public CreateMethodFoundState(Parser parser) : base(parser)
        {
        }

        public override ParseState Parse(DslToken token)
        {
            switch (token.TokenType)
            {
                case TokenType.ParameterBracketOpen:
                    return CreateParamsStarted();
                default:
                    throw new NoTransitionException(token);
            }
        }

        private ParseState CreateParamsStarted()
        {
            return new CreateParamsStartedState(Parser);
        }
    }
}