using System.Collections.Generic;
using Microwave.LanguageModel;
using Microwave.WebServiceModel.Domain;

namespace Microwave.WebServiceModel.Application
{
    public class HangfireQueueInterface : DomainClass
    {
        public HangfireQueueInterface()
        {
            Name = "IHangfireQueue";
            Methods = new List<DomainMethod>
            {
                new DomainMethod
                {
                    Name = "AddEvents",
                    ReturnType = "Task",
                    Parameters = {new Parameter {Name = "domainEvents", Type = $"List<{new DomainEventBaseClass().Name}>"}}
                },
                new DomainMethod
                {
                    Name = "RemoveEventsFromQueue",
                    Parameters =
                    {
                        new Parameter
                        {
                            Name = "handledEvents",
                            Type = $"List<{new EventJobClass().Name}>"
                        }
                    },
                    ReturnType = "Task"
                },
                new DomainMethod
                {
                    Name = "GetEvents",
                    ReturnType = $"Task<List<{new EventJobClass().Name}>>",
                    Parameters =
                    {
                        new Parameter
                        {
                            Name = "jobName",
                            Type = $" string"
                        }
                    }
                }
            };
        }
    }

    public class EventJobClass : DomainClass
    {
        public EventJobClass()
        {
            Name = "EventAndJob";
        }
    }
}