using System.IO;
using System.Text;
using DslModel.Domain;
using DslModelToCSharp.Domain;
using FileToDslModel;
using FileToDslModel.Lexer;
using FileToDslModel.ParseAutomat;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DslModelToCSharp.Tests.Domain
{
    [TestClass]
    public class DomainClassWriterTests : TestBase
    {
        [TestMethod]
        public void TestDomainClasses()
        {
            var domainBuilder = new DomainWriter(DomainNameSpace, DomainBasePath, SolutionBasePath);
            using (var reader = new StreamReader("Schema.wsb"))
            {
                var content = reader.ReadToEnd();
                var domainTree = new DslParser(new Tokenizer(), new Parser()).Parse(content);
                domainBuilder.Build(domainTree, DomainBasePath);
            }

            Assert.AreEqual(File.ReadAllText("../../../DomainExpected/Generated/Users/User.g.cs"),
                File.ReadAllText("Domain/Users/User.g.cs"));
        }

        [TestMethod]
        public void TestCreateEvents()
        {
            var domainBuilder = new DomainWriter(DomainNameSpace, DomainBasePath, SolutionBasePath);
            using (var reader = new StreamReader("Schema.wsb"))
            {
                var content = reader.ReadToEnd();
                var domainTree = new DslParser(new Tokenizer(), new Parser()).Parse(content);
                domainBuilder.Build(domainTree, DomainBasePath);
            }

            Assert.AreEqual(File.ReadAllText("../../../DomainExpected/Generated/Users/UserCreateEvent.g.cs"),
                File.ReadAllText("Domain/Users/UserCreateEvent.g.cs"));
        }

        [TestMethod]
        public void TestUpdateEvents()
        {
            var domainBuilder = new DomainWriter(DomainNameSpace, DomainBasePath, SolutionBasePath);
            using (var reader = new StreamReader("Schema.wsb"))
            {
                var content = reader.ReadToEnd();
                var domainTree = new DslParser(new Tokenizer(), new Parser()).Parse(content);
                domainBuilder.Build(domainTree, DomainBasePath);
            }

            Assert.AreEqual(File.ReadAllText("../../../DomainExpected/Generated/Users/UserUpdateAgeEvent.g.cs", Encoding.UTF8),
                File.ReadAllText("Domain/Users/UserUpdateAgeEvent.g.cs", Encoding.UTF8));
            Assert.AreEqual(File.ReadAllText("../../../DomainExpected/Generated/Users/UserUpdateNameEvent.g.cs", Encoding.UTF8),
                File.ReadAllText("Domain/Users/UserUpdateNameEvent.g.cs", Encoding.UTF8));
        }

        [TestMethod]
        public void CreationResultBase_Builder()
        {
            new DomainClassWriter(DomainNameSpace, DomainBasePath, SolutionBasePath).Write(new CreationResultBaseClass());
            new PrivateSetPropertyHackCleaner().ReplaceHackPropertyNames(DomainBasePath);

            Assert.AreEqual(File.ReadAllText("../../../DomainExpected/Generated/Base/CreationResult.g.cs", Encoding.UTF8),
                File.ReadAllText("Domain/Base/CreationResult.g.cs", Encoding.UTF8));
        }

        [TestMethod]
        public void DomainEventBaseClass_Builder()
        {
            var classFactory = new ClassFactory();
            var baseClass = classFactory.BuildInstance(new DomainEventBaseClassBuilder());
            new FileWriter(DomainBasePath).WriteToFile("DomainEventBase", "Base", baseClass);

            new PrivateSetPropertyHackCleaner().ReplaceHackPropertyNames(DomainBasePath);

            Assert.AreEqual(File.ReadAllText("../../../DomainExpected/Generated/Base/DomainEventBase.g.cs", Encoding.UTF8),
                File.ReadAllText("Domain/Base/DomainEventBase.g.cs", Encoding.UTF8));
        }
    }
}