using System.Collections.Generic;
using Microwave.LanguageModel;
using Microwave.WebServiceModel.Application;

namespace Microwave.WebServiceModel.SqlAdapter
{
    public class HangfireQueueClass : DomainClass
    {
        public HangfireQueueClass()
        {
            Name = "HangfireQueue";
            Properties = new List<Property>
            {
                new Property
                {
                    Name = "RegisteredJobs",
                    Type = "EventJobRegistration"
                },
                new Property
                {
                    Name = "QueueRepository",
                    Type = new QueueRepositoryInterface().Name
                }
            };
        }
    }
}