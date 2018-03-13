using FileToDslModel.Lexer;

namespace FileToDslModel.ParseAutomat.Members.ListProperties
{
    internal class ListPropertyStartedFoundState : ParseState
    {
        public ListPropertyStartedFoundState(Parser parser) : base(parser)
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
            Parser.CurrentListProperty.Type = token.Value;
            return new ListPropertyTypeFoundState(Parser);
        }
    }
}