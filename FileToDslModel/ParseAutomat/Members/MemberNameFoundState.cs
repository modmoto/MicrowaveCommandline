using DslModel.Domain;
using FileToDslModel.Lexer;
using FileToDslModel.ParseAutomat.Members.ListProperties;
using FileToDslModel.ParseAutomat.Members.Methods;
using FileToDslModel.ParseAutomat.Members.Properties;

namespace FileToDslModel.ParseAutomat.Members
{
    public class MemberNameFoundState : ParseState
    {
        public MemberNameFoundState(Parser parser) : base(parser)
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
            Parser.CurrentMethod = new DomainMethod
            {
                Name = Parser.CurrentMemberName,
                ReturnType = new ValidationResultBaseClass().Name
            };
            return new MethodParamOpenState(Parser);
        }

        private ParseState PropertySeparatorFound()
        {
            Parser.CurrentProperty = new Property {Name = Parser.CurrentMemberName};
            return new PropertySeparatorFoundState(Parser);
        }
    }

    internal class SynchronousDomainHookFoundState : ParseState
    {
        public SynchronousDomainHookFoundState(Parser parser) : base(parser)
        {
        }

        public override ParseState Parse(DslToken token)
        {
            switch (token.TokenType)
            {
                case TokenType.Value:
                    return SynchronousDomainHookNameFound(token);
                default:
                    throw new NoTransitionException(token);
            }
        }

        private ParseState SynchronousDomainHookNameFound(DslToken token)
        {
            Parser.CurrentSynchronousDomainHook = new SynchronousDomainHook()
            {
                ClassType = token.Value
            };

            return new SynchronousDomainHookNameFoundState(Parser);
        }
    }

    internal class SynchronousDomainHookNameFoundState : ParseState
    {
        public SynchronousDomainHookNameFoundState(Parser parser) : base(parser)
        {
        }

        public override ParseState Parse(DslToken token)
        {
            switch (token.TokenType)
            {
                case TokenType.DomainHookOn:
                    return SynchronousDomainHookOnEventFound();
                default:
                    throw new NoTransitionException(token);
            }
        }

        private ParseState SynchronousDomainHookOnEventFound()
        {
            return new SynchronousDomainHookOnEventFoundState(Parser);
        }
    }

    internal class SynchronousDomainHookOnEventFoundState : ParseState
    {
        public SynchronousDomainHookOnEventFoundState(Parser parser) : base(parser)
        {
        }

        public override ParseState Parse(DslToken token)
        {
            switch (token.TokenType)
            {
                case TokenType.DomainHookEventDefinition:
                    return SynchronousDomainHookEventFound(token);
                default:
                    throw new NoTransitionException(token);
            }
        }


        private ParseState SynchronousDomainHookEventFound(DslToken token)
        {
            var strings = token.Value.Split(".");
            Parser.CurrentSynchronousDomainHook.ClassType = strings[0];
            Parser.CurrentSynchronousDomainHook.MethodName = strings[1];
           
            Parser.SynchronousDomainHooks.Add(Parser.CurrentSynchronousDomainHook);

            return new StartState(Parser);
        }
    }
}