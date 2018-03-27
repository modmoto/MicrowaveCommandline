using System;
using System.IO;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microwave.LanguageModel.Application;
using Microwave.WebServiceGenerator.Application;

namespace Microwave.WebServiceGenerator.Tests.Application
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

            Assert.AreEqual(Regex.Replace(File.ReadAllText("../../../ApplicationExpected/Generated/Base/IEventStoreRepository.g.cs"), @"\s+", String.Empty),
                Regex.Replace(File.ReadAllText("Application/Base/IEventStoreRepository.g.cs"), @"\s+", String.Empty));
        }
    }
}