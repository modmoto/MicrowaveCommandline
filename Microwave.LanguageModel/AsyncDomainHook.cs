namespace Microwave.LanguageModel
{
    public class AsyncDomainHook
    {
        public bool IsCreateHook { get; set; }
        public string ClassType { get; set; }
        public string MethodName { get; set; }
        public string Name { get; set; }
    }
}