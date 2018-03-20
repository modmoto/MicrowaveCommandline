using System.IO;
using DslModelToCSharp.Application;
using FileToDslModel;
using FileToDslModel.Lexer;
using FileToDslModel.ParseAutomat;
using NUnit.Framework;

namespace DslModelToCSharp.Tests.Application
{
    [TestFixture]
    public class ApplicationWriterTests : TestBase
    {
        [Test]
        public void Write()
        {
            var storeBuilder = new ApplicationWriter(ApplicationNameSpace, ApplicationBasePath, SolutionBasePath);
            
            using (var reader = new StreamReader("Schema.wsb"))
            {
                var content = reader.ReadToEnd();
                var domainTree = new DslParser(new Tokenizer(), new Parser()).Parse(content);

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