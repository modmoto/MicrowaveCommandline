using System.CodeDom;

namespace DslModelToCSharp
{
    public interface INameSpaceBuilder
    {
        CodeNamespace Build(string domain);
    }

    public class NameSpaceBuilder : INameSpaceBuilder
    {
        public CodeNamespace Build(string domain)
        {
            var nameSpaceName = domain;
            var nameSpace = new CodeNamespace(nameSpaceName);
            nameSpace.Imports.Add(new CodeNamespaceImport("System"));
            nameSpace.Imports.Add(new CodeNamespaceImport("System.Collections.Generic"));
            return nameSpace;
        }
    }
}