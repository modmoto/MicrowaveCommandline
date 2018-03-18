using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DslModelToCSharp.Tests
{
    public class TestBase
    {
        protected readonly string DomainNameSpace = "Domain";
        protected readonly string DomainBasePath = "Domain/";
        protected readonly string SolutionBasePath = "Solution/";
        protected readonly string ApplicationNameSpace = "Application";
        protected readonly string SqlAdpaterNameSpace = "SqlAdapter";
        protected readonly string SqlAdpaterBasePath = "SqlAdapter/";
        protected readonly string ApplicationBasePath = "Application/";
       
        [TestInitialize]
        public void Setup()
        {
            if (Directory.Exists("Application")) Directory.Delete("Application", true);
            if (Directory.Exists("Domain")) Directory.Delete("Domain", true);
        }
    }
}