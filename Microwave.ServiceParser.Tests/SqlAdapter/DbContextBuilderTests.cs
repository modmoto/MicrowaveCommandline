using System;
using System.IO;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microwave.LanguageParser;
using Microwave.LanguageParser.Lexer;
using Microwave.LanguageParser.ParseAutomat;
using Microwave.ServiceParser.SqlAdapter;

namespace Microwave.ServiceParser.Tests.SqlAdapter
{
    [TestClass]
    public class DbContextBuilderTests : TestBase
    {
        [TestMethod]
        public void Write()
        {
            var storeBuilder = new DbContextBuilder(SqlAdpaterNameSpace);

            using (var reader = new StreamReader("Schema.mic"))
            {
                var content = reader.ReadToEnd();
                var domainTree = new DslParser(new MicrowaveLanguageTokenizer(), new MicrowaveLanguageParser()).Parse(content);

                var eventStore = storeBuilder.Build(domainTree.Classes);
                new FileWriter(SqlAdpaterNameSpace).WriteToFile(eventStore.Types[0].Name, "Base/", eventStore);
            }

            new PrivateSetPropertyHackCleaner().ReplaceHackPropertyNames(SqlAdpaterBasePath);

            Assert.AreEqual(Regex.Replace(File.ReadAllText("../../../SqlAdapterExpected/Generated/Base/EventStoreContext.g.cs"), @"\s+", String.Empty),
                Regex.Replace(File.ReadAllText("SqlAdapter/Base/EventStoreContext.g.cs"), @"\s+", String.Empty));
        }
    }
}