using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microwave.LanguageParser;
using Microwave.LanguageParser.Lexer;
using Microwave.LanguageParser.ParseAutomat;
using Microwave.WebServiceGenerator.Application;

namespace Microwave.WebServiceGenerator.Tests.Application
{
    [TestClass]
    public class ApplicationWriterTests : TestBase
    {
        [TestMethod]
        public void Write()
        {
            var storeBuilder = new ApplicationWriter(ApplicationNameSpace, ApplicationBasePath, SolutionBasePath);
            
            using (var reader = new StreamReader("Schema.mic"))
            {
                var content = reader.ReadToEnd();
                var domainTree = new DslParser(new MicrowaveLanguageTokenizer(), new MicrowaveLanguageParser()).Parse(content);

                storeBuilder.Write(domainTree);
            }

            Assert.IsTrue(File.Exists("Application/Base/EventStore.g.cs"));
            Assert.IsTrue(File.Exists("Application/Base/HookResult.g.cs"));
            Assert.IsTrue(File.Exists("Application/Base/IDomainHook.g.cs"));
            Assert.IsTrue(File.Exists("Application/Base/IEventStoreRepository.g.cs"));
            Assert.IsTrue(File.Exists("Application/Posts/PostCommandHandler.g.cs"));
            Assert.IsTrue(File.Exists("Application/Posts/IPostRepository.g.cs"));
            Assert.IsTrue(File.Exists("Application/Users/IUserRepository.g.cs"));
            Assert.IsTrue(File.Exists("Application/Users/UserCommandHandler.g.cs"));
            Assert.IsTrue(File.Exists("Application/Users/Hooks/SendPasswordMailHook.g.cs"));

            Assert.IsTrue(File.Exists("Solution/Users/SendPasswordMailHook.cs"));
        }
    }
}