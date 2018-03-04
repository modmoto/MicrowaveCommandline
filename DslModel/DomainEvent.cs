using System.Collections.Generic;

namespace DslModel
{
    public class DomainEvent
    {
        public DomainEvent()
        {
            Properties = new List<Property>();
        }

        public string Name { get; set; }
        public ICollection<Property> Properties { get; set; }
    }
}