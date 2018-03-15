using FileToDslModel.Lexer;
using FileToDslModel.ParseAutomat.DomainClasses;
using FileToDslModel.ParseAutomat.Members;
using FileToDslModel.ParseAutomat.Members.EventHooks;

namespace FileToDslModel.ParseAutomat
{
    public class StartState : ParseState
    {
        public StartState(Parser parser) : base(parser)
        {
        }

        public override ParseState Parse(DslToken token)
        {
            switch (token.TokenType)
            {
                case TokenType.DomainClass:
                    return DomainClassIdentifierFound();
                case TokenType.SynchronousDomainHook:
                    return SynchronousDomainHookFound();
                default:
                    throw new NoTransitionException(token);
            }
        }

        private ParseState SynchronousDomainHookFound()
        {
            return new SynchronousDomainHookFoundState(Parser);
        }
        private ParseState DomainClassIdentifierFound()
        {
            return new DomainClassIdentifierFoundState(Parser);
        }
    }
}