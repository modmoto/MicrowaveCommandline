using FileToDslModel.Lexer;

namespace FileToDslModel.ParseAutomat.DomainClasses
{
    internal class PropertyCreateMethodTypeFoundState : ParseState
    {
        public PropertyCreateMethodTypeFoundState(Parser parser) : base(parser)
        {
        }

        public override ParseState Parse(DslToken token)
        {
            switch (token.TokenType)
            {
                case TokenType.ParameterBracketClose:
                    return CreateMethodParamsFinished();
                case TokenType.ParamSeparator:
                    return AdditionalCreateMethodParamFound();
                default:
                    throw new NoTransitionException(token);
            }
        }

        private ParseState AdditionalCreateMethodParamFound()
        {
            return new CreateParamsStartedState(Parser);
        }

        private ParseState CreateMethodParamsFinished()
        {
            Parser.CurrentClass.CreateMethods.Add(Parser.CurrentCreateMethod);
            Parser.CurrentClass.Events.Add(Parser.CurrentEvent);
            return new DomainClassOpenedState(Parser);
        }
        
    }
}