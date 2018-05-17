using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microwave.WebServiceGenerator.Tests
{
    [TestClass]
    public class DependencyInjectionBuilderTests : TestBase
    {
        [TestMethod]
        public void BuildDiAsyncHost()
        {
            var hookResultBuilder = new DependencyInjectionBuilderAsyncHost();
            var codeNamespace = hookResultBuilder.Write(DomainTree.Classes, DomainTree.AsyncDomainHooks);
            TestUtils.SnapshotTest(codeNamespace);
        }

        [TestMethod]
        public void BuildDiHost()
        {
            var hookResultBuilder = new DependencyInjectionBuilderHost();
            var codeNamespace = hookResultBuilder.Build(DomainTree.Classes, DomainTree.SynchronousDomainHooks, DomainTree.OnChildHooks);
            TestUtils.SnapshotTest(codeNamespace);
        }
    }
}