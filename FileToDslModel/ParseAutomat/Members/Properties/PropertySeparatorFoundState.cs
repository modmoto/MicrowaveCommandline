using DslModel.Domain;
using FileToDslModel.Lexer;
using FileToDslModel.ParseAutomat.DomainClasses;
using FileToDslModel.ParseAutomat.Members.ListProperties;

namespace FileToDslModel.ParseAutomat.Members.Properties
{
    public class PropertySeparatorFoundState : ParseState
    {
        public PropertySeparatorFoundState(Parser parser) : base(parser)
        {
        }

        public override ParseState Parse(DslToken token)
        {
            switch (token.TokenType)
            {
                case TokenType.Value:
                    return PropertyTypeDefFound(token);
                case TokenType.ListBracketOpen:
                    return ListPropertyFound();
                default:
                    throw new NoTransitionException(token);
            }
        }

        private ParseState ListPropertyFound()
        {
            Parser.CurrentListProperty = new ListProperty
            {
                Name = Parser.CurrentMemberName
            };
            return new ListPropertyStartedFoundState(Parser);
        }

        private ParseState PropertyTypeDefFound(DslToken token)
        {
            Parser.CurrentProperty.Type = token.Value;
            Parser.CurrentClass.Properties.Add(Parser.CurrentProperty);
            return new DomainClassOpenedState(Parser);
        }
    }
}