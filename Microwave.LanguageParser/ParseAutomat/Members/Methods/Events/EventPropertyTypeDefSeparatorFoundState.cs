using Microwave.LanguageParser.Lexer;

namespace Microwave.LanguageParser.ParseAutomat.Members.Methods.Events
{
    public class EventPropertyTypeDefSeparatorFoundState : ParseState
    {
        public EventPropertyTypeDefSeparatorFoundState(MicrowaveLanguageParser microwaveLanguageParser) : base(microwaveLanguageParser)
        {
        }

        public override ParseState Parse(DslToken token)
        {
            switch (token.TokenType)
            {
                case TokenType.Value:
                    return EventPropertyTypeFound(token);
                default:
                    throw new NoTransitionException(token);
            }
        }

        private ParseState EventPropertyTypeFound(DslToken token)
        {
            MicrowaveLanguageParser.CurrentProperty.Type = token.Value;
            MicrowaveLanguageParser.CurrentEvent.Properties.Add(MicrowaveLanguageParser.CurrentProperty);
            return new EventDefinitionFoundState(MicrowaveLanguageParser);
        }
    }
}