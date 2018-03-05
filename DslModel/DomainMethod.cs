using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace DslModel
{
    public class DomainMethod
    {
        public DomainMethod()
        {
            Parameters = new Collection<Parameter>();
        }

        public string Name { get; set; }
        public string ReturnType { get; set; }
        public ICollection<Parameter> Parameters { get; }
    }
}