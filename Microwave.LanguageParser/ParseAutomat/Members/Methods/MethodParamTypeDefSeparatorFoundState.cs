using Microwave.LanguageParser.Lexer;

namespace Microwave.LanguageParser.ParseAutomat.Members.Methods
{
    internal class MethodParamTypeDefSeparatorFoundState : ParseState
    {
        public MethodParamTypeDefSeparatorFoundState(MicrowaveLanguageParser microwaveLanguageParser) : base(microwaveLanguageParser)
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
            MicrowaveLanguageParser.CurrentParamIsLoadParam = true;
            return new MethodParamTypeDefSeparatorFoundState(MicrowaveLanguageParser);
        }

        private ParseState MethodParamTypeFound(DslToken token)
        {
            MicrowaveLanguageParser.CurrentParam.Type = token.Value;
            if (MicrowaveLanguageParser.CurrentParamIsLoadParam)
            {
                MicrowaveLanguageParser.CurrentMethod.LoadParameters.Add(MicrowaveLanguageParser.CurrentParam);
            }
            else
            {
                MicrowaveLanguageParser.CurrentMethod.Parameters.Add(MicrowaveLanguageParser.CurrentParam);
            }

            MicrowaveLanguageParser.CurrentParamIsLoadParam = false;
            return new MethodSingleParamFinishedState(MicrowaveLanguageParser);
        }
    }
}