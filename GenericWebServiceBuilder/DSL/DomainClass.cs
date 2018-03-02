using System;
using System.Collections.Generic;
using System.Text;

namespace GenericWebServiceBuilder.DSL
{
    public class DomainClass
    {
        public DomainClass()
        {
            Functions = new List<DomainFunction>();
            Propteries = new List<Property>();
        }
        public IList<DomainFunction> Functions { get; set; }
        public string Name { get; set; }
        public IList<Property> Propteries { get; set; }
    }
}
