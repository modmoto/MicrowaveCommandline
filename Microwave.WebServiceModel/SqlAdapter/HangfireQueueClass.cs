using System.Collections.Generic;
using Microwave.LanguageModel;

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
                    Name = "Context",
                    Type = "HangfireContext"
                }
            };
        }
    }
}