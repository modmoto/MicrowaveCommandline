using System.Collections.Generic;

namespace DslModel.Domain
{
    public class DomainMethod
    {
        public DomainMethod()
        {
            Parameters = new List<Parameter>();
        }

        public string Name { get; set; }
        public string ReturnType { get; set; }
        public IList<Parameter> Parameters { get; }
    }
}