using System.IO;
using DslModel.Application;
using DslModelToCSharp.Application;
using FileToDslModel;
using FileToDslModel.Lexer;
using FileToDslModel.ParseAutomat;
using NUnit.Framework;

namespace DslModelToCSharp.Tests.Application
{
    [TestFixture]
    public class EventStoreBuilderTests : TestBase
    {
        [Test]
        public void Write()
        {
            var storeBuilder = new EventStoreBuilder(ApplicationNameSpace);

            using (var reader = new StreamReader("Schema.wsb"))
            {
                var content = reader.ReadToEnd();
                var domainTree = new DslParser(new Tokenizer(), new Parser()).Parse(content);

                var eventStore = storeBuilder.Build(new EventStore(), domainTree.SynchronousDomainHooks);
                new FileWriter(ApplicationBasePath).WriteToFile(eventStore.Types[0].Name, "Base/", eventStore);
            }

            new PrivateSetPropertyHackCleaner().ReplaceHackPropertyNames(ApplicationBasePath);

            Assert.AreEqual(File.ReadAllText("../../../ApplicationExpected/Generated/Base/EventStore.g.cs").Replace("\\n", "\\r\\n"),
                File.ReadAllText("Application/Base/EventStore.g.cs"));
        }
    }
}