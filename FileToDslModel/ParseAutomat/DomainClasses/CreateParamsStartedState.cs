using DslModel.Domain;
using FileToDslModel.Lexer;

namespace FileToDslModel.ParseAutomat.DomainClasses
{
    internal class CreateParamsStartedState : ParseState
    {
        public CreateParamsStartedState(Parser parser) : base(parser)
        {
        }

        public override ParseState Parse(DslToken token)
        {
            switch (token.TokenType)
            {
                case TokenType.ParameterBracketClose:
                    return CreateMethodParamsFinished();
                case TokenType.Value:
                    return ParamCreateMethodStarted(token);
                default:
                    throw new NoTransitionException(token);
            }
        }
        private ParseState CreateMethodParamsFinished()
        {
            Parser.CurrentClass.CreateMethods.Add(Parser.CurrentCreateMethod);
            Parser.CurrentClass.Events.Add(Parser.CurrentEvent);
            return new DomainClassOpenedState(Parser);
        }

        private ParseState ParamCreateMethodStarted(DslToken token)
        {
            Parser.CurrentParam = new Parameter { Name = token.Value };
            return new ParamCreateMethodStartedState(Parser);
        }
    }
}