using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microwave.WebServiceGenerator.Application;
using Microwave.WebServiceModel.Application;

namespace Microwave.WebServiceGenerator.Tests.Application
{
    [TestClass]
    public class EventStoreBuilderTests : TestBase
    {
        [TestMethod]
        public void Write()
        {
            var storeBuilder = new EventStoreBuilder(ApplicationNameSpace);

            var eventStore = storeBuilder.Build(new EventStore());
            TestUtils.SnapshotTest(eventStore);
        }
    }
}