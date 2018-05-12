using System.Collections.Generic;
using Microwave.LanguageModel;

namespace Microwave.WebServiceModel.SqlAdapter
{
    public class EventTupleClass : DomainClass
    {
        public EventTupleClass()
        {
            Name = "EventTuple";
            Properties = new List<Property>
            {
                new Property
                {
                    Name = "DomainType",
                    Type = " string"
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