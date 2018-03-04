using System.Collections.Generic;

namespace GenericWebServiceBuilder.FileToDslModel.Lexer
{
    public interface ITokenizer
    {
        List<DslToken> Tokenize(string lqlText);
    }
}