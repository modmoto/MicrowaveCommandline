using System;
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

    public class StartState : ParseState
    {
        public StartState(Parser parser) : base(parser)
        {
        }

        private ParseState DomainClassIdentifierFound()
        {
            return new DomainClassIdentifierFoundState(Parser);
        }

        public override ParseState Parse(DslToken token)
        {
            switch (token.TokenType)
            {
                case TokenType.DomainClass:
                    return DomainClassIdentifierFound();
                default:
                    throw new NoTransitionException(this);
            }
        }
    }

    public class DomainClassIdentifierFoundState : ParseState
    {
        public DomainClassIdentifierFoundState(Parser parser) : base(parser)
        {
        }

        private ParseState DomainClassNameFound(DslToken token)
        {
            var domainClass = new DomainClass
            {
                Name = token.Value
            };
            Parser.CurrentClass = domainClass;
            var domainClassNameFoundState = new DomainClassNameFoundState(Parser);
            return domainClassNameFoundState;
        }

        public override ParseState Parse(DslToken token)
        {
            switch (token.TokenType)
            {
                case TokenType.Value:
                    return DomainClassNameFound(token);
                default:
                    throw new NoTransitionException(this);
            }
        }
    }

    public class DomainClassNameFoundState : ParseState
    {
        public DomainClassNameFoundState(Parser parser) : base(parser)
        {
        }

        private ParseState BracketOpeneFound()
        {
            return new DomainClassOpenedState(Parser);
        }

        public override ParseState Parse(DslToken token)
        {
            switch (token.TokenType)
            {
                case TokenType.ObjectBracketOpen:
                    return BracketOpeneFound();
                default:
                    throw new NoTransitionException(this);
            }
        }
    }

    public class DomainClassOpenedState : ParseState
    {
        public DomainClassOpenedState(Parser parser) : base(parser)
        {
        }

        private ParseState DomainClassClosed()
        {
            Parser.Classes.Add(Parser.CurrentClass);
            return new StartState(Parser);
        }

        public override ParseState Parse(DslToken token)
        {
            switch (token.TokenType)
            {
                case TokenType.ObjectBracketClose:
                    return DomainClassClosed();
                default:
                    throw new NoTransitionException(this);
            }
        }
    }

    public class NoTransitionException : Exception
    {
        public NoTransitionException(ParseState state) : base($"There is no Transition defined for {state.GetType()}")
        {
        }
    }

    public abstract class ParseState
    {
        protected ParseState(Parser parser)
        {
            Parser = parser;
        }

        public Parser Parser { get; }
        public abstract ParseState Parse(DslToken token);
    }
}