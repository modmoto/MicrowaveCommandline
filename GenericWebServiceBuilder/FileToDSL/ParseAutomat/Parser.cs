using System;
using System.Collections.Generic;
using System.Linq;
using GenericWebServiceBuilder.DomainSpecificGrammar;
using GenericWebServiceBuilder.FileToDSL.Lexer;

namespace GenericWebServiceBuilder.FileToDSL.ParseAutomat
{
    public class Parser
    {
        private ParseState _currentState = new StartState(null);

        private readonly IEnumerable<DslToken> _tokens;

        public Parser(IEnumerable<DslToken> tokens)
        {
            var dslTokens = tokens.ToList();
            _tokens = dslTokens;
        }

        public DomainTree Parse()
        {
            foreach (var token in _tokens)
                _currentState = _currentState.Parse(token);

            return new DomainTree(_currentState.Classes, _currentState.Events);
        }
    }

    public class StartState : ParseState
    {
        public StartState(ParseState state) : base(state, new Dictionary<TokenType, Func<StateAndTokenTuple, ParseState>>
        {
            {TokenType.DomainClass, DomainClassIdentifierFound}
        })
        {
        }

        private static ParseState DomainClassIdentifierFound(StateAndTokenTuple arg)
        {
            return new DomainClassIdentifierFoundState(arg.State);
        }
    }

    public class DomainClassIdentifierFoundState : ParseState
    {
        public DomainClassIdentifierFoundState(ParseState state) : base(state, new Dictionary<TokenType, Func<StateAndTokenTuple, ParseState>>
        {
            {TokenType.Value, DomainClassNameFound}
        })
        {
        }

        private static ParseState DomainClassNameFound(StateAndTokenTuple arg)
        {
            var domainClass = new DomainClass
            {
                Name = arg.Token.Value
            };
            arg.State.CurrentClass = domainClass;
            var domainClassNameFoundState = new DomainClassNameFoundState(arg.State);
            return domainClassNameFoundState;
        }
    }

    public class DomainClassNameFoundState : ParseState
    {
        public DomainClassNameFoundState(ParseState state) : base(state, new Dictionary<TokenType, Func<StateAndTokenTuple, ParseState>>
        {
            {TokenType.ObjectBracketOpen, BracketOpeneFound}
        })
        {
        }

        private static ParseState BracketOpeneFound(StateAndTokenTuple arg)
        {
            return new DomainClassOpenedState(arg.State);
        }
    }

    public class DomainClassOpenedState : ParseState
    {
        public DomainClassOpenedState(ParseState state) : base(state, new Dictionary<TokenType, Func<StateAndTokenTuple, ParseState>>
        {
            {TokenType.ObjectBracketClose, DomainClassClosed}
        })
        {
        }

        private static ParseState DomainClassClosed(StateAndTokenTuple arg)
        {
            arg.State.Classes.Add(arg.State.CurrentClass);
            return new StartState(arg.State);
        }
    }

    public abstract class ParseState
    {
        protected readonly Dictionary<TokenType, Func<StateAndTokenTuple, ParseState>> Transitions;

        public IList<DomainClass> Classes;
        public IList<DomainEvent> Events;
        public DomainClass CurrentClass;
        public DomainEvent CurrentEvent;
        public DomainMethod CurrentMethod;
        public Property CurrentProperty;
        public DomainTree DomainTree;

        protected ParseState(ParseState state, Dictionary<TokenType, Func<StateAndTokenTuple, ParseState>> transitions)
        {
            Transitions = transitions;

            Classes = state?.Classes ?? new List<DomainClass>();
            Events = state?.Events ?? new List<DomainEvent>();

            CurrentClass = state?.CurrentClass;
            CurrentEvent = state?.CurrentEvent;
            CurrentMethod = state?.CurrentMethod;
            CurrentProperty = state?.CurrentProperty;
        }

        public ParseState Parse(DslToken token)
        {
            if (Transitions.ContainsKey(token.TokenType))
                return Transitions[token.TokenType](new StateAndTokenTuple {State = this, Token = token});

            throw new Exception($"Transition not possible: {GetType()} to {token.TokenType}");
        }
    }

    public class StateAndTokenTuple
    {
        public ParseState State { get; set; }
        public DslToken Token { get; set; }
    }
}