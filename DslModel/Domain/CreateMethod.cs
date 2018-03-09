using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace DslModel.Domain
{
    public class CreateMethod
    {
        public CreateMethod()
        {
            Parameters = new Collection<Parameter>();
        }

        public string Name => "Create";
        public string ReturnType { get; set; }
        public ICollection<Parameter> Parameters { get; }
    }
}