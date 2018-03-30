using System.Collections.Generic;
using Microwave.LanguageModel;
using Microwave.LanguageParser.Lexer;

namespace Microwave.LanguageParser.ParseAutomat
{
    public interface IParser
    {
        DomainTree Parse(IEnumerable<DslToken> tokens);
    }
}