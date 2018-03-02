using System.Collections.Generic;

namespace GenericWebServiceBuilder.DomainSpecificGrammar
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