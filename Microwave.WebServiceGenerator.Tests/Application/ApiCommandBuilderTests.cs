using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microwave.WebServiceGenerator.Application;

namespace Microwave.WebServiceGenerator.Tests.Application
{
    [TestClass]
    public class ApiCommandBuilderTests : TestBase
    {
        [TestMethod]
        public void Build()
        {
            var commandBuilder = new ApiCommandBuilder();

            foreach (var domainClass in DomainTree.Classes)
            {
                foreach (var method in domainClass.LoadMethods)
                {
                    var codeNamespace = commandBuilder.Build(method, domainClass);
                    TestUtils.SnapshotTest(codeNamespace);
                }
            }
        }
    }
}