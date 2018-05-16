using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microwave.LanguageModel;
using Microwave.LanguageParser;
using Microwave.LanguageParser.Lexer;
using Microwave.LanguageParser.ParseAutomat;

namespace Microwave.WebServiceGenerator.Tests
{
    [TestClass]
    public class TestBase
    {
        protected readonly string SolutionBasePath = "Solution/";
        protected readonly string DomainNameSpace = "Domain";
        protected readonly string DomainBasePath = "Domain/";
        protected readonly string ApplicationNameSpace = "Application";
        protected readonly string ApplicationBasePath = "Application/";
        protected readonly string SqlAdpaterNameSpace = "SqlAdapter";
        protected readonly string SqlAdpaterBasePath = "SqlAdapter/";
        protected readonly string HttpAdpaterNameSpace = "HttpAdapter";
        protected readonly string HttpAdpaterBasePath = "HttpAdapter/";
        protected DomainTree DomainTree;

        [TestInitialize]
        public void Setup()
        {
            if (Directory.Exists("Generated")) Directory.Delete("Generated", true);
            if (Directory.Exists("HttpAdapter")) Directory.Delete("HttpAdapter", true);
            if (Directory.Exists("SqlAdapter")) Directory.Delete("SqlAdapter", true);
            if (Directory.Exists("Application")) Directory.Delete("Application", true);
            if (Directory.Exists("Domain")) Directory.Delete("Domain", true);
            if (Directory.Exists("Solution")) Directory.Delete("Solution", true);

            using (var reader = new StreamReader("Schema.mic"))
            {
                var content = reader.ReadToEnd();
                DomainTree = new DslParser(new MicrowaveLanguageTokenizer(), new MicrowaveLanguageParser()).Parse(content);
            }
        }
    }
}