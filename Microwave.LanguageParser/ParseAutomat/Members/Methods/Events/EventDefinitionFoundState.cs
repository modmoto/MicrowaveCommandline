using Microwave.LanguageModel.Domain;
using Microwave.LanguageParser.Lexer;
using Microwave.LanguageParser.ParseAutomat.DomainClasses;

namespace Microwave.LanguageParser.ParseAutomat.Members.Methods.Events
{
    public class EventDefinitionFoundState : ParseState
    {
        public EventDefinitionFoundState(MicrowaveLanguageParser microwaveLanguageParser) : base(microwaveLanguageParser)
        {
        }

        public override ParseState Parse(DslToken token)
        {
            switch (token.TokenType)
            {
                case TokenType.ObjectBracketClose:
                    return EventDefinitionEndFound();
                case TokenType.Value:
                    return PropertyNameFound(token);
                default:
                    throw new NoTransitionException(token);
            }
        }

        private ParseState PropertyNameFound(DslToken token)
        {
            MicrowaveLanguageParser.CurrentProperty = new Property {Name = token.Value};
            return new EventPropertyNameFoundState(MicrowaveLanguageParser);
        }

        private ParseState EventDefinitionEndFound()
        {
            MicrowaveLanguageParser.CurrentClass.Methods.Add(MicrowaveLanguageParser.CurrentMethod);
            MicrowaveLanguageParser.CurrentClass.Events.Add(MicrowaveLanguageParser.CurrentEvent);
            return new DomainClassOpenedState(MicrowaveLanguageParser);
        }
    }
}