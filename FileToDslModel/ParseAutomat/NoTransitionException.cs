using System;
using FileToDslModel.Lexer;

namespace FileToDslModel.ParseAutomat
{
    public class NoTransitionException : Exception
    {
        public NoTransitionException(DslToken token) : base(
            $"Unexpected Token after {token.Value} at Line {token.LineNumber}")
        {
        }
    }
}