using System.Collections.Generic;
using GenericWebServiceBuilder.DslModel;
using GenericWebServiceBuilder.FileToDslModel.Lexer;

namespace GenericWebServiceBuilder.FileToDslModel.ParseAutomat
{
    public interface IParser
    {
        DomainTree Parse(IEnumerable<DslToken> tokens);
    }
}