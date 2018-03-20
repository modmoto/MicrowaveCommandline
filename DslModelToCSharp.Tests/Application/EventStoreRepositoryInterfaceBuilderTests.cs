using System;
using System.IO;
using System.Text.RegularExpressions;
using DslModel.Application;
using DslModelToCSharp.Application;
using NUnit.Framework;

namespace DslModelToCSharp.Tests.Application
{
    [TestFixture]
    public class EventStoreRepositoryInterfaceBuilderTests : TestBase
    {
        [Test]
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