using System.Collections.Generic;

namespace Microwave.LanguageModel.Domain
{
    public class CreationResultBaseClass
    {
        public string Name => "CreationResult";
        public string GenericType => "T";

        public IList<Property> Properties => new List<Property>
        {
            new Property {Name = "Ok", Type = "Boolean" },
            new Property {Name = "DomainEvents", Type = $"List<{new DomainEventBaseClass().Name}>"},
            new Property {Name = "DomainErrors", Type = "List<string>"},
            new Property {Name = "CreatedEntity", Type = GenericType}
        };
    }
}