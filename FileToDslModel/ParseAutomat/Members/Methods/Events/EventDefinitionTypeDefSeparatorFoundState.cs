using FileToDslModel.Lexer;

namespace FileToDslModel.ParseAutomat.Members.Methods.Events
{
    internal class EventDefinitionTypeDefSeparatorFoundState : ParseState
    {
        public EventDefinitionTypeDefSeparatorFoundState(Parser parser) : base(parser)
        {
        }

        public override ParseState Parse(DslToken token)
        {
            switch (token.TokenType)
            {
                case TokenType.ObjectBracketOpen:
                    return EventDefinitionFound();
                default:
                    throw new NoTransitionException(token);
            }
        }

        private ParseState EventDefinitionFound()
        {
            return new EventDefinitionFoundState(Parser);
        }
    }
}