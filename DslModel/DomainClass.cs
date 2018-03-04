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
        }

        public ICollection<DomainMethod> Methods { get; set; }
        public string Name { get; set; }
        public ICollection<Property> Propteries { get; set; }
    }
}