namespace FileToDslModel.Lexer
{
    public enum TokenType
    {
        NotDefined,
        ObjectBracketOpen,
        ObjectBracketClose,
        DomainClass,
        ListBracketOpen,
        ListBracketClose,
        Value,
        ParameterBracketOpen,
        ParameterBracketClose,
        TypeDefSeparator,
        ParamSeparator,
        CreateMethod
    }
}