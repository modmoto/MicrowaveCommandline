using System.IO;
using DslModel.Domain;
using FileToDslModel;
using FileToDslModel.Lexer;
using FileToDslModel.ParseAutomat;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DslModelToCSharp.Tests
{
    [TestClass]
    public class DomainClassWriterTests : TestBase
    {
        [TestMethod]
        public void TestAll_Snapshot()
        {
            var domainBuilder = new DomainWriter(DomainNameSpace, BasePathDomain, BasePathSolution);
            using (var reader = new StreamReader("Schema.wsb"))
            {
                var content = reader.ReadToEnd();
                var domainTree = new DslParser(new Tokenizer(), new Parser()).Parse(content);
                domainBuilder.Build(domainTree, DomainNameSpace, BasePathDomain);
            }

            Assert.AreEqual(File.ReadAllText("../../../DomainExpected/Generated/Users/UserCreateEvent.g.cs"),
                File.ReadAllText("Domain/Users/UserCreateEvent.g.cs"));
            Assert.AreEqual(File.ReadAllText("../../../DomainExpected/Generated/Users/User.g.cs"),
                File.ReadAllText("Domain/Users/User.g.cs"));
            Assert.AreEqual(File.ReadAllText("../../../DomainExpected/Generated/Users/UserUpdateAgeEvent.g.cs"),
                File.ReadAllText("Domain/Users/UserUpdateAgeEvent.g.cs"));
            Assert.AreEqual(File.ReadAllText("../../../DomainExpected/Generated/Users/UserUpdateNameEvent.g.cs"),
                File.ReadAllText("Domain/Users/UserUpdateNameEvent.g.cs"));
        }

        [TestMethod]
        public void CreateionResultBase_Builder()
        {
            new DomainClassWriter(DomainNameSpace, BasePathDomain, BasePathSolution).Write(new CreationResultBaseClass());
            new PrivateSetPropertyHackCleaner().ReplaceHackPropertyNames(BasePathDomain);

            Assert.AreEqual(File.ReadAllText("../../../DomainExpected/Generated/Base/CreationResult.g.cs"),
                File.ReadAllText("Domain/Base/CreationResult.g.cs"));
        }

        [TestMethod]
        public void DomainEventBaseClass_Builder()
        {
            new DomainEventBaseClassWriter(DomainNameSpace, BasePathDomain).Build(new DomainEventBaseClass().Name, new DomainEventBaseClass().Properties);
            new PrivateSetPropertyHackCleaner().ReplaceHackPropertyNames(BasePathDomain);

            Assert.AreEqual(File.ReadAllText("../../../DomainExpected/Generated/Base/DomainEventBase.g.cs"),
                File.ReadAllText("Domain/Base/DomainEventBase.g.cs"));
        }
    }
}