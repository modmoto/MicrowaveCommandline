using System.IO;
using DslModelToCSharp.SqlAdapter;
using FileToDslModel;
using FileToDslModel.Lexer;
using FileToDslModel.ParseAutomat;
using NUnit.Framework;

namespace DslModelToCSharp.Tests.SqlAdapter
{
    [TestFixture]
    public class DbContextBuilderTests : TestBase
    {
        [Test]
        public void Write()
        {
            var storeBuilder = new DbContextBuilder(SqlAdpaterNameSpace);

            using (var reader = new StreamReader("Schema.wsb"))
            {
                var content = reader.ReadToEnd();
                var domainTree = new DslParser(new Tokenizer(), new Parser()).Parse(content);

                var eventStore = storeBuilder.Build(domainTree.Classes);
                new FileWriter(SqlAdpaterNameSpace).WriteToFile(eventStore.Types[0].Name, "Base/", eventStore);
            }

            new PrivateSetPropertyHackCleaner().ReplaceHackPropertyNames(SqlAdpaterBasePath);

            Assert.AreEqual(File.ReadAllText("../../../SqlAdapterExpected/Generated/Base/EventStoreContext.g.cs"),
                File.ReadAllText("SqlAdapter/Base/EventStoreContext.g.cs"));
        }
    }
}