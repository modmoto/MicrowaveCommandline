using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microwave.LanguageParser;
using Microwave.LanguageParser.Lexer;
using Microwave.LanguageParser.ParseAutomat;
using Microwave.ServiceParser.SqlAdapter;

namespace Microwave.ServiceParser.Tests.SqlAdapter
{
    [TestClass]
    public class SqlAdapterWriterTests : TestBase
    {
        [TestMethod]
        public void Write()
        {
            var storeBuilder = new SqlAdapterWriter(SqlAdpaterNameSpace, SqlAdpaterBasePath);
            
            using (var reader = new StreamReader("Schema.mic"))
            {
                var content = reader.ReadToEnd();
                var domainTree = new DslParser(new MicrowaveLanguageTokenizer(), new MicrowaveLanguageParser()).Parse(content);

                storeBuilder.Write(domainTree);
            }

            Assert.IsTrue(File.Exists("SqlAdapter/Users/UserRepository.g.cs"));
            Assert.IsTrue(File.Exists("SqlAdapter/Posts/PostRepository.g.cs"));
            Assert.IsTrue(File.Exists("SqlAdapter/Base/EventStoreContext.g.cs"));
            Assert.IsTrue(File.Exists("SqlAdapter/Base/EventStoreRepository.g.cs"));
        }
    }
}