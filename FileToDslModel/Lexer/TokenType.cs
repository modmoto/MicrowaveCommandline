namespace FileToDslModel.Lexer
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
        ParamSeparator
    }
}