using Microwave.LanguageParser.Lexer;

namespace Microwave.LanguageParser.ParseAutomat.DomainClasses
{
    internal class PropertyCreateMethodTypeFoundState : ParseState
    {
        public PropertyCreateMethodTypeFoundState(MicrowaveLanguageParser microwaveLanguageParser) : base(microwaveLanguageParser)
        {
        }

        public override ParseState Parse(DslToken token)
        {
            switch (token.TokenType)
            {
                case TokenType.ParameterBracketClose:
                    return CreateMethodParamsFinished();
                case TokenType.ParamSeparator:
                    return AdditionalCreateMethodParamFound();
                default:
                    throw new NoTransitionException(token);
            }
        }

        private ParseState AdditionalCreateMethodParamFound()
        {
            return new CreateParamsStartedState(MicrowaveLanguageParser);
        }

        private ParseState CreateMethodParamsFinished()
        {
            MicrowaveLanguageParser.CurrentClass.CreateMethods.Add(MicrowaveLanguageParser.CurrentCreateMethod);
            MicrowaveLanguageParser.CurrentClass.Events.Add(MicrowaveLanguageParser.CurrentEvent);
            return new DomainClassOpenedState(MicrowaveLanguageParser);
        }
        
    }
}