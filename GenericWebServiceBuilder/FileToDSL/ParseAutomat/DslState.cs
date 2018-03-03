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
        DomainEventClosed
    }
}