namespace GenericWebServiceBuilder.FileToDSL.Lexer
{
    public enum TokenType
    {
        NotDefined,
        ObjectBracketOpen,
        ObjectBracketClose,
        DomainClass,
        DomainEvent,
        ListBracketOpen,
        ListBracketClose,
        Value,
        ParameterBracketOpen,
        ParameterBracketClose,
        TypeDefSeparator,
        SequenceTerminator
    }
}