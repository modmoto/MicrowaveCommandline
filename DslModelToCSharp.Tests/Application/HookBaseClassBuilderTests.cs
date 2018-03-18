using System.IO;
using DslModel.Application;
using DslModelToCSharp.Application;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DslModelToCSharp.Tests.Application
{
    [TestClass]
    public class HookBaseClassBuilderTests : TestBase
    {
        [TestMethod]
        public void Write()
        {
            var hookResultBuilder = new HookBaseClassBuilder(ApplicationNameSpace);

            var codeNamespace = hookResultBuilder.Build(new DomainHookBaseClass());

            new FileWriter(BasePathApplication).WriteToFile(codeNamespace.Types[0].Name, "Base", codeNamespace);

            new PrivateSetPropertyHackCleaner().ReplaceHackPropertyNames(BasePathApplication);

            Assert.AreEqual(File.ReadAllText("../../../ApplicationExpected/Generated/Base/IDomainHook.g.cs"),
                File.ReadAllText("Application/Base/IDomainHook.g.cs"));
        }
    }
}