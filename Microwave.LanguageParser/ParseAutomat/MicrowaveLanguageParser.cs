using System.Collections.Generic;
using Microwave.LanguageModel;
using Microwave.LanguageParser.Lexer;

namespace Microwave.LanguageParser.ParseAutomat
{
    public class MicrowaveLanguageParser : IParser
    {
        private ParseState _currentState;

        public MicrowaveLanguageParser()
        {
            _currentState = new StartState(this);
        }

        public List<DomainClass> Classes { get; } = new List<DomainClass>();
        public DomainClass CurrentClass { get; set; }
        public DomainEvent CurrentEvent { get; set; }
        public DomainMethod CurrentMethod { get; set; }
        public Property CurrentProperty { get; set; }
        public Parameter CurrentParam { get; set; }
        public string CurrentMemberName { get; set; }
        public CreateMethod CurrentCreateMethod { get; set; }
        public ListProperty CurrentListProperty { get; set; }
        public SynchronousDomainHook CurrentSynchronousDomainHook { get; set; }
        public List<SynchronousDomainHook> SynchronousDomainHooks { get; } = new List<SynchronousDomainHook>();
        public AsyncDomainHook CurrentAsyncDomainHook { get; set; }
        public List<AsyncDomainHook> AsyncDomainHooks { get; set; } = new List<AsyncDomainHook>();
        public string CurrentFoundValue { get; set; }

        public DomainTree Parse(IEnumerable<DslToken> tokens)
        {
            foreach (var token in tokens)
                _currentState = _currentState.Parse(token);

            return new DomainTree(Classes, SynchronousDomainHooks, AsyncDomainHooks);
        }
    }
}