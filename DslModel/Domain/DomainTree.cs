using System.Collections.Generic;

namespace DslModel.Domain
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

    public class SynchronousDomainHook
    {
        public string ClassType { get; set; }
        public string MethodName { get; set; }
        public string Name { get; set; }
    }
}