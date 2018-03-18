using System.IO;
using DslModelToCSharp.HttpAdapter;
using FileToDslModel;
using FileToDslModel.Lexer;
using FileToDslModel.ParseAutomat;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DslModelToCSharp.Tests.HttpAdapter
{
    [TestClass]
    public class HttpAdapterWriterTests : TestBase
    {
        [TestMethod]
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