using System.IO;
using DslModel;
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
                domainBuilder.Build(domainTree, DomainNameSpace, BasePath);
            }

            Assert.AreEqual(File.ReadAllText("../../../DomainExpected/Generated/Users/CreateUserEvent.g.cs"),
                File.ReadAllText("Generated/Users/CreateUserEvent.g.cs"));
            Assert.AreEqual(File.ReadAllText("../../../DomainExpected/Generated/Users/User.g.cs"),
                File.ReadAllText("Generated/Users/User.g.cs"));
            Assert.AreEqual(File.ReadAllText("../../../DomainExpected/Generated/Users/UserUpdateAgeEvent.g.cs"),
                File.ReadAllText("Generated/Users/UserUpdateAgeEvent.g.cs"));
            Assert.AreEqual(File.ReadAllText("../../../DomainExpected/Generated/Users/UserUpdateNameEvent.g.cs"),
                File.ReadAllText("Generated/Users/UserUpdateNameEvent.g.cs"));
        }

        [TestMethod]
        public void CreateionResultBase_Builder()
        {
            ClassWriter.Write(new CreationResultBaseClass());
            new PrivateSetPropertyHackCleaner().ReplaceHackPropertyNames(BasePath);

            Assert.AreEqual(File.ReadAllText("../../../DomainExpected/Generated/Base/CreationResult.g.cs"),
                File.ReadAllText("Generated/Base/CreationResult.g.cs"));
        }

        [TestMethod]
        public void DomainEventBaseClass_Builder()
        {
            DomainEventBaseClassBuilder.Build(new DomainEventBaseClass().Name, new DomainEventBaseClass().Properties);
            new PrivateSetPropertyHackCleaner().ReplaceHackPropertyNames(BasePath);

            Assert.AreEqual(File.ReadAllText("../../../DomainExpected/Generated/Base/DomainEventBase.g.cs"),
                File.ReadAllText("Generated/Base/DomainEventBase.g.cs"));
        }
    }
}