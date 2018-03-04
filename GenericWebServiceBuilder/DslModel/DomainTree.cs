using System.Collections.Generic;

namespace GenericWebServiceBuilder.DslModel
{
    public class DomainTree
    {
        public DomainTree(IList<DomainClass> classes, IList<DomainEvent> events)
        {
            Classes = classes;
            Events = events;
        }

        public IList<DomainClass> Classes { get; }
        public IList<DomainEvent> Events { get; }
    }
}