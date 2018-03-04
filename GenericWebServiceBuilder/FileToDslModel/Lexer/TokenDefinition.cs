using System.Text.RegularExpressions;

namespace GenericWebServiceBuilder.FileToDslModel.Lexer
{
    public class TokenDefinition
    {
        private readonly Regex _regex;
        private readonly TokenType _returnsToken;

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