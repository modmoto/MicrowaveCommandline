using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microwave.WebServiceGenerator.SqlAdapter;
using Microwave.WebServiceModel.SqlAdapter;

namespace Microwave.WebServiceGenerator.Tests.SqlAdapter
{
    [TestClass]
    public class EventStoreRepositoryBuilderTests : TestBase
    {
        [TestMethod]
        public void BuildEventStoreRepository()
        {
            var classFactory = new ClassBuilderDirector();
            var buildInstance = classFactory.BuildInstance(new EventStoreRepositoryBuilder(new EventStoreRepository()));
            TestUtils.SnapshotTest(buildInstance);
        }
    }
}