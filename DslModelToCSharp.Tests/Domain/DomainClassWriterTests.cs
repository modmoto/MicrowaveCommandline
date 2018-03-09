using System.IO;
using DslModel.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DslModelToCSharp.Tests
{
    [TestClass]
    public class DomainClassWriterTests : TestBase
    {
        [TestMethod]
        public void TestAll_Snapshot()
        {
            var domainBuilder = new DomainBuilder(ClassWriter, DomainEventWriter, BaseClassBuilder, ValidationResultBaseClassBuilder);
            using (var reader = new StreamReader("Schema.wsb"))
            {
                var content = reader.ReadToEnd();
                var domainTree = DslParser.Parse(content);
                domainBuilder.Build(domainTree, DomainNameSpace, BasePathDomain);
            }

            Assert.AreEqual(File.ReadAllText("../../../DomainExpected/Generated/Users/CreateUserEvent.g.cs"),
                File.ReadAllText("Domain/Users/CreateUserEvent.g.cs"));
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
            ClassWriter.Write(new CreationResultBaseClass());
            new PrivateSetPropertyHackCleaner().ReplaceHackPropertyNames(BasePathDomain);

            Assert.AreEqual(File.ReadAllText("../../../DomainExpected/Generated/Base/CreationResult.g.cs"),
                File.ReadAllText("Domain/Base/CreationResult.g.cs"));
        }

        [TestMethod]
        public void DomainEventBaseClass_Builder()
        {
            DomainEventBaseClassBuilder.Build(new DomainEventBaseClass().Name, new DomainEventBaseClass().Properties);
            new PrivateSetPropertyHackCleaner().ReplaceHackPropertyNames(BasePathDomain);

            Assert.AreEqual(File.ReadAllText("../../../DomainExpected/Generated/Base/DomainEventBase.g.cs"),
                File.ReadAllText("Domain/Base/DomainEventBase.g.cs"));
        }
    }
}