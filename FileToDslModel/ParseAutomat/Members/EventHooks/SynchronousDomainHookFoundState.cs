using DslModel.Domain;
using FileToDslModel.Lexer;

namespace FileToDslModel.ParseAutomat.Members.EventHooks
{
    internal class SynchronousDomainHookFoundState : ParseState
    {
        public SynchronousDomainHookFoundState(Parser parser) : base(parser)
        {
        }

        public override ParseState Parse(DslToken token)
        {
            switch (token.TokenType)
            {
                case TokenType.Value:
                    return SynchronousDomainHookNameFound(token);
                default:
                    throw new NoTransitionException(token);
            }
        }

        private ParseState SynchronousDomainHookNameFound(DslToken token)
        {
            Parser.CurrentSynchronousDomainHook = new SynchronousDomainHook
            {
                Name = token.Value
            };


            return new SynchronousDomainHookNameFoundState(Parser);
        }
    }
}