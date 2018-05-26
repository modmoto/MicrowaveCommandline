using System.Collections.Generic;
using System.Linq;

namespace Microwave.LanguageModel
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
            ChildHookMethods = new List<OnChildHookMethod>();
        }

        public List<ListProperty> ListProperties { get; }
        public IList<DomainMethod> Methods { get; protected set; }
        public IList<DomainEvent> Events { get; }
        public IList<CreateMethod> CreateMethods { get; }
        public string Name { get; set; }
        public List<Property> Properties { get; protected set; }
        public List<OnChildHookMethod> ChildHookMethods { get; }
        public IEnumerable<DomainMethod> LoadMethods => Methods.Where(method => method.LoadParameters.Count > 0);
    }
}