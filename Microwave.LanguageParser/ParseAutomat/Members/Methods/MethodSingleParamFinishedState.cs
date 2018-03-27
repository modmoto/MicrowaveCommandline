using Microwave.LanguageParser.Lexer;

namespace Microwave.LanguageParser.ParseAutomat.Members.Methods
{
    internal class MethodSingleParamFinishedState : ParseState
    {
        public MethodSingleParamFinishedState(MicrowaveLanguageParser microwaveLanguageParser) : base(microwaveLanguageParser)
        {
        }

        public override ParseState Parse(DslToken token)
        {
            switch (token.TokenType)
            {
                case TokenType.ParamSeparator:
                    return AdditionalParamStateFound();
                case TokenType.ParameterBracketClose:
                    return MethodParamClosedStateFound();
                default:
                    throw new NoTransitionException(token);
            }
        }

        private ParseState MethodParamClosedStateFound()
        {
            return new MethodParamEndedState(MicrowaveLanguageParser);
        }

        private ParseState AdditionalParamStateFound()
        {
            return new AdditionalMethodParamState(MicrowaveLanguageParser);
        }
    }
}