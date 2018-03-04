using FileToDslModel.Lexer;

namespace FileToDslModel.ParseAutomat.Members.Methods.Events
{
    public class EventPropertyNameFoundState : ParseState
    {
        public EventPropertyNameFoundState(Parser parser) : base(parser)
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
            return new EventPropertyTypeDefSeparatorFoundState(Parser);
        }
    }
}