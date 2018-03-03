namespace GenericWebServiceBuilder.FileToDSL.Lexer
{
    public enum TokenType
    {
        NotDefined,
        OpenParenthesis,
        CloseParenthesis,
        DomainClass,
        DomainEvent,
        ListBracketOpen,
        ListBracketClose,
        Value,
        ParameterBracketOpen,
        ParameterBracketClose,
        TypeNameDef,
        TypeName,
        TypeDef,
        PropertyDefEnd,
        SequenceTerminator
    }
}