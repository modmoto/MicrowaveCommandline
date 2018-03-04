using System.Collections.Generic;

namespace FileToDslModel.Lexer
{
    public interface ITokenizer
    {
        List<DslToken> Tokenize(string lqlText);
    }
}