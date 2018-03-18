using System.IO;
using DslModel.Application;
using DslModelToCSharp.Application;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DslModelToCSharp.Tests.Application
{
    [TestClass]
    public class HookResultBuilderTests : TestBase
    {
        [TestMethod]
        public void Write()
        {
            var hookResultBuilder = new HookResultBuilder(ApplicationNameSpace);

            var codeNamespace = hookResultBuilder.Write(new HookResultBaseClass());

            new FileWriter(ApplicationBasePath).WriteToFile(codeNamespace.Types[0].Name, "Base", codeNamespace);

            new PrivateSetPropertyHackCleaner().ReplaceHackPropertyNames(ApplicationBasePath);

            Assert.AreEqual(File.ReadAllText("../../../ApplicationExpected/Generated/Base/HookResult.g.cs"),
                File.ReadAllText("Application/Base/HookResult.g.cs"));
        }
    }
}