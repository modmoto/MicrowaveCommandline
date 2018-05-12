using System.Collections.Generic;
using Microwave.LanguageModel;

namespace Microwave.WebServiceModel.SqlAdapter
{
    public class QueueRepositoryClass : DomainClass
    {
        public QueueRepositoryClass()
        {
            Name = "QueueRepository";
            Properties = new List<Property>
            {
                new Property
                {
                    Name = "Context",
                    Type = "HangfireContext"
                }
            };
        }
    }
}