using System.Collections.Generic;

namespace GenericWebServiceBuilder.DomainSpecificGrammar
{
    public class DomainTree
    {
        public DomainTree(ICollection<DomainClass> classes, ICollection<DomainEvent> events)
        {
            Classes = classes;
            Events = events;
        }

        public ICollection<DomainClass> Classes { get; }
        public ICollection<DomainEvent> Events { get; }
    }
}