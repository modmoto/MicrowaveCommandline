using Microwave.LanguageModel;
using Microwave.LanguageParser.Lexer;
using Microwave.LanguageParser.ParseAutomat.DomainClasses;
using Microwave.LanguageParser.ParseAutomat.Members.ListProperties;

namespace Microwave.LanguageParser.ParseAutomat.Members.Properties
{
    public class PropertySeparatorFoundState : ParseState
    {
        public PropertySeparatorFoundState(MicrowaveLanguageParser microwaveLanguageParser) : base(microwaveLanguageParser)
        {
        }

        public override ParseState Parse(DslToken token)
        {
            switch (token.TokenType)
            {
                case TokenType.Value:
                    return PropertyTypeDefFound(token);
                case TokenType.ListBracketOpen:
                    return ListPropertyFound();
                default:
                    throw new NoTransitionException(token);
            }
        }

        private ParseState ListPropertyFound()
        {
            MicrowaveLanguageParser.CurrentListProperty = new ListProperty
            {
                Name = MicrowaveLanguageParser.CurrentMemberName
            };
            return new ListPropertyStartedFoundState(MicrowaveLanguageParser);
        }

        private ParseState PropertyTypeDefFound(DslToken token)
        {
            MicrowaveLanguageParser.CurrentProperty.Type = token.Value;
            MicrowaveLanguageParser.CurrentClass.Properties.Add(MicrowaveLanguageParser.CurrentProperty);
            return new DomainClassOpenedState(MicrowaveLanguageParser);
        }
    }
}