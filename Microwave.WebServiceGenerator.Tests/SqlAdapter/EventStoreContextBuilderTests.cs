using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microwave.WebServiceGenerator.SqlAdapter;
using Microwave.WebServiceModel.SqlAdapter;

namespace Microwave.WebServiceGenerator.Tests.SqlAdapter
{
    [TestClass]
    public class DbContextBuilderTests : TestBase
    {
        [TestMethod]
        public void BuildDbContext()
        {
            var storeBuilder = new EventStoreContextBuilder(SqlAdpaterNameSpace);
            var eventStore = storeBuilder.Build(DomainTree.Classes);
            TestUtils.SnapshotTest(eventStore);
        }

        [TestMethod]
        public void BuildHangfireContext()
        {
            var storeBuilder = new HangfireContextBuilder(SqlAdpaterNameSpace);
            var eventStore = storeBuilder.Build(DomainTree.Classes);
            TestUtils.SnapshotTest(eventStore);
        }

        [TestMethod]
        public void BuildEventJobRegistration()
        {
            var storeBuilder = new EventJobRegistrationClassBuilder(SqlAdpaterNameSpace);
            var eventStore = storeBuilder.Build(DomainTree.AsyncDomainHooks);
            TestUtils.SnapshotTest(eventStore);
        }

        [TestMethod]
        public void BuildQueueRepositoryBuilder()
        {
            var storeBuilder = new QueueRepositoryBuilder(SqlAdpaterNameSpace);
            var eventStore = storeBuilder.Build(new QueueRepositoryClass());
            TestUtils.SnapshotTest(eventStore);
        }
    }
}