using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microwave.WebServiceGenerator.SqlAdapter;

namespace Microwave.WebServiceGenerator.Tests.SqlAdapter
{
    [TestClass]
    public class RepositoryBuilderTests : TestBase
    {
        [TestMethod]
        public void Write()
        {
            var storeBuilder = new RepositoryBuilder(SqlAdpaterNameSpace);

            foreach (var domainTreeClass in DomainTree.Classes)
            {
                var eventStore = storeBuilder.Build(domainTreeClass);
                TestUtils.SnapshotTest(eventStore);
            }
        }
    }
}