﻿using FileToDslModel.Lexer;
using FileToDslModel.ParseAutomat.Members.Methods.Events;

namespace FileToDslModel.ParseAutomat.Members.Methods
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
}