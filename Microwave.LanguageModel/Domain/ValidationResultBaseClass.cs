using System.Collections.Generic;

namespace Microwave.LanguageModel.Domain
{
    public class ValidationResultBaseClass
    {
        public string Name => "ValidationResult";

        public IList<Property> Properties => new List<Property>
        {
            new Property {Name = "Ok", Type = "Boolean" },
            new Property {Name = "DomainEvents", Type = $"List<{new DomainEventBaseClass().Name}>"},
            new Property {Name = "DomainErrors", Type = "List<string>"}
        };
    }
}