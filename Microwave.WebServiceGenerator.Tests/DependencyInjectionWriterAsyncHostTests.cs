using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microwave.WebServiceGenerator.Tests
{
    [TestClass]
    public class DependencyInjectionWriterAsyncHostTests : TestBase
    {
        [TestMethod]
        public void Write()
        {
            var hookResultBuilder = new DependencyInjectionWriterAsyncHost();
            var codeNamespace = hookResultBuilder.Write(DomainTree.Classes, DomainTree.AsyncDomainHooks);
            TestUtils.SnapshotTest(codeNamespace);
        }
    }
}