using System.Collections.Generic;
using GenericWebServiceBuilder.DomainSpecificGrammar;
using GenericWebServiceBuilder.FileToDSL.Lexer;

namespace GenericWebServiceBuilder.FileToDSL.ParseAutomat
{
    public interface IParser
    {
        DomainTree Parse(IEnumerable<DslToken> tokens);
    }
}