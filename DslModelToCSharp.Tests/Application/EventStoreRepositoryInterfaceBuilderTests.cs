using System.IO;
using DslModel.Application;
using DslModelToCSharp.Application;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DslModelToCSharp.Tests.Application
{
    [TestClass]
    public class EventStoreRepositoryInterfaceBuilderTests : TestBase
    {
        [TestMethod]
        public void Write()
        {
            var storeBuilder = new EventStoreRepositoryInterfaceBuilder(ApplicationNameSpace);

            var codeNamespace = storeBuilder.Build(new EventStoreRepositoryInterface());

            new FileWriter(ApplicationBasePath).WriteToFile(codeNamespace.Types[0].Name, "Base", codeNamespace);

            new PrivateSetPropertyHackCleaner().ReplaceHackPropertyNames(ApplicationBasePath);

            Assert.AreEqual(File.ReadAllText("../../../ApplicationExpected/Generated/Base/IEventStoreRepository.g.cs"),
                File.ReadAllText("Application/Base/IEventStoreRepository.g.cs"));
        }
    }
}