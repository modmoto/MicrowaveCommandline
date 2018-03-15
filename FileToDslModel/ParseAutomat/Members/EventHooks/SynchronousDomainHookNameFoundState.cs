using FileToDslModel.Lexer;

namespace FileToDslModel.ParseAutomat.Members.EventHooks
{
    internal class SynchronousDomainHookNameFoundState : ParseState
    {
        public SynchronousDomainHookNameFoundState(Parser parser) : base(parser)
        {
        }

        public override ParseState Parse(DslToken token)
        {
            switch (token.TokenType)
            {
                case TokenType.DomainHookOn:
                    return SynchronousDomainHookOnEventFound();
                default:
                    throw new NoTransitionException(token);
            }
        }

        private ParseState SynchronousDomainHookOnEventFound()
        {
            return new SynchronousDomainHookOnEventFoundState(Parser);
        }
    }
}