namespace FileToDslModel.Lexer
{
    public class DslToken
    {
        public DslToken(TokenType tokenType, string value, int lineNumber)
        {
            TokenType = tokenType;
            Value = value;
            LineNumber = lineNumber;
        }

        public TokenType TokenType { get; }
        public string Value { get; }
        public int LineNumber { get; }
    }
}