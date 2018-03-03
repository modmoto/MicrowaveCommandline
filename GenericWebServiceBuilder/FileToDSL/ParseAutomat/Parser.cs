using System.Collections.Generic;
using System.Linq;
using GenericWebServiceBuilder.DomainSpecificGrammar;
using GenericWebServiceBuilder.FileToDSL.Lexer;

namespace GenericWebServiceBuilder.FileToDSL.ParseAutomat
{
    public class Parser
    {
        private readonly IEnumerable<DslToken> _tokens;
        private ParseState _currentState;

        public Parser(IEnumerable<DslToken> tokens)
        {
            Classes = new List<DomainClass>();
            Events = new List<DomainEvent>();
            var dslTokens = tokens.ToList();
            _tokens = dslTokens;
            _currentState = new StartState(this);
        }

        public IList<DomainClass> Classes { get; }
        public IList<DomainEvent> Events { get; }
        public DomainClass CurrentClass { get; set; }
        public DomainEvent CurrentEvent { get; set; }
        public DomainMethod CurrentMethod { get; set; }
        public Property CurrentProperty { get; set; }

        public DomainTree Parse()
        {
            foreach (var token in _tokens)
                _currentState = _currentState.Parse(token);

            return new DomainTree(Classes, Events);
        }
    }
}