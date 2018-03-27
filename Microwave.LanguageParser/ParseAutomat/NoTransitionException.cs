using System;
using Microwave.LanguageParser.Lexer;

namespace Microwave.LanguageParser.ParseAutomat
{
    public class NoTransitionException : Exception
    {
        public NoTransitionException(DslToken token) : base(
            $"Unexpected Token \"{token.Value}\" before {token.Value} at Line {token.LineNumber}")
        {
        }
    }
}