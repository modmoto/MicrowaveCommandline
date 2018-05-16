using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microwave.WebServiceGenerator.Application;

namespace Microwave.WebServiceGenerator.Tests.Application
{
    [TestClass]
    public class AsyncHookBuilderTests : TestBase
    {
        [TestMethod]
        public void BuildTests()
        {
            foreach (var hook in DomainTree.AsyncDomainHooks)
            {
                var codeNamespace = new ClassBuilderDirector().BuildInstance(new AsyncHookBuilder(hook));
                TestUtils.SnapshotTest(codeNamespace);
            }
        }
    }
}