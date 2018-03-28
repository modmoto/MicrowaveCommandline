using System.CodeDom;
using System.Collections.Generic;

namespace Microwave.WebServiceGenerator.Util
{
    public class NameSpaceBuilderUtil
    {
        private CodeNamespace _nameSpace;

        public List<CodeNamespaceImport> Imports { get; private set; }

        public NameSpaceBuilderUtil WithList()
        {
            Imports.Add(new CodeNamespaceImport("System.Collections.Generic"));
            return this;
        }

        public NameSpaceBuilderUtil WithTask()
        {
            Imports.Add(new CodeNamespaceImport("System.Threading.Tasks"));
            return this;
        }

        public NameSpaceBuilderUtil WithDomain()
        {
            Imports.Add(new CodeNamespaceImport("Domain"));
            return this;
        }

        public NameSpaceBuilderUtil WithApplication()
        {
            Imports.Add(new CodeNamespaceImport("Application"));
            return this;
        }

        public NameSpaceBuilderUtil WithSqlAdapter()
        {
            Imports.Add(new CodeNamespaceImport("SqlAdapter"));
            return this;
        }

        public NameSpaceBuilderUtil WithDependencyInjection()
        {
            Imports.Add(new CodeNamespaceImport("Microsoft.Extensions.DependencyInjection"));
            return this;
        }

        public NameSpaceBuilderUtil WithMvcImport()
        {
            Imports.Add(new CodeNamespaceImport("Microsoft.AspNetCore.Mvc"));
            return this;
        }

        public NameSpaceBuilderUtil WithName(string domain)
        {
            _nameSpace = new CodeNamespace(domain);
            Imports = new List<CodeNamespaceImport>();
            Imports.Add(new CodeNamespaceImport("System"));

            return this;
        }

        public NameSpaceBuilderUtil WithApplicationEntityNameSpace(string domainClassName)
        {
            Imports.Add(new CodeNamespaceImport($"Application.{domainClassName}s"));
            return this;
        }

        public NameSpaceBuilderUtil WithHttpAdapterEntityNameSpace(string domainClassName)
        {
            Imports.Add(new CodeNamespaceImport($"HttpAdapter.{domainClassName}s"));
            return this;
        }

        public NameSpaceBuilderUtil WithDomainEntityNameSpace(string domainClassName)
        {
            Imports.Add(new CodeNamespaceImport($"Domain.{domainClassName}s"));
            return this;
        }

        public NameSpaceBuilderUtil WithSqlEntityNameSpace(string domainClassName)
        {
            Imports.Add(new CodeNamespaceImport($"SqlAdapter.{domainClassName}s"));
            return this;
        }

        public NameSpaceBuilderUtil WithHookEntityNameSpace(string hookClassType)
        {
            Imports.Add(new CodeNamespaceImport($"Application.{hookClassType}s.Hooks"));
            return this;
        }

        public NameSpaceBuilderUtil WithEfCore()
        {
            Imports.Add(new CodeNamespaceImport("Microsoft.EntityFrameworkCore"));
            return this;
        }

        public NameSpaceBuilderUtil WithLinq()
        {
            Imports.Add(new CodeNamespaceImport("System.Linq"));
            return this;
        }

        public CodeNamespace Build()
        {
            foreach (var namespaceImport in Imports)
            {
                _nameSpace.Imports.Add(namespaceImport);
            }
            return _nameSpace;
        }

        public NameSpaceBuilderUtil WithRepository(string paramType)
        {
            Imports.Add(new CodeNamespaceImport($"Application.{paramType}s"));
            return this;
        }

        public NameSpaceBuilderUtil WithStopWatch()
        {
            Imports.Add(new CodeNamespaceImport("System.Diagnostics"));
            return this;
        }
    }
}