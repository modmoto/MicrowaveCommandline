using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microwave.WebServiceGenerator.Application;

namespace Microwave.WebServiceGenerator.Tests.Application
{
    [TestClass]
    public class RepositoryInterfaceBuilderTests : TestBase
    {
        [TestMethod]
        public void BuildRepositoryInterface()
        {
            var builder = new RepositoryInterfaceBuilder(ApplicationNameSpace);

            foreach (var domainClass in DomainTree.Classes)
            {
                var codeNamespace = builder.Build(domainClass);
                TestUtils.SnapshotTest(codeNamespace);
            }
        }
    }
}