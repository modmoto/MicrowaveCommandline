using Microwave.LanguageModel.Domain;
using Microwave.LanguageParser.Lexer;

namespace Microwave.LanguageParser.ParseAutomat.Members.Methods
{
    internal class MethodParamOpenState : ParseState
    {
        public MethodParamOpenState(MicrowaveLanguageParser microwaveLanguageParser) : base(microwaveLanguageParser)
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
            MicrowaveLanguageParser.CurrentParam = new Parameter {Name = token.Value};
            return new MethodParamNameFoundState(MicrowaveLanguageParser);
        }

        private ParseState MethodParamClosedStateFound()
        {
            return new MethodParamEndedState(MicrowaveLanguageParser);
        }
    }
}