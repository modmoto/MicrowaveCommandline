using System.Collections.Generic;

namespace Microwave.LanguageModel
{
    public class DomainMethod
    {
        public DomainMethod()
        {
            Parameters = new List<Parameter>();
            LoadParameters = new List<Parameter>();
        }

        public string Name { get; set; }
        public string ReturnType { get; set; }
        public IList<Parameter> Parameters { get; }
        public IList<Parameter> LoadParameters { get; }
    }
}