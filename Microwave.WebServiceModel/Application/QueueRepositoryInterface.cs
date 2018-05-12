using System.Collections.Generic;
using Microwave.LanguageModel;

namespace Microwave.WebServiceModel.Application
{
    public class QueueRepositoryInterface : DomainClass
    {
        public QueueRepositoryInterface()
        {
            Name = "IQueueRepository";
            Methods = new List<DomainMethod>
            {
                new DomainMethod
                {
                    Name = "AddEventForJob",
                    ReturnType = "Task",
                    Parameters = {new Parameter {Name = "eventAndJob", Type = new EventAndJobClass().Name}}
                },
                new DomainMethod
                {
                    Name = "RemoveEventsFromQueue",
                    Parameters =
                    {
                        new Parameter
                        {
                            Name = "handledEvents",
                            Type = $"List<{new EventAndJobClass().Name}>"
                        }
                    },
                    ReturnType = "Task"
                },
                new DomainMethod
                {
                    Name = "GetEvents",
                    ReturnType = $"Task<List<{new EventAndJobClass().Name}>>",
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
}