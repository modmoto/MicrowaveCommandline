using System.Collections.Generic;

namespace GenericWebServiceBuilder.FileToDSL.Lexer
{
    public interface ITokenizer
    {
        List<DslToken> Tokenize(string lqlText);
    }
}