using System.Collections.Generic;

namespace DslModel
{
    public class DomainClass
    {
        public DomainClass()
        {
            Methods = new List<DomainMethod>();
            Propteries = new List<Property>();
            CreateMethods = new List<CreateMethod>();
            Events = new List<DomainEvent>();
        }

        public IList<DomainMethod> Methods { get; }
        public IList<DomainEvent> Events { get; }
        public IList<CreateMethod> CreateMethods { get; }
        public string Name { get; set; }
        public IList<Property> Propteries { get; }
    }
}