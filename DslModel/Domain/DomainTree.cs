using System.Collections.Generic;

namespace DslModel.Domain
{
    public class DomainTree
    {
        public DomainTree(IList<DomainClass> classes, IList<SynchronousDomainHook> domainHooks)
        {
            Classes = classes;
            DomainHooks = domainHooks;
        }

        public IList<DomainClass> Classes { get; }
        public IList<SynchronousDomainHook> DomainHooks { get; }
    }

    public class SynchronousDomainHook
    {
        public string ClassType { get; set; }
        public string MethodName { get; set; }
    }
}