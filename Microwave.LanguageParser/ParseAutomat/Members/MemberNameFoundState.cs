using Microwave.LanguageModel;
using Microwave.LanguageParser.Lexer;
using Microwave.LanguageParser.ParseAutomat.Members.Methods;
using Microwave.LanguageParser.ParseAutomat.Members.Properties;
using Microwave.WebServiceModel.Domain;

namespace Microwave.LanguageParser.ParseAutomat.Members
{
    public class MemberNameFoundState : ParseState
    {
        public MemberNameFoundState(MicrowaveLanguageParser microwaveLanguageParser) : base(microwaveLanguageParser)
        {
        }

        public override ParseState Parse(DslToken token)
        {
            switch (token.TokenType)
            {
                case TokenType.TypeDefSeparator:
                    return PropertySeparatorFound();
                case TokenType.ParameterBracketOpen:
                    return MethodParamOpenFound();
                default:
                    throw new NoTransitionException(token);
            }
        }

        private ParseState MethodParamOpenFound()
        {
            MicrowaveLanguageParser.CurrentMethod = new DomainMethod
            {
                Name = MicrowaveLanguageParser.CurrentMemberName,
                ReturnType = new ValidationResultBaseClass().Name
            };
            return new MethodParamOpenState(MicrowaveLanguageParser);
        }

        private ParseState PropertySeparatorFound()
        {
            MicrowaveLanguageParser.CurrentProperty = new Property {Name = MicrowaveLanguageParser.CurrentMemberName};
            return new PropertySeparatorFoundState(MicrowaveLanguageParser);
        }
    }
}