using System.Collections.Generic;

namespace DslModel.Domain
{
    public class DomainTree
    {
        public DomainTree(IList<DomainClass> classes)
        {
            Classes = classes;
        }

        public IList<DomainClass> Classes { get; }
    }
}