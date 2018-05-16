using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microwave.WebServiceGenerator.Application;
using Microwave.WebServiceModel.Application;

namespace Microwave.WebServiceGenerator.Tests.Application
{
    [TestClass]
    public class EventStoreRepositoryInterfaceBuilderTests : TestBase
    {
        [TestMethod]
        public void Write()
        {
            var storeBuilder = new EventStoreRepositoryInterfaceBuilder(ApplicationNameSpace);

            var codeNamespace = storeBuilder.Build(new EventStoreRepositoryInterface());
            TestUtils.SnapshotTest(codeNamespace);
        }
    }
}