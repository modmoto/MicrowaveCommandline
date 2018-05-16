using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microwave.WebServiceGenerator.SqlAdapter;
using Microwave.WebServiceModel.SqlAdapter;

namespace Microwave.WebServiceGenerator.Tests.SqlAdapter
{
    [TestClass]
    public class HangfireQueueBuilderTests : TestBase
    {
        [TestMethod]
        public void Build()
        {
            var hangfireQueue = new HangfireQueueBuilder(SqlAdpaterNameSpace).Build(new HangfireQueueClass());
            TestUtils.SnapshotTest(hangfireQueue);
        }
    }
}