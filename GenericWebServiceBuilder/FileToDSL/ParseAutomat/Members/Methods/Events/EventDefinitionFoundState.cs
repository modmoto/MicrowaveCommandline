using GenericWebServiceBuilder.DomainSpecificGrammar;
using GenericWebServiceBuilder.FileToDSL.Lexer;
using GenericWebServiceBuilder.FileToDSL.ParseAutomat.DomainClasses;

namespace GenericWebServiceBuilder.FileToDSL.ParseAutomat.Members.Methods.Events
{
    internal class EventDefinitionFoundState : ParseState
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
                default:
                    throw new NoTransitionException(token);
            }
        }

        private ParseState EventDefinitionEndFound()
        {
            Parser.CurrentClass.Methods.Add(Parser.CurrentMethod);
            var emptyEvent = new DomainEvent {Name = Parser.CurrentMethod.ReturnType};
            Parser.Events.Add(emptyEvent);
            return new DomainClassOpenedState(Parser);
        }
    }
}