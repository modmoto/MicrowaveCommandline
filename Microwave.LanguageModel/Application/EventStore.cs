using System.Collections.Generic;
using Microwave.LanguageModel.Domain;

namespace Microwave.LanguageModel.Application
{
    public class EventStore : DomainClass
    {
        public EventStore()
        {
            Name = "EventStore";
            Methods = new List<DomainMethod>
            {
                new DomainMethod
                {
                    Name = "AddEvents",
                    ReturnType = "Task",
                    Parameters = {new Parameter {Name = "domainEvents", Type = "List<DomainEventBase>"}}
                }
            };
            Properties = new List<Property>
            {
                new Property {Name = "EventStoreRepository", Type = new EventStoreRepositoryInterface().Name}
            };
            ListProperties = new List<ListProperty>
            {
                new ListProperty {Name = "DomainHooks", Type = new DomainHookBaseClass().Name}
            };
            Methods = new List<DomainMethod>
            {
                new DomainMethod
                {
                    Name = "AppendAll",
                    ReturnType = $"async Task<{new HookResultBaseClass().Name}>",
                    Parameters = { new Parameter
                    {
                        Name = "domainEvents",
                        Type = $"List<{new DomainEventBaseClass().Name}>"
                    }}
                }
            };
        }
    }
}