using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace GenericWebServiceBuilder.DomainSpecificGrammar
{
    public class DomainClass
    {
        public DomainClass()
        {
            Functions = new Collection<DomainMethod>();
            Propteries = new Collection<Property>();
        }

        public ICollection<DomainMethod> Functions { get; set; }
        public string Name { get; set; }
        public ICollection<Property> Propteries { get; set; }
    }
}