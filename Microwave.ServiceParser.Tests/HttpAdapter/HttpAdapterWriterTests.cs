using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microwave.LanguageParser;
using Microwave.LanguageParser.Lexer;
using Microwave.LanguageParser.ParseAutomat;
using Microwave.ServiceParser.HttpAdapter;

namespace Microwave.ServiceParser.Tests.HttpAdapter
{
    [TestClass]
    public class HttpAdapterWriterTests : TestBase
    {
        [TestMethod]
        public void Write()
        {
            var storeBuilder = new HttpAdapterWriter(HttpAdpaterNameSpace, HttpAdpaterBasePath);
            
            using (var reader = new StreamReader("Schema.mic"))
            {
                var content = reader.ReadToEnd();
                var domainTree = new DslParser(new MicrowaveLanguageTokenizer(), new MicrowaveLanguageParser()).Parse(content);

                storeBuilder.Write(domainTree);
            }

            Assert.IsTrue(File.Exists("HttpAdapter/Users/UserController.g.cs"));
            Assert.IsTrue(File.Exists("HttpAdapter/Posts/PostController.g.cs"));
        }
    }
}