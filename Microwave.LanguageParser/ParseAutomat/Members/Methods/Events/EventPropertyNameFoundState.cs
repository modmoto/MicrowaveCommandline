using Microwave.LanguageParser.Lexer;

namespace Microwave.LanguageParser.ParseAutomat.Members.Methods.Events
{
    public class EventPropertyNameFoundState : ParseState
    {
        public EventPropertyNameFoundState(MicrowaveLanguageParser microwaveLanguageParser) : base(microwaveLanguageParser)
        {
        }

        public override ParseState Parse(DslToken token)
        {
            switch (token.TokenType)
            {
                case TokenType.TypeDefSeparator:
                    return EventPropertyTypeDefSeparatorFound();
                default:
                    throw new NoTransitionException(token);
            }
        }

        private ParseState EventPropertyTypeDefSeparatorFound()
        {
            return new EventPropertyTypeDefSeparatorFoundState(MicrowaveLanguageParser);
        }
    }
}