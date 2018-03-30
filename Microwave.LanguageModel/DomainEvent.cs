using System.Collections.Generic;

namespace Microwave.LanguageModel
{
    public class DomainEvent
    {
        public DomainEvent()
        {
            Properties = new List<Property>();
        }

        public string Name { get; set; }
        public IList<Property> Properties { get; }
    }
}