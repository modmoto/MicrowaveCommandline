using System.Collections.Generic;

namespace GenericWebServiceBuilder.FileToDSL.Lexer
{
    public class Tokenizer : ITokenizer
    {
        private readonly List<TokenDefinition> _tokenDefinitions;

        public Tokenizer()
        {
            _tokenDefinitions = new List<TokenDefinition>
            {
                new TokenDefinition(TokenType.ObjectBracketOpen, "^\\{"),
                new TokenDefinition(TokenType.ObjectBracketClose, "^\\}"),

                new TokenDefinition(TokenType.ParameterBracketOpen, "^\\("),
                new TokenDefinition(TokenType.ParameterBracketClose, "^\\)"),

                new TokenDefinition(TokenType.ListBracketOpen, "^\\["),
                new TokenDefinition(TokenType.ListBracketClose, "^\\]"),

                new TokenDefinition(TokenType.TypeDefSeparator, "^:"),

                new TokenDefinition(TokenType.DomainClass, "^DomainClass"),
                new TokenDefinition(TokenType.DomainEvent, "^DomainEvent"),

                new TokenDefinition(TokenType.Value, "^\\w+")
            };
        }

        public List<DslToken> Tokenize(string lqlText)
        {
            var tokens = new List<DslToken>();
            var remainingText = lqlText;

            while (!string.IsNullOrWhiteSpace(remainingText))
            {
                var match = FindMatch(remainingText);
                if (match.IsMatch)
                {
                    tokens.Add(new DslToken(match.TokenType, match.Value));
                    remainingText = match.RemainingText;
                }
                else
                {
                    remainingText = remainingText.Substring(1);
                }
            }

            tokens.Add(new DslToken(TokenType.SequenceTerminator, string.Empty));

            return tokens;
        }

        private TokenMatch FindMatch(string lqlText)
        {
            foreach (var tokenDefinition in _tokenDefinitions)
            {
                var match = tokenDefinition.Match(lqlText);
                if (match.IsMatch)
                    return match;
            }

            return new TokenMatch {IsMatch = false};
        }
    }
}