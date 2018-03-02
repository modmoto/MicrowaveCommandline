using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace GenericWebServiceBuilder.DomainSpecificGrammar
{
    public class DomainMethod
    {
        public DomainMethod()
        {
            Parameters = new Collection<Parameter>();
        }

        public string Name { get; set; }
        public Type ReturnType { get; set; }
        public ICollection<Parameter> Parameters { get; set; }
    }
}