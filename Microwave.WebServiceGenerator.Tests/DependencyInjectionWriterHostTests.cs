using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microwave.WebServiceGenerator.Tests
{
    [TestClass]
    public class DependencyInjectionWriterHostTests : TestBase
    {
        [TestMethod]
        public void Write()
        {
            var hookResultBuilder = new DependencyInjectionBuilderHost();
            var codeNamespace = hookResultBuilder.Build(DomainTree.Classes, DomainTree.SynchronousDomainHooks, DomainTree.OnChildHooks);
            TestUtils.SnapshotTest(codeNamespace);
        }
    }
}