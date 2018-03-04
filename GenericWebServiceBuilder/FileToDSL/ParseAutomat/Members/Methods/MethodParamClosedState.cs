using GenericWebServiceBuilder.FileToDSL.Lexer;
using GenericWebServiceBuilder.FileToDSL.ParseAutomat.Members.Methods.Events;

namespace GenericWebServiceBuilder.FileToDSL.ParseAutomat.Members.Methods
{
    internal class MethodParamClosedState : ParseState
    {
        public MethodParamClosedState(Parser parser) : base(parser)
        {
        }

        public override ParseState Parse(DslToken token)
        {
            switch (token.TokenType)
            {
                case TokenType.TypeDefSeparator:
                    return EventDefinitionFound();
                default:
                    throw new NoTransitionException(token);
            }
        }

        private ParseState EventDefinitionFound()
        {
            return new EventDefinitionTypeDefSeparatorFoundState(Parser);
        }
    }

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
            return new EventDefinitionFoundState(Parser);
        }
    }
}