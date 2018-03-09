using System.Collections.Generic;
using DslModel.Domain;
using FileToDslModel.Lexer;

namespace FileToDslModel.ParseAutomat
{
    public interface IParser
    {
        DomainTree Parse(IEnumerable<DslToken> tokens);
    }
}