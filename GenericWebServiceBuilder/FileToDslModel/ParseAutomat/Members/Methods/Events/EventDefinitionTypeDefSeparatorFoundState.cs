using GenericWebServiceBuilder.DslModel;
using GenericWebServiceBuilder.FileToDslModel.Lexer;

namespace GenericWebServiceBuilder.FileToDslModel.ParseAutomat.Members.Methods.Events
{
    internal class EventDefinitionTypeDefSeparatorFoundState : ParseState
    {
        public EventDefinitionTypeDefSeparatorFoundState(Parser parser) : base(parser)
        {
        }

        public override ParseState Parse(DslToken token)
        {
            switch (token.TokenType)
            {
                case TokenType.ObjectBracketOpen:
                    return EventDefinitionFound();
                default:
                    throw new NoTransitionException(token);
            }
        }

        private ParseState EventDefinitionFound()
        {
            Parser.CurrentEvent = new DomainEvent { Name = Parser.CurrentMethod.ReturnType };
            return new EventDefinitionFoundState(Parser);
        }
    }
}