using System.Collections.Generic;
using DslModel;
using FileToDslModel.Lexer;

namespace FileToDslModel.ParseAutomat
{
    public interface IParser
    {
        DomainTree Parse(IEnumerable<DslToken> tokens);
    }
}