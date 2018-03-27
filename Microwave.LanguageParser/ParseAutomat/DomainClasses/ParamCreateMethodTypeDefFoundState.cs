using Microwave.LanguageParser.Lexer;

namespace Microwave.LanguageParser.ParseAutomat.DomainClasses
{
    internal class ParamCreateMethodTypeDefFoundState : ParseState
    {
        public ParamCreateMethodTypeDefFoundState(MicrowaveLanguageParser microwaveLanguageParser) : base(microwaveLanguageParser)
        {
        }

        public override ParseState Parse(DslToken token)
        {
            switch (token.TokenType)
            {
                case TokenType.Value:
                    return ParamCreateMethodTypeFound(token);
                default:
                    throw new NoTransitionException(token);
            }
        }

        private ParseState ParamCreateMethodTypeFound(DslToken token)
        {
            MicrowaveLanguageParser.CurrentParam.Type = token.Value;
            MicrowaveLanguageParser.CurrentCreateMethod.Parameters.Add(MicrowaveLanguageParser.CurrentParam);
            return new PropertyCreateMethodTypeFoundState(MicrowaveLanguageParser);
        }
    }
}