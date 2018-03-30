using System.Collections.Generic;
using Microwave.LanguageModel;
using Microwave.WebServiceModel.Domain;

namespace Microwave.WebServiceModel.Application
{
    public class EventStoreRepositoryInterface : DomainClass
    {
        public EventStoreRepositoryInterface()
        {
            Name = "IEventStoreRepository";
            Methods = new List<DomainMethod>
            {
                new DomainMethod
                {
                    Name = "AddEvents",
                    ReturnType = "Task",
                    Parameters = {new Parameter {Name = "domainEvents", Type = $"List<{new DomainEventBaseClass().Name}>"}}
                }
            };
        }
    }
}