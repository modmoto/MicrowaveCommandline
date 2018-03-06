using System.Collections.Generic;

namespace DslModel
{
    public class ValidationResultBaseClass
    {
        public string Name => "ValidationResult";

        public IList<Property> Properties => new List<Property>
        {
            new Property {Name = "Ok", Type = "Boolean" },
            new Property {Name = "DomainEvents", Type = $"IList<{new DomainEventBaseClass().Name}>"},
            new Property {Name = "DomainErrors", Type = "IList<string>"}
        };
    }
}