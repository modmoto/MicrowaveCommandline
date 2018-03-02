using System;
using System.Collections.Generic;

namespace GenericWebServiceBuilder.DSL
{
    internal class DomainFunction
    {
        public DomainFunction()
        {
            Parameters = new List<Parameter>(); 
        }

        public string Name { get; set; }
        public Type ReturnType { get; set; }
        public IList<Parameter> Parameters { get; set; }
    }
}