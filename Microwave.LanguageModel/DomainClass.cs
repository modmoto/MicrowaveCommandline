using System.Collections.Generic;
using System.Linq;

namespace Microwave.LanguageModel.Domain
{
    public class DomainClass
    {
        public DomainClass()
        {
            Methods = new List<DomainMethod>();
            Properties = new List<Property>();
            ListProperties = new List<ListProperty>();
            CreateMethods = new List<CreateMethod>();
            Events = new List<DomainEvent>();
        }

        public List<ListProperty> ListProperties { get; protected set; }
        public IList<DomainMethod> Methods { get; protected set; }
        public IList<DomainEvent> Events { get; }
        public IList<CreateMethod> CreateMethods { get; }
        public string Name { get; set; }
        public IList<Property> Properties { get; protected set; }
        public IEnumerable<DomainMethod> LoadMethods => Methods.Where(method => method.LoadParameters.Count > 0);
    }
}