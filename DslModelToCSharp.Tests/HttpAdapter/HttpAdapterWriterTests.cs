using System.IO;
using DslModelToCSharp.HttpAdapter;
using FileToDslModel;
using FileToDslModel.Lexer;
using FileToDslModel.ParseAutomat;
using NUnit.Framework;

namespace DslModelToCSharp.Tests.HttpAdapter
{
    [TestFixture]
    public class HttpAdapterWriterTests : TestBase
    {
        [Test]
        public void Write()
        {
            var storeBuilder = new HttpAdapterWriter(HttpAdpaterNameSpace, HttpAdpaterBasePath);
            
            using (var reader = new StreamReader("Schema.wsb"))
            {
                var content = reader.ReadToEnd();
                var domainTree = new DslParser(new Tokenizer(), new Parser()).Parse(content);

                storeBuilder.Write(domainTree);
            }

            Assert.IsTrue(File.Exists("HttpAdapter/Users/UserController.g.cs"));
            Assert.IsTrue(File.Exists("HttpAdapter/Posts/PostController.g.cs"));
        }
    }
}