namespace Microwave.LanguageModel
{
    public class OnChildHookMethod : DomainMethod
    {
        public string OriginFieldName { get; set; }
        public string MethodName { get; set; }
        public string ContainingClassName { get; set; }
    }
}