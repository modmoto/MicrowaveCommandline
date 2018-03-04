using DslModel;
using FileToDslModel.Lexer;
using FileToDslModel.ParseAutomat;

namespace FileToDslModel
{
    public class DslParser
    {
        private readonly ITokenizer _tokenizer;
        private readonly Parser _parser;

        public DslParser(ITokenizer tokenizer, Parser parser)
        {
            _tokenizer = tokenizer;
            _parser = parser;
        }

        public DomainTree Parse(string file)
        {
            var dslTokens = _tokenizer.Tokenize(file);
            var domainTree = _parser.Parse(dslTokens);
            return domainTree;
        }
    }
}