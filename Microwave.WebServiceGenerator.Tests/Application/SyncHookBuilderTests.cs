using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microwave.WebServiceGenerator.Application;

namespace Microwave.WebServiceGenerator.Tests.Application
{
    [TestClass]
    public class SyncHookBuilderTests : TestBase
    {
        [TestMethod]
        public void BuildTests()
        {
            var commandHandlerBuilder = new SynchronousHookBuilder(ApplicationNameSpace);

            foreach (var hook in DomainTree.SynchronousDomainHooks)
            {
                var codeNamespace = commandHandlerBuilder.Build(hook);
                TestUtils.SnapshotTest(codeNamespace);
            }
        }

        [TestMethod]
        public void BuildReplacementClass()
        {
            var commandHandlerBuilder = new SynchronousHookBuilder(ApplicationNameSpace);

            foreach (var hook in DomainTree.SynchronousDomainHooks)
            {
                var codeNamespace = commandHandlerBuilder.BuildReplacementClass(hook);
                TestUtils.SnapshotTest(codeNamespace, false);
            }
        }
    }
}