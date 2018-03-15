using FileToDslModel.Lexer;

namespace FileToDslModel.ParseAutomat.Members.EventHooks
{
    internal class SynchronousDomainHookOnEventFoundState : ParseState
    {
        public SynchronousDomainHookOnEventFoundState(Parser parser) : base(parser)
        {
        }

        public override ParseState Parse(DslToken token)
        {
            switch (token.TokenType)
            {
                case TokenType.DomainHookEventDefinition:
                    return SynchronousDomainHookEventFound(token);
                default:
                    throw new NoTransitionException(token);
            }
        }


        private ParseState SynchronousDomainHookEventFound(DslToken token)
        {
            var strings = token.Value.Split(".");
            Parser.CurrentSynchronousDomainHook.ClassType = strings[0];
            Parser.CurrentSynchronousDomainHook.MethodName = strings[1];
           
            Parser.SynchronousDomainHooks.Add(Parser.CurrentSynchronousDomainHook);

            return new StartState(Parser);
        }
    }
}