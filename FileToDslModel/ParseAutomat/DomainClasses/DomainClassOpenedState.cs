using DslModel;
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
            Parser.CurrentEvent = new DomainEvent {Name = $"Create{Parser.CurrentClass.Name}Event"};
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

    internal class CreateMethodFoundState : ParseState
    {
        public CreateMethodFoundState(Parser parser) : base(parser)
        {
        }

        public override ParseState Parse(DslToken token)
        {
            switch (token.TokenType)
            {
                case TokenType.ParameterBracketOpen:
                    return CreateParamsStarted();
                default:
                    throw new NoTransitionException(token);
            }
        }

        private ParseState CreateParamsStarted()
        {
            return new CreateParamsStartedState(Parser);
        }
    }

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

    internal class ParamCreateMethodStartedState : ParseState
    {
        public ParamCreateMethodStartedState(Parser parser) : base(parser)
        {
        }

        public override ParseState Parse(DslToken token)
        {
            switch (token.TokenType)
            {
                case TokenType.TypeDefSeparator:
                    return ParamCreateMethodTypeDefFound();
                default:
                    throw new NoTransitionException(token);
            }
        }

        private ParseState ParamCreateMethodTypeDefFound()
        {
            return new ParamCreateMethodTypeDefFoundState(Parser);
        }
    }

    internal class ParamCreateMethodTypeDefFoundState : ParseState
    {
        public ParamCreateMethodTypeDefFoundState(Parser parser) : base(parser)
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
            Parser.CurrentParam.Type = token.Value;
            Parser.CurrentCreateMethod.Parameters.Add(Parser.CurrentParam);
            return new PropertyCreateMethodTypeFoundState(Parser);
        }
    }

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