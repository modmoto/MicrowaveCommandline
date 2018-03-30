using System.Collections.Generic;

namespace Microwave.LanguageModel
{
    public class DomainTree
    {
        public DomainTree(List<DomainClass> classes, List<SynchronousDomainHook> synchronousDomainHooks)
        {
            Classes = classes;
            SynchronousDomainHooks = synchronousDomainHooks;
        }

        public List<DomainClass> Classes { get; }
        public List<SynchronousDomainHook> SynchronousDomainHooks { get; }
    }
}