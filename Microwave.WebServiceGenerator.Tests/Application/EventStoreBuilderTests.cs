using System;
using System.IO;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microwave.LanguageModel.Application;
using Microwave.LanguageParser;
using Microwave.LanguageParser.Lexer;
using Microwave.LanguageParser.ParseAutomat;
using Microwave.WebServiceGenerator.Application;

namespace Microwave.WebServiceGenerator.Tests.Application
{
    [TestClass]
    public class EventStoreBuilderTests : TestBase
    {
        [TestMethod]
        public void Write()
        {
            var storeBuilder = new EventStoreBuilder(ApplicationNameSpace);

            using (var reader = new StreamReader("Schema.mic"))
            {
                var content = reader.ReadToEnd();
                var domainTree = new DslParser(new MicrowaveLanguageTokenizer(), new MicrowaveLanguageParser()).Parse(content);

                var eventStore = storeBuilder.Build(new EventStore(), domainTree.SynchronousDomainHooks);
                new FileWriter(ApplicationBasePath).WriteToFile(eventStore.Types[0].Name, "Base/", eventStore);
            }

            new PrivateSetPropertyHackCleaner().ReplaceHackPropertyNames(ApplicationBasePath);

            Assert.AreEqual(Regex.Replace(File.ReadAllText("../../../ApplicationExpected/Generated/Base/EventStore.g.cs"), @"\s+", String.Empty),
                Regex.Replace(File.ReadAllText("Application/Base/EventStore.g.cs"),  @"\s+", String.Empty));
        }
    }
}