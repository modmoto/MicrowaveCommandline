using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microwave.WebServiceGenerator.Application;

namespace Microwave.WebServiceGenerator.Tests.Application
{
    [TestClass]
    public class SyncHookBuilderTests : TestBase
    {
        [TestMethod]
        public void BuildSynchronousHook()
        {
            var commandHandlerBuilder = new SynchronousHookBuilder(ApplicationNameSpace);

            foreach (var hook in DomainTree.SynchronousDomainHooks)
            {
                var codeNamespace = commandHandlerBuilder.Build(hook);
                TestUtils.SnapshotTest(codeNamespace);
            }
        }

        [TestMethod]
        public void BuildSynchronousHookReplacementClass()
        {
            var commandHandlerBuilder = new SynchronousHookBuilder(ApplicationNameSpace);

            foreach (var hook in DomainTree.SynchronousDomainHooks)
            {
                var codeNamespace = commandHandlerBuilder.BuildReplacementClass(hook);
                TestUtils.SnapshotTest(codeNamespace, false);
            }
        }

        [TestMethod]
        public void BuildOnChildHook()
        {
            var commandHandlerBuilder = new SynchronousHookBuilder(ApplicationNameSpace);

            foreach (var domainClass in DomainTree.Classes)
            {
                foreach (var hook in domainClass.ChildHookMethods)
                {
                    var codeNamespace = commandHandlerBuilder.BuildOnChildHook(hook, domainClass.Properties, domainClass.ListProperties);
                    TestUtils.SnapshotTest(codeNamespace, false);
                }
            }
        }
    }
}