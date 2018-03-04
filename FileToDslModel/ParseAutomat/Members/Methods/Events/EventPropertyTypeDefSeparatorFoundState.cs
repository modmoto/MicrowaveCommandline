using FileToDslModel.Lexer;

namespace FileToDslModel.ParseAutomat.Members.Methods.Events
{
    public class EventPropertyTypeDefSeparatorFoundState : ParseState
    {
        public EventPropertyTypeDefSeparatorFoundState(Parser parser) : base(parser)
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
            Parser.CurrentProperty.Type = token.Value;
            Parser.CurrentEvent.Properties.Add(Parser.CurrentProperty);
            return new EventDefinitionFoundState(Parser);
        }
    }
}