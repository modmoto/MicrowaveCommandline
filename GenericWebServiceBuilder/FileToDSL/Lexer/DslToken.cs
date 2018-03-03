namespace GenericWebServiceBuilder.FileToDSL.Lexer
{
    public class DslToken
    {
        public DslToken(TokenType tokenType, string value)
        {
            TokenType = tokenType;
            Value = value;
        }

        public TokenType TokenType { get; set; }
        public string Value { get; set; }
    }
}