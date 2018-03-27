using Microwave.LanguageParser.Lexer;

namespace Microwave.LanguageParser.ParseAutomat.Members.Methods
{
    public class MethodParamNameFoundState : ParseState
    {
        public MethodParamNameFoundState(MicrowaveLanguageParser microwaveLanguageParser) : base(microwaveLanguageParser)
        {
        }

        public override ParseState Parse(DslToken token)
        {
            switch (token.TokenType)
            {
                case TokenType.TypeDefSeparator:
                    return MethodParamTypeDefSeparatorFound();
                default:
                    throw new NoTransitionException(token);
            }
        }

        private ParseState MethodParamTypeDefSeparatorFound()
        {
            return new MethodParamTypeDefSeparatorFoundState(MicrowaveLanguageParser);
        }
    }
}