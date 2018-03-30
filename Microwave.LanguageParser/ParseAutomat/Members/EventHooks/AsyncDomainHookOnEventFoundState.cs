using Microwave.LanguageParser.Lexer;

namespace Microwave.LanguageParser.ParseAutomat.Members.EventHooks
{
    internal class AsyncDomainHookOnEventFoundState : ParseState
    {
        public AsyncDomainHookOnEventFoundState(MicrowaveLanguageParser microwaveLanguageParser) : base(microwaveLanguageParser)
        {
        }

        public override ParseState Parse(DslToken token)
        {
            switch (token.TokenType)
            {
                case TokenType.DomainHookEventDefinition:
                    return AsyncDomainHookEventFound(token);
                default:
                    throw new NoTransitionException(token);
            }
        }

        private ParseState AsyncDomainHookEventFound(DslToken token)
        {
            var strings = token.Value.Split(".");
            MicrowaveLanguageParser.CurrentAsyncDomainHook.ClassType = strings[0];
            MicrowaveLanguageParser.CurrentAsyncDomainHook.MethodName = strings[1];

            if (strings[1] == "Create") MicrowaveLanguageParser.CurrentAsyncDomainHook.IsCreateHook = true;
           
            MicrowaveLanguageParser.AsyncDomainHooks.Add(MicrowaveLanguageParser.CurrentAsyncDomainHook);

            return new StartState(MicrowaveLanguageParser);
        }
    }
}