using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace DslModel
{
    public class DomainClass
    {
        public DomainClass()
        {
            Methods = new Collection<DomainMethod>();
            Propteries = new Collection<Property>();
            CreateMethods = new Collection<CreateMethod>();
        }

        public ICollection<DomainMethod> Methods { get; set; }
        public ICollection<CreateMethod> CreateMethods { get; set; }
        public string Name { get; set; }
        public ICollection<Property> Propteries { get; set; }
    }
}