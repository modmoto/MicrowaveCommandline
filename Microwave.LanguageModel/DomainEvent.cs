using System.Collections.Generic;

namespace Microwave.LanguageModel.Domain
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