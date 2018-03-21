using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DslModelToCSharp.Tests
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
       
        [TestInitialize]
        public void Setup()
        {
            if (Directory.Exists("HttpAdapter")) Directory.Delete("HttpAdapter", true);
            if (Directory.Exists("SqlAdapter")) Directory.Delete("SqlAdapter", true);
            if (Directory.Exists("Application")) Directory.Delete("Application", true);
            if (Directory.Exists("Domain")) Directory.Delete("Domain", true);
            if (Directory.Exists("Solution")) Directory.Delete("Solution", true);
        }
    }
}