using System.IO;
using DslModel.Application;
using DslModelToCSharp.Application;
using NUnit.Framework;

namespace DslModelToCSharp.Tests.Application
{
    [TestFixture]
    public class HookBaseClassBuilderTests : TestBase
    {
        [Test]
        public void Write()
        {
            var hookResultBuilder = new HookBaseClassBuilder(ApplicationNameSpace);

            var codeNamespace = hookResultBuilder.Build(new DomainHookBaseClass());

            new FileWriter(ApplicationBasePath).WriteToFile(codeNamespace.Types[0].Name, "Base", codeNamespace);

            new PrivateSetPropertyHackCleaner().ReplaceHackPropertyNames(ApplicationBasePath);

            Assert.AreEqual(File.ReadAllText("../../../ApplicationExpected/Generated/Base/IDomainHook.g.cs"),
                File.ReadAllText("Application/Base/IDomainHook.g.cs"));
        }
    }
}