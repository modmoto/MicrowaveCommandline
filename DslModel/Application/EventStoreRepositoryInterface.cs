using System.Collections.Generic;
using DslModel.Domain;

namespace DslModel.Application
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
                    Parameters = {new Parameter {Name = "domainEvents", Type = "List<DomainEventBase> "}}
                }
            };
        }
    }
}