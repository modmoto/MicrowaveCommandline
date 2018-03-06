using DslModel;
using FileToDslModel.Lexer;
using FileToDslModel.ParseAutomat.DomainClasses;

namespace FileToDslModel.ParseAutomat.Members.Methods.Events
{
    public class EventDefinitionFoundState : ParseState
    {
        public EventDefinitionFoundState(Parser parser) : base(parser)
        {
        }

        public override ParseState Parse(DslToken token)
        {
            switch (token.TokenType)
            {
                case TokenType.ObjectBracketClose:
                    return EventDefinitionEndFound();
                case TokenType.Value:
                    return PropertyNameFound(token);
                default:
                    throw new NoTransitionException(token);
            }
        }

        private ParseState PropertyNameFound(DslToken token)
        {
            Parser.CurrentProperty = new Property {Name = token.Value};
            return new EventPropertyNameFoundState(Parser);
        }

        private ParseState EventDefinitionEndFound()
        {
            Parser.CurrentClass.Methods.Add(Parser.CurrentMethod);
            Parser.CurrentClass.Events.Add(Parser.CurrentEvent);
            return new DomainClassOpenedState(Parser);
        }
    }
}