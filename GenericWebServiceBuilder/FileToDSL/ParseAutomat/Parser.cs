using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using GenericWebServiceBuilder.DomainSpecificGrammar;
using GenericWebServiceBuilder.FileToDSL.Lexer;

namespace GenericWebServiceBuilder.FileToDSL.ParseAutomat
{
    public class Parser
    {
        private readonly Dictionary<DslState, Type> _finishStates = new Dictionary<DslState, Type>
        {
            {DslState.DomainClassClosed, typeof(DomainClass)},
        };

        private readonly ICollection<DslToken> _stackOfSavedElements;

        private readonly Dictionary<Tuple<DslState, TokenType>, DslState> _stateMachine =
            new Dictionary<Tuple<DslState, TokenType>, DslState>
            {
                {Tuple.Create(DslState.Start, TokenType.DomainClass), DslState.DomainClassIdentifierFound},
                {Tuple.Create(DslState.DomainClassIdentifierFound, TokenType.Value), DslState.DomainClassNameFound},
                {
                    Tuple.Create(DslState.DomainClassNameFound, TokenType.ObjectBracketOpen),
                    DslState.DomainClassOpened
                },
                {
                    Tuple.Create(DslState.DomainClassOpened, TokenType.ObjectBracketClose),
                    DslState.DomainClassClosed
                }
            };

        private IList<DomainClass> _classes;
        private IList<DomainEvent> _events;
        private DslState _state = DslState.Start;

        public Parser()
        {
            _stackOfSavedElements = new Collection<DslToken>();
        }

        public DomainTree Parse(IEnumerable<DslToken> tokens)
        {
            _events = new List<DomainEvent>();
            _classes = new List<DomainClass>();

            foreach (var token in tokens)
                Parse(token);

            return new DomainTree(_classes, _events);
        }

        private void Parse(DslToken token)
        {
            var transition = Tuple.Create(_state, token.TokenType);
            if (_stateMachine.ContainsKey(transition))
            {
                _state = _stateMachine[transition];
                _stackOfSavedElements.Add(token);

                if (_finishStates.ContainsKey(_state))
                {
                    // do that generic
                    _classes.Add(new DomainClass
                    {
                        Name = _stackOfSavedElements.ToList()[1].Value
                    });
                    _stackOfSavedElements.Clear();
                }
            }
            else
            {
                throw new Exception();
            }
        }
    }
}