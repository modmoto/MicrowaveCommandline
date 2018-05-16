using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microwave.WebServiceGenerator.Application;
using Microwave.WebServiceModel.Application;

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

        public void BuildCommandHandlers()
        {
            foreach (var hook in DomainTree.AsyncDomainHooks)
            {
                var asyncHookCreateEventHandlerBuilder = new AsyncHookCreateEventHandlerBuilder(ApplicationNameSpace);
                var codeNamespace = asyncHookCreateEventHandlerBuilder.Build(hook);
                TestUtils.SnapshotTest(codeNamespace);
            }
        }

        [TestMethod]
        public void BuildEventAndJob()
        {
            var asyncHookCreateEventHandlerBuilder = new EventAndJobClassBuilder(ApplicationNameSpace);
            var codeNamespace = asyncHookCreateEventHandlerBuilder.Build(new EventAndJobClass());
            TestUtils.SnapshotTest(codeNamespace);
        }

        [TestMethod]
        public void BuildHangfireQueueInterface()
        {
            var asyncHookCreateEventHandlerBuilder = new HangfireQueueInterfaceBuilder(ApplicationNameSpace);
            var codeNamespace = asyncHookCreateEventHandlerBuilder.Build(new HangfireQueueInterface());
            TestUtils.SnapshotTest(codeNamespace);
        }

        [TestMethod]
        public void BuildQueueRepositoryInterface()
        {
            var asyncHookCreateEventHandlerBuilder = new QueueRepositoryInterfaceBuilder(ApplicationNameSpace);
            var codeNamespace = asyncHookCreateEventHandlerBuilder.Build(new QueueRepositoryInterface());
            TestUtils.SnapshotTest(codeNamespace);
        }

        [TestMethod]
        public void BuildEventStoreInterface()
        {
            var asyncHookCreateEventHandlerBuilder = new EventStoreInterfaceBuilder(ApplicationNameSpace);
            var codeNamespace = asyncHookCreateEventHandlerBuilder.Build(new EventStoreInterface());
            TestUtils.SnapshotTest(codeNamespace);
        }
    }
}