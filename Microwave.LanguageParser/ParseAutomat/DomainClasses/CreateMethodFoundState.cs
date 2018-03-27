using Microwave.LanguageParser.Lexer;

namespace Microwave.LanguageParser.ParseAutomat.DomainClasses
{
    internal class CreateMethodFoundState : ParseState
    {
        public CreateMethodFoundState(MicrowaveLanguageParser microwaveLanguageParser) : base(microwaveLanguageParser)
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
            return new CreateParamsStartedState(MicrowaveLanguageParser);
        }
    }
}