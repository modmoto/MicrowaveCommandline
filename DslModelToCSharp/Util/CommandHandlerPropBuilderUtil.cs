using System.Collections.Generic;
using DslModel.Domain;

namespace DslModelToCSharp.Util
{
    public class CommandHandlerPropBuilderUtil
    {
        public IList<Property> Build(DomainClass domainClass)
        {
            return new List<Property>
            {
                new Property {Name = "EventStore", Type = "EventStore"},
                new Property {Name = $"{domainClass.Name}Repository", Type = $"I{domainClass.Name}Repository"}
            };
        }
    }
}