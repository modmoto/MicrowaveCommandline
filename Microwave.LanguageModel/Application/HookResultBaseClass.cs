using System.Collections.Generic;
using Microwave.LanguageModel.Domain;

namespace Microwave.LanguageModel.Application
{
    public class HookResultBaseClass
    {
        public string Name => "HookResult";

        public IList<Property> Properties => new List<Property>
        {
            new Property {Name = "Ok", Type = "Boolean" },
            new Property {Name = "Errors", Type = "List<string>"}
        };
    }
}