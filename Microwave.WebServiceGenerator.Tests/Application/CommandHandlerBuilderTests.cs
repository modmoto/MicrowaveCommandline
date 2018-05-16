using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microwave.WebServiceGenerator.Application;

namespace Microwave.WebServiceGenerator.Tests.Application
{
    [TestClass]
    public class CommandHandlerBuilderTests : TestBase
    {
        [TestMethod]
        public void Build()
        {
            var commandHandlerBuilder = new CommandHandlerBuilder(ApplicationNameSpace);

            foreach (var domainClass in DomainTree.Classes)
            {
                var codeNamespace = commandHandlerBuilder.Build(domainClass);
                TestUtils.SnapshotTest(codeNamespace);
            }
        }
    }
}