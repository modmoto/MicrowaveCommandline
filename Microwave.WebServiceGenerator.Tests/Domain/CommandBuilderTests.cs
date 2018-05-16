using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microwave.WebServiceGenerator.Domain;

namespace Microwave.WebServiceGenerator.Tests.Domain
{
    [TestClass]
    public class CommandBuilderTests : TestBase
    {
        [TestMethod]
        public void Build()
        {
            var commandBuilder = new CommandBuilder();

            foreach (var domainClass in DomainTree.Classes)
            {
                var codeNamespaces = commandBuilder.Build(domainClass);

                foreach (var codeNamespace in codeNamespaces)
                {
                    TestUtils.SnapshotTest(codeNamespace);
                }
            }
        }
    }
}