using System.Collections.Generic;
using Microwave.LanguageModel;

namespace Microwave.WebServiceModel.Domain
{
    public class DomainEventBaseClass
    {
        public string Name => "DomainEventBase";
        public IList<Property> Properties => new List<Property>
        {
            new Property { Name = "Id", Type = "Guid" },
            new Property { Name = "CreatedAt", Type = "DateTimeOffset" },
            new Property { Name = "EntityId", Type = "Guid" }
        };
    }
}