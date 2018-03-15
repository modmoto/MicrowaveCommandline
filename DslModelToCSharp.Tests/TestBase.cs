using System.IO;
using FileToDslModel;
using FileToDslModel.Lexer;
using FileToDslModel.ParseAutomat;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DslModelToCSharp.Tests
{
    public class TestBase
    {
        protected readonly string DomainNameSpace = "Domain";
        protected readonly string BasePathDomain = "Domain/";
        protected readonly string BasePathSolution = "Solution/";
        protected readonly string ApplicationNameSpace = "Application";
        protected readonly string BasePathApplication = "Application/";
       
        [TestInitialize]
        public void Setup()
        {
            if (Directory.Exists("Application")) Directory.Delete("Application", true);
            if (Directory.Exists("Domain")) Directory.Delete("Domain", true);
        }
    }
}