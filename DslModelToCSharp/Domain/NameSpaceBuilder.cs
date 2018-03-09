using System.CodeDom;

namespace DslModelToCSharp
{
    public interface INameSpaceBuilder
    {
        CodeNamespace BuildWithListImport(string domain);
        CodeNamespace Build(string domain);
    }

    public class NameSpaceBuilder : INameSpaceBuilder
    {
        public CodeNamespace BuildWithListImport(string domain)
        {
            var nameSpace = Build(domain);
            nameSpace.Imports.Add(new CodeNamespaceImport("System.Collections.Generic"));
            return nameSpace;
        }

        public CodeNamespace Build(string domain)
        {
            var nameSpaceName = domain;
            var nameSpace = new CodeNamespace(nameSpaceName);
            nameSpace.Imports.Add(new CodeNamespaceImport("System"));
            return nameSpace;
        }
    }
}