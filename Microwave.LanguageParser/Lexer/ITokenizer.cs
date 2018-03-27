using System.Collections.Generic;

namespace Microwave.LanguageParser.Lexer
{
    public interface ITokenizer
    {
        List<DslToken> Tokenize(string lqlText);
    }
}