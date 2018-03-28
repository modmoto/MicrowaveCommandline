using System.Collections.Generic;
using Microwave.LanguageModel.Domain;
using Microwave.WebServiceModel.Application;
using Microwave.WebServiceModel.Domain;

namespace Microwave.WebServiceModel.SqlAdapter
{
    public class EventStoreRepository : DomainClass
    {
        public EventStoreRepository()
        {
            Name = "EventStoreRepository";
            Properties = new List<Property>
            {
                new Property { Name = "Context", Type = "EventStoreContext"}
            };
            Methods = new List<DomainMethod>
            {
                new DomainMethod
                {
                    Name = new EventStoreRepositoryInterface().Methods[0].Name,
                    ReturnType = "async Task",
                    Parameters = { new Parameter { Name = "domainEvents", Type = $"List<{new DomainEventBaseClass().Name}>"}}
                }
            };
        }
    }
}