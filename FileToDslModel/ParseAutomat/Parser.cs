using System.Collections.Generic;
using DslModel.Domain;
using FileToDslModel.Lexer;

namespace FileToDslModel.ParseAutomat
{
    public class Parser : IParser
    {
        private ParseState _currentState;

        public Parser()
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

        public DomainTree Parse(IEnumerable<DslToken> tokens)
        {
            foreach (var token in tokens)
                _currentState = _currentState.Parse(token);

            return new DomainTree(Classes, SynchronousDomainHooks);
        }
    }
}