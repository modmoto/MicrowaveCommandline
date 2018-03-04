using System.Collections.Generic;
using DslModel;
using FileToDslModel.Lexer;

namespace FileToDslModel.ParseAutomat
{
    public class Parser : IParser
    {
        private ParseState _currentState;

        public Parser()
        {
            Classes = new List<DomainClass>();
            Events = new List<DomainEvent>();
            _currentState = new StartState(this);
        }

        public IList<DomainClass> Classes { get; }
        public IList<DomainEvent> Events { get; }
        public DomainClass CurrentClass { get; set; }
        public DomainEvent CurrentEvent { get; set; }
        public DomainMethod CurrentMethod { get; set; }
        public Property CurrentProperty { get; set; }
        public Parameter CurrentParam { get; set; }
        public string CurrentMemberName { get; set; }

        public DomainTree Parse(IEnumerable<DslToken> tokens)
        {
            foreach (var token in tokens)
                _currentState = _currentState.Parse(token);

            return new DomainTree(Classes, Events);
        }
    }
}