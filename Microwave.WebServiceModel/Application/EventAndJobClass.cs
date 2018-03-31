using System.Collections.Generic;
using Microwave.LanguageModel;
using Microwave.WebServiceModel.Domain;

namespace Microwave.WebServiceModel.Application
{
    public class EventAndJobClass : DomainClass
    {
        public EventAndJobClass()
        {
            Name = "EventAndJob";
            Properties = new List<Property>
            {
                new Property
                {
                    Name = "Id",
                    Type = "Guid"
                },
                new Property
                {
                    Name = "DomainEvent",
                    Type = new DomainEventBaseClass().Name
                },
                new Property
                {
                    Name = "JobName",
                    Type = " string"
                },
            };
        }
    }
}