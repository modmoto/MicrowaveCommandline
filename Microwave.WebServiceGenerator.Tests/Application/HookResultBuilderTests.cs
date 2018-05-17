using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microwave.WebServiceGenerator.Application;
using Microwave.WebServiceModel.Application;

namespace Microwave.WebServiceGenerator.Tests.Application
{
    [TestClass]
    public class HookResultBuilderTests : TestBase
    {
        [TestMethod]
        public void BuildHookResult()
        {
            var hookResultBuilder = new HookResultBuilder(ApplicationNameSpace);
            var codeNamespace = hookResultBuilder.Build(new HookResultBaseClass());
            TestUtils.SnapshotTest(codeNamespace);
        }
    }
}