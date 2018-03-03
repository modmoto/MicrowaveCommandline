using System;
using GenericWebServiceBuilder.FileToDSL.Lexer;

namespace GenericWebServiceBuilder.FileToDSL.ParseAutomat
{
    public class NoTransitionException : Exception
    {
        public NoTransitionException(DslToken token) : base($"Unexpected Token after {token.Value} at Line {token.LineNumber}")
        {
        }
    }
}