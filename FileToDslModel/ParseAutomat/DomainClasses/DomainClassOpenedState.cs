using FileToDslModel.Lexer;
using FileToDslModel.ParseAutomat.Members;

namespace FileToDslModel.ParseAutomat.DomainClasses
{
    public class DomainClassOpenedState : ParseState
    {
        public DomainClassOpenedState(Parser parser) : base(parser)
        {
        }

        private ParseState DomainClassClosed()
        {
            Parser.Classes.Add(Parser.CurrentClass);
            return new StartState(Parser);
        }

        public override ParseState Parse(DslToken token)
        {
            switch (token.TokenType)
            {
                case TokenType.ObjectBracketClose:
                    return DomainClassClosed();
                case TokenType.Value:
                    return PropertyStarted(token);
                default:
                    throw new NoTransitionException(token);
            }
        }

        private ParseState PropertyStarted(DslToken token)
        {
            Parser.CurrentMemberName = token.Value;
            return new MemberNameFoundState(Parser);
        }
    }
}