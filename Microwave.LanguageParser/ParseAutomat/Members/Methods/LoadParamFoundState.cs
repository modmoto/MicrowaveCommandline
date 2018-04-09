using Microwave.LanguageParser.Lexer;

namespace Microwave.LanguageParser.ParseAutomat.Members.Methods
{
    internal class LoadParamFoundState : ParseState
    {
        public LoadParamFoundState(MicrowaveLanguageParser microwaveLanguageParser) : base(microwaveLanguageParser)
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
            MicrowaveLanguageParser.CurrentParam.Type = token.Value;
            MicrowaveLanguageParser.CurrentMethod.LoadParameters.Add(MicrowaveLanguageParser.CurrentParam);
            return new MethodSingleParamFinishedState(MicrowaveLanguageParser);
        }
    }
}