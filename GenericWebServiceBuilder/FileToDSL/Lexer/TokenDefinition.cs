using System.Text.RegularExpressions;

namespace GenericWebServiceBuilder.FileToDSL.Lexer
{
    public class TokenDefinition
    {
        private readonly TokenType _returnsToken;
        private readonly Regex _regex;

        public TokenDefinition(TokenType returnsToken, string regexPattern)
        {
            _regex = new Regex(regexPattern, RegexOptions.IgnoreCase);
            _returnsToken = returnsToken;
        }

        public TokenMatch Match(string inputString)
        {
            var match = _regex.Match(inputString);
            if (match.Success)
            {
                var remainingText = string.Empty;
                if (match.Length != inputString.Length)
                    remainingText = inputString.Substring(match.Length);

                return new TokenMatch
                {
                    IsMatch = true,
                    RemainingText = remainingText,
                    TokenType = _returnsToken,
                    Value = match.Value
                };
            }

            return new TokenMatch {IsMatch = false};
        }
    }
}