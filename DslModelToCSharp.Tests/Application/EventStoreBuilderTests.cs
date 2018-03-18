using System.IO;
using DslModel.Application;
using DslModelToCSharp.Application;
using FileToDslModel;
using FileToDslModel.Lexer;
using FileToDslModel.ParseAutomat;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DslModelToCSharp.Tests.Application
{
    [TestClass]
    public class EventStoreBuilderTests : TestBase
    {
        [TestMethod]
        public void Write()
        {
            var storeBuilder = new EventStoreBuilder(ApplicationNameSpace);

            using (var reader = new StreamReader("Schema.wsb"))
            {
                var content = reader.ReadToEnd();
                var domainTree = new DslParser(new Tokenizer(), new Parser()).Parse(content);

                var eventStore = storeBuilder.Build(new EventStore(), domainTree.SynchronousDomainHooks);
                new FileWriter(BasePathApplication).WriteToFile(eventStore.Types[0].Name, "Base/", eventStore);
            }

            new PrivateSetPropertyHackCleaner().ReplaceHackPropertyNames(BasePathApplication);

            Assert.AreEqual(File.ReadAllText("../../../ApplicationExpected/Generated/Base/EventStore.g.cs"),
                File.ReadAllText("Application/Base/EventStore.g.cs"));
        }
    }
}