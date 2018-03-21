using DslModel.Domain;
using FileToDslModel.Lexer;
using FileToDslModel.ParseAutomat.Members;

namespace FileToDslModel.ParseAutomat.DomainClasses
{
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
                case TokenType.CreateMethod:
                    return CreateMethodFound();
                case TokenType.ObjectBracketClose:
                    return DomainClassClosed();
                case TokenType.Value:
                    return PropertyStarted(token);
                default:
                    throw new NoTransitionException(token);
            }
        }

        private ParseState CreateMethodFound()
        {
            Parser.CurrentEvent = new DomainEvent {Name = $"{Parser.CurrentClass.Name}CreateEvent" };
            Parser.CurrentEvent.Properties.Add(new Property {Name = Parser.CurrentClass.Name, Type = Parser.CurrentClass.Name });
            Parser.CurrentCreateMethod = new CreateMethod {ReturnType = new ValidationResultBaseClass().Name };
            return new CreateMethodFoundState(Parser);
        }

        private ParseState PropertyStarted(DslToken token)
        {
            Parser.CurrentMemberName = token.Value;
            Parser.CurrentEvent = new DomainEvent { Name = $"{Parser.CurrentClass.Name}{token.Value}Event" };
            return new MemberNameFoundState(Parser);
        }
    }
}