using System;
using System.Collections.Generic;
using System.Linq;
using GenericWebServiceBuilder.DomainSpecificGrammar;
using GenericWebServiceBuilder.FileToDSL.Lexer;

namespace GenericWebServiceBuilder.FileToDSL.ParseAutomat
{
    public class Parser
    {
        private readonly DomainTree _domainTree;

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
                },
                {
                    Tuple.Create(DslState.DomainClassOpened, TokenType.Value),
                    DslState.PropertyNameFound
                },
                {
                    Tuple.Create(DslState.PropertyNameFound, TokenType.TypeDefSeparator),
                    DslState.TypeDefSeparatorFound
                },
                {
                    Tuple.Create(DslState.TypeDefSeparatorFound, TokenType.Value),
                    DslState.PropertyTypeEnded
                },
                {
                    Tuple.Create(DslState.PropertyTypeEnded, TokenType.ObjectBracketClose),
                    DslState.DomainClassClosed
                }
            };

        private readonly IEnumerable<DslToken> _tokens;

        private DomainClass _currentClass;
        private DomainEvent _currentEvent;
        private DomainMethod _currentMethod;
        private Property _currentProperty;
        private IList<DomainEvent> _events;
        private DslState _state = DslState.Start;

        private readonly IList<DslState> stuffToHappen = new List<DslState>
        {
            DslState.DomainClassClosed,
            DslState.DomainClassNameFound,
            DslState.PropertyTypeEnded,
            DslState.PropertyNameFound
        };

        public Parser(IEnumerable<DslToken> tokens)
        {
            _domainTree = new DomainTree(new List<DomainClass>(), new List<DomainEvent>());
            var dslTokens = tokens.ToList();
            _tokens = dslTokens;
        }

        public DomainTree Parse()
        {
            foreach (var token in _tokens)
                Parse(token);

            return _domainTree;
        }

        private void Parse(DslToken token)
        {
            var transition = Tuple.Create(_state, token.TokenType);
            if (_stateMachine.ContainsKey(transition))
            {
                _state = _stateMachine[transition];

                if (stuffToHappen.Contains(_state)) AddToTree(token);
            }
            else
            {
                throw new Exception($"token not InStateMachine: {token.TokenType} {token.Value}");
            }
        }

        private void AddToTree(DslToken token)
        {
            switch (_state)
            {
                case DslState.DomainClassNameFound:
                    OpenNewClass(token);
                    break;
                case DslState.DomainClassClosed:
                    CloseCurrentClass();
                    break;
                case DslState.PropertyTypeEnded:
                    CloseCurrentProperty(token);
                    break;
                case DslState.PropertyNameFound:
                    OpenNewProperty(token);
                    break;
                default: throw new Exception($"token not in Doing Block: {token.TokenType} {token.Value}");
            }
        }

        private void OpenNewProperty(DslToken token)
        {
            var property = new Property
            {
                Name = token.Value
            };
            _currentProperty = property;
        }

        private void CloseCurrentProperty(DslToken token)
        {
            _currentProperty.Type = token.Value;
            _currentClass.Propteries.Add(_currentProperty);
        }

        private void CloseCurrentClass()
        {
            _domainTree.Classes.Add(_currentClass);
        }

        private void OpenNewClass(DslToken token)
        {
            var domainClass = new DomainClass
            {
                Name = token.Value
            };
            _currentClass = domainClass;
        }
    }
}