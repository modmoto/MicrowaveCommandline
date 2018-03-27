using Microwave.LanguageParser.Lexer;

namespace Microwave.LanguageParser.ParseAutomat.DomainClasses
{
    internal class ParamCreateMethodStartedState : ParseState
    {
        public ParamCreateMethodStartedState(MicrowaveLanguageParser microwaveLanguageParser) : base(microwaveLanguageParser)
        {
        }

        public override ParseState Parse(DslToken token)
        {
            switch (token.TokenType)
            {
                case TokenType.TypeDefSeparator:
                    return ParamCreateMethodTypeDefFound();
                default:
                    throw new NoTransitionException(token);
            }
        }

        private ParseState ParamCreateMethodTypeDefFound()
        {
            return new ParamCreateMethodTypeDefFoundState(MicrowaveLanguageParser);
        }
    }
}