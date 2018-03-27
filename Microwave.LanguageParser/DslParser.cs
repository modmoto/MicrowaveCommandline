using Microwave.LanguageModel.Domain;
using Microwave.LanguageParser.Lexer;
using Microwave.LanguageParser.ParseAutomat;

namespace Microwave.LanguageParser
{
    public class DslParser
    {
        private readonly ITokenizer _tokenizer;
        private readonly MicrowaveLanguageParser _microwaveLanguageParser;

        public DslParser(ITokenizer tokenizer, MicrowaveLanguageParser microwaveLanguageParser)
        {
            _tokenizer = tokenizer;
            _microwaveLanguageParser = microwaveLanguageParser;
        }

        // new stuff

        public DomainTree Parse(string file)
        {
            var dslTokens = _tokenizer.Tokenize(file);
            var domainTree = _microwaveLanguageParser.Parse(dslTokens);
            return domainTree;
        }
    }
}