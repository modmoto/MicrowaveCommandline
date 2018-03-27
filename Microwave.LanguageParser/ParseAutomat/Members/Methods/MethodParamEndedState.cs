using Microwave.LanguageParser.Lexer;
using Microwave.LanguageParser.ParseAutomat.Members.Methods.Events;

namespace Microwave.LanguageParser.ParseAutomat.Members.Methods
{
    internal class MethodParamEndedState : ParseState
    {
        public MethodParamEndedState(MicrowaveLanguageParser microwaveLanguageParser) : base(microwaveLanguageParser)
        {
        }

        public override ParseState Parse(DslToken token)
        {
            switch (token.TokenType)
            {
                case TokenType.TypeDefSeparator:
                    return EventDefinitionFound();
                default:
                    throw new NoTransitionException(token);
            }
        }

        private ParseState EventDefinitionFound()
        {
            return new EventDefinitionTypeDefSeparatorFoundState(MicrowaveLanguageParser);
        }
    }
}