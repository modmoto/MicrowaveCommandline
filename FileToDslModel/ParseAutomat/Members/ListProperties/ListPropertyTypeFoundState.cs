using FileToDslModel.Lexer;
using FileToDslModel.ParseAutomat.DomainClasses;

namespace FileToDslModel.ParseAutomat.Members.ListProperties
{
    internal class ListPropertyTypeFoundState : ParseState
    {
        public ListPropertyTypeFoundState(Parser parser) : base(parser)
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
            Parser.CurrentClass.ListProperties.Add(Parser.CurrentListProperty);
            return new DomainClassOpenedState(Parser);
        }
    }
}