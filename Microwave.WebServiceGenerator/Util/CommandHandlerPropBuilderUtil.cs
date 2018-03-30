using System.Collections.Generic;
using System.Linq;
using Microwave.LanguageModel;
using Microwave.WebServiceGenerator.Application;

namespace Microwave.WebServiceGenerator.Util
{
    public class CommandHandlerPropBuilderUtil
    {
        public IList<Property> Build(DomainClass domainClass)
        {
            var properties = new List<Property>
            {
                new Property {Name = "EventStore", Type = new EventStoreInterface().Name},
                new Property {Name = $"{domainClass.Name}Repository", Type = $"I{domainClass.Name}Repository"}
            };
            foreach (var loadMethod in domainClass.LoadMethods)
            {
                foreach (var loadParam in loadMethod.LoadParameters)
                {
                    var repoWithSameName = properties.FirstOrDefault(prop => prop.Name == $"{loadParam.Type}Repository");
                    if (repoWithSameName == null)
                    {
                        properties.Add(new Property { Name = $"{loadParam.Type}Repository", Type = $"I{loadParam.Type}Repository" });
                    }
                }
            }
            return properties;
        }
    }
}