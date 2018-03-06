using System.Collections.Generic;

namespace DslModel
{
    public class DomainEventBaseClass
    {
        public string Name => "DomainEventBase";
        public IList<Property> Properties => new List<Property>
        {
            new Property { Name = "EntityId", Type = "Guid" }
        };
    }
}