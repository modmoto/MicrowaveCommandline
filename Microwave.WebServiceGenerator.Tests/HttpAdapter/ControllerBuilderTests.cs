using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microwave.WebServiceGenerator.HttpAdapter;

namespace Microwave.WebServiceGenerator.Tests.HttpAdapter
{
    [TestClass]
    public class ControllerBuilderTests : TestBase
    {
        [TestMethod]
        public void BuildController()
        {
            var controllerBuilder = new ControllerBuilder(HttpAdpaterNameSpace);

            foreach (var domainTreeClass in DomainTree.Classes)
            {
                var controller = controllerBuilder.Build(domainTreeClass);
                TestUtils.SnapshotTest(controller);
            }
        }
    }
}