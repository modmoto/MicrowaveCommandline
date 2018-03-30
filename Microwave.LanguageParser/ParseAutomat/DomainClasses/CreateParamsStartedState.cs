using Microwave.LanguageModel;
using Microwave.LanguageParser.Lexer;

namespace Microwave.LanguageParser.ParseAutomat.DomainClasses
{
    internal class CreateParamsStartedState : ParseState
    {
        public CreateParamsStartedState(MicrowaveLanguageParser microwaveLanguageParser) : base(microwaveLanguageParser)
        {
        }

        public override ParseState Parse(DslToken token)
        {
            switch (token.TokenType)
            {
                case TokenType.ParameterBracketClose:
                    return CreateMethodParamsFinished();
                case TokenType.Value:
                    return ParamCreateMethodStarted(token);
                default:
                    throw new NoTransitionException(token);
            }
        }
        private ParseState CreateMethodParamsFinished()
        {
            MicrowaveLanguageParser.CurrentClass.CreateMethods.Add(MicrowaveLanguageParser.CurrentCreateMethod);
            MicrowaveLanguageParser.CurrentClass.Events.Add(MicrowaveLanguageParser.CurrentEvent);
            return new DomainClassOpenedState(MicrowaveLanguageParser);
        }

        private ParseState ParamCreateMethodStarted(DslToken token)
        {
            MicrowaveLanguageParser.CurrentParam = new Parameter { Name = token.Value };
            return new ParamCreateMethodStartedState(MicrowaveLanguageParser);
        }
    }
}