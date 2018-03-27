using Microwave.LanguageParser.Lexer;

namespace Microwave.LanguageParser.ParseAutomat.Members.ListProperties
{
    internal class ListPropertyStartedFoundState : ParseState
    {
        public ListPropertyStartedFoundState(MicrowaveLanguageParser microwaveLanguageParser) : base(microwaveLanguageParser)
        {
        }

        public override ParseState Parse(DslToken token)
        {
            switch (token.TokenType)
            {
                case TokenType.Value:
                    return ListPropertyTypeFound(token);
                default:
                    throw new NoTransitionException(token);
            }
        }

        private ParseState ListPropertyTypeFound(DslToken token)
        {
            MicrowaveLanguageParser.CurrentListProperty.Type = token.Value;
            return new ListPropertyTypeFoundState(MicrowaveLanguageParser);
        }
    }
}