using System.IO;
using DslModelToCSharp.SqlAdapter;
using FileToDslModel;
using FileToDslModel.Lexer;
using FileToDslModel.ParseAutomat;
using NUnit.Framework;

namespace DslModelToCSharp.Tests.SqlAdapter
{
    [TestFixture]
    public class SqlAdapterWriterTests : TestBase
    {
        [Test]
        public void Write()
        {
            var storeBuilder = new SqlAdapterWriter(SqlAdpaterNameSpace, SqlAdpaterBasePath);
            
            using (var reader = new StreamReader("Schema.wsb"))
            {
                var content = reader.ReadToEnd();
                var domainTree = new DslParser(new Tokenizer(), new Parser()).Parse(content);

                storeBuilder.Write(domainTree);
            }

            Assert.IsTrue(File.Exists("SqlAdapter/Users/UserRepository.g.cs"));
            Assert.IsTrue(File.Exists("SqlAdapter/Posts/PostRepository.g.cs"));
            Assert.IsTrue(File.Exists("SqlAdapter/Base/EventStoreContext.g.cs"));
            Assert.IsTrue(File.Exists("SqlAdapter/Base/EventStoreRepository.g.cs"));
        }
    }
}