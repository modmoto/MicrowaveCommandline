using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microwave.WebServiceGenerator.Application;
using Microwave.WebServiceModel.Application;

namespace Microwave.WebServiceGenerator.Tests.Application
{
    [TestClass]
    public class HookBaseClassBuilderTests : TestBase
    {
        [TestMethod]
        public void Write()
        {
            var hookResultBuilder = new HookBaseClassBuilder(ApplicationNameSpace);
            var codeNamespace = hookResultBuilder.Build(new DomainHookBaseClass());
            TestUtils.SnapshotTest(codeNamespace);
        }
    }
}