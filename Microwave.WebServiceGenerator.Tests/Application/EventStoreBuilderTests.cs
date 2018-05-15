using System;
using System.IO;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microwave.WebServiceGenerator.Application;
using Microwave.WebServiceModel.Application;

namespace Microwave.WebServiceGenerator.Tests.Application
{
    [TestClass]
    public class EventStoreBuilderTests : TestBase
    {
        [TestMethod]
        public void Write()
        {
            var storeBuilder = new EventStoreBuilder(ApplicationNameSpace);

            var eventStore = storeBuilder.Build(new EventStore());
            new FileWriter(ApplicationBasePath).WriteToFile("Base/", eventStore);

            new PrivateSetPropertyHackCleaner().ReplaceHackPropertyNames(ApplicationBasePath);

            Assert.AreEqual(Regex.Replace(File.ReadAllText("../../../ApplicationExpected/Generated/Base/EventStore.g.cs"), @"\s+", String.Empty),
                Regex.Replace(File.ReadAllText("Application/Base/EventStore.g.cs"),  @"\s+", String.Empty));
        }
    }
}