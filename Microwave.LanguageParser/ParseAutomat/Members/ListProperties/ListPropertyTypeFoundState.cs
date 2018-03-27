using Microwave.LanguageParser.Lexer;
using Microwave.LanguageParser.ParseAutomat.DomainClasses;

namespace Microwave.LanguageParser.ParseAutomat.Members.ListProperties
{
    internal class ListPropertyTypeFoundState : ParseState
    {
        public ListPropertyTypeFoundState(MicrowaveLanguageParser microwaveLanguageParser) : base(microwaveLanguageParser)
        {
        }

        public override ParseState Parse(DslToken token)
        {
            switch (token.TokenType)
            {
                case TokenType.ListBracketClose:
                    return ListPropertyClosedFound();
                default:
                    throw new NoTransitionException(token);
            }
        }

        private ParseState ListPropertyClosedFound()
        {
            MicrowaveLanguageParser.CurrentClass.ListProperties.Add(MicrowaveLanguageParser.CurrentListProperty);
            return new DomainClassOpenedState(MicrowaveLanguageParser);
        }
    }
}