using Microwave.LanguageModel;
using Microwave.LanguageParser.Lexer;
using Microwave.LanguageParser.ParseAutomat.Members;
using Microwave.WebServiceModel.Domain;

namespace Microwave.LanguageParser.ParseAutomat.DomainClasses
{
    public class DomainClassOpenedState : ParseState
    {
        public DomainClassOpenedState(MicrowaveLanguageParser microwaveLanguageParser) : base(microwaveLanguageParser)
        {
        }

        private ParseState DomainClassClosed()
        {
            MicrowaveLanguageParser.Classes.Add(MicrowaveLanguageParser.CurrentClass);
            return new StartState(MicrowaveLanguageParser);
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
            MicrowaveLanguageParser.CurrentEvent = new DomainEvent {Name = $"{MicrowaveLanguageParser.CurrentClass.Name}CreateEvent" };
            MicrowaveLanguageParser.CurrentEvent.Properties.Add(new Property {Name = MicrowaveLanguageParser.CurrentClass.Name, Type = MicrowaveLanguageParser.CurrentClass.Name });
            MicrowaveLanguageParser.CurrentCreateMethod = new CreateMethod {ReturnType = new ValidationResultBaseClass().Name };
            return new CreateMethodFoundState(MicrowaveLanguageParser);
        }

        private ParseState PropertyStarted(DslToken token)
        {
            MicrowaveLanguageParser.CurrentMemberName = token.Value;
            MicrowaveLanguageParser.CurrentEvent = new DomainEvent { Name = $"{MicrowaveLanguageParser.CurrentClass.Name}{token.Value}Event" };
            return new MemberNameFoundState(MicrowaveLanguageParser);
        }
    }
}