using System.CodeDom;

namespace DslModelToCSharp
{
    public interface INameSpaceBuilder
    {
        CodeNamespace BuildWithListImport(string domain);
        CodeNamespace Build(string domain);
        CodeNamespace BuildWithMvcImport(string domain, string domainClassName);
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

        public CodeNamespace BuildWithMvcImport(string domain, string domainClassName)
        {
            var nameSpace = BuildWithListImport(domain);
            nameSpace.Imports.Add(new CodeNamespaceImport("System.Threading.Tasks"));
            nameSpace.Imports.Add(new CodeNamespaceImport("Domain"));
            nameSpace.Imports.Add(new CodeNamespaceImport($"Domain.{domainClassName}s"));
            nameSpace.Imports.Add(new CodeNamespaceImport("Microsoft.AspNetCore.Mvc"));
            return nameSpace;
        }

        public CodeNamespace BuildWithMvcApplicationImport(string domain, string domainClassName)
        {
            var nameSpace = BuildWithListImport(domain);
            nameSpace.Imports.Add(new CodeNamespaceImport("System.Threading.Tasks"));
            nameSpace.Imports.Add(new CodeNamespaceImport($"Domain.{domainClassName}s"));
            nameSpace.Imports.Add(new CodeNamespaceImport("Microsoft.AspNetCore.Mvc"));
            nameSpace.Imports.Add(new CodeNamespaceImport($"Application.{domainClassName}s"));
            return nameSpace;
        }

        public CodeNamespace BuildWithTask(string domain, string domainClassName)
        {
            var nameSpace = BuildWithListImport(domain);
            nameSpace.Imports.Add(new CodeNamespaceImport("System.Threading.Tasks"));
            nameSpace.Imports.Add(new CodeNamespaceImport($"Domain.{domainClassName}s"));
            return nameSpace;
        }

        public CodeNamespace BuildWithEfCore(string domain, string domainClassName)
        {
            var nameSpace = BuildWithTask(domain, domainClassName);
            nameSpace.Imports.Add(new CodeNamespaceImport($"Application.{domainClassName}s"));
            nameSpace.Imports.Add(new CodeNamespaceImport($"Microsoft.EntityFrameworkCore"));
            return nameSpace;
        }

        public CodeNamespace BuildWithDbImport(string domain)
        {
            var nameSpace = new CodeNamespace(domain);
            nameSpace.Imports.Add(new CodeNamespaceImport("Domain"));
            nameSpace.Imports.Add(new CodeNamespaceImport("Microsoft.EntityFrameworkCore"));
            return nameSpace;
        }
    }
}