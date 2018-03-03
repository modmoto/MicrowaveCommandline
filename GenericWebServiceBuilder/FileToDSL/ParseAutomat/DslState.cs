namespace GenericWebServiceBuilder.FileToDSL.ParseAutomat
{
    public enum DslState
    {
        Start,
        DomainClassIdentifierFound,
        DomainClassNameFound,
        OpeningBracketForDomainClassFound,
        DomainClassOpened,
        DomainClassClosed,
        PropertyNameFound,
        PropertyDefCharFound,
        PropertyTypeEnded,
    }
}