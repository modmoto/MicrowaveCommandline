using System.Collections.Generic;

namespace Microwave.LanguageModel
{
    public class DomainTree
    {
        public DomainTree(List<DomainClass> classes, List<SynchronousDomainHook> synchronousDomainHooks, List<AsyncDomainHook> asyncDomainHooks, List<OnChildDomainHook> onChildHooks)
        {
            Classes = classes;
            SynchronousDomainHooks = synchronousDomainHooks;
            AsyncDomainHooks = asyncDomainHooks;
            OnChildHooks = onChildHooks;
        }

        public List<DomainClass> Classes { get; }
        public List<SynchronousDomainHook> SynchronousDomainHooks { get; }
        public List<AsyncDomainHook> AsyncDomainHooks { get; }
        public List<OnChildDomainHook> OnChildHooks { get; }
    }
}