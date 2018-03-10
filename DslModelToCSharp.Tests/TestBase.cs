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
        protected readonly string ApplicationNameSpace = "Application";
        protected readonly string BasePathApplication = "Application/";
       
        [TestInitialize]
        public void Setup()
        {
            if (Directory.Exists("DomainActual")) Directory.Delete("DomainActual", true);
        }
    }
}