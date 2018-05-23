using System.Collections.Generic;

namespace Microwave.LanguageModel
{
    public class DomainTree
    {
        public DomainTree(List<DomainClass> classes, List<SynchronousDomainHook> synchronousDomainHooks, List<AsyncDomainHook> asyncDomainHooks)
        {
            Classes = classes;
            SynchronousDomainHooks = synchronousDomainHooks;
            AsyncDomainHooks = asyncDomainHooks;
        }

        public List<DomainClass> Classes { get; }
        public List<SynchronousDomainHook> SynchronousDomainHooks { get; }
        public List<AsyncDomainHook> AsyncDomainHooks { get; }
    }
}