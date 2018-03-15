using System.Collections.Generic;

namespace DslModel.Domain
{
    public class DomainTree
    {
        public DomainTree(IList<DomainClass> classes, IList<SynchronousDomainHook> synchronousDomainHooks)
        {
            Classes = classes;
            SynchronousDomainHooks = synchronousDomainHooks;
        }

        public IList<DomainClass> Classes { get; }
        public IList<SynchronousDomainHook> SynchronousDomainHooks { get; }
    }

    public class SynchronousDomainHook
    {
        public string ClassType { get; set; }
        public string MethodName { get; set; }
        public string Name { get; set; }
    }
}