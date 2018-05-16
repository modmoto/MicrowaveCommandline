using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microwave.WebServiceGenerator.Application;
using Microwave.WebServiceModel.Application;

namespace Microwave.WebServiceGenerator.Tests.Application
{
    [TestClass]
    public class HookResultBuilderTests : TestBase
    {
        [TestMethod]
        public void Write()
        {
            var hookResultBuilder = new HookResultBuilder(ApplicationNameSpace);
            var codeNamespace = hookResultBuilder.Write(new HookResultBaseClass());
            TestUtils.SnapshotTest(codeNamespace);
        }
    }
}