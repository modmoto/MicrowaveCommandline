using System.IO;
using DslModel;
using FileToDslModel;
using FileToDslModel.Lexer;
using FileToDslModel.ParseAutomat;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DslModelToCSharp.Tests
{
    [TestClass]
    public class DomainClassWriterTests
    {
        private readonly string _basePath = "DomainActual/Generated";

        [TestInitialize]
        public void Setup()
        {
            if (Directory.Exists("DomainActual")) Directory.Delete("DomainActual", true);
        }

        [TestMethod]
        public void TestAll_Snapshot()
        {
            var domainEventWriter =
                new DomainEventWriter(new PropBuilder(), new FileWriter(), new ClassBuilder(), new ConstBuilder());
            var classWriter = new DomainClassWriter(new InterfaceBuilder(), new PropBuilder(), new ClassBuilder(),
                domainEventWriter, new FileWriter(), new ConstBuilder(), new StaticConstructorBuilder());
            var tokenizer = new Tokenizer();
            var parser = new Parser();

            using (var reader = new StreamReader("Schema.wsb"))
            {
                var content = reader.ReadToEnd();

                var dslParser = new DslParser(tokenizer, parser);
                var domainTree = dslParser.Parse(content);

                foreach (var domainClass in domainTree.Classes)
                    classWriter.Write(domainClass, _basePath);
            }

            Assert.AreEqual(File.ReadAllText("../../../DomainExpected/Generated/Domain/Users/CreateUserEvent.g.cs"),
                File.ReadAllText("DomainActual/Generated/Domain/Users/CreateUserEvent.g.cs"));
            Assert.AreEqual(File.ReadAllText("../../../DomainExpected/Generated/Domain/Users/User.g.cs"),
                File.ReadAllText("DomainActual/Generated/Domain/Users/User.g.cs"));
            Assert.AreEqual(File.ReadAllText("../../../DomainExpected/Generated/Domain/Users/UserUpdateAgeEvent.g.cs"),
                File.ReadAllText("DomainActual/Generated/Domain/Users/UserUpdateAgeEvent.g.cs"));
            Assert.AreEqual(File.ReadAllText("../../../DomainExpected/Generated/Domain/Users/UserUpdateNameEvent.g.cs"),
                File.ReadAllText("DomainActual/Generated/Domain/Users/UserUpdateNameEvent.g.cs"));
        }

        [TestMethod]
        public void CreateionResultBase_Builder()
        {
            var domainEventWriter =
                new DomainEventWriter(new PropBuilder(), new FileWriter(), new ClassBuilder(), new ConstBuilder());
            var classWriter = new DomainClassWriter(new InterfaceBuilder(), new PropBuilder(), new ClassBuilder(),
                domainEventWriter, new FileWriter(), new ConstBuilder(), new StaticConstructorBuilder());

            classWriter.Write(new CreationResultBaseClass(), _basePath);

            Assert.AreEqual(File.ReadAllText("../../../DomainExpected/Generated/Domain/Base/CreationResult.g.cs"),
                File.ReadAllText("DomainActual/Generated/Domain/Base/CreationResult.g.cs"));
        }

        [TestMethod]
        public void DomainEventBaseClass_Builder()
        {
            var domainEventWriter =
                new DomainEventWriter(new PropBuilder(), new FileWriter(), new ClassBuilder(), new ConstBuilder());
            var classWriter = new DomainClassWriter(new InterfaceBuilder(), new PropBuilder(), new ClassBuilder(),
                domainEventWriter, new FileWriter(), new ConstBuilder(), new StaticConstructorBuilder());

            classWriter.Write(new DomainEventBaseClass(), _basePath);

            Assert.AreEqual(File.ReadAllText("../../../DomainExpected/Generated/Domain/Base/DomainEventBase.g.cs"),
                File.ReadAllText("DomainActual/Generated/Domain/Base/DomainEventBase.g.cs"));
        }

        [TestMethod]
        public void ValidationResultBaseClass_Builder()
        {
            var domainEventWriter =
                new DomainEventWriter(new PropBuilder(), new FileWriter(), new ClassBuilder(), new ConstBuilder());
            var classWriter = new DomainClassWriter(new InterfaceBuilder(), new PropBuilder(), new ClassBuilder(),
                domainEventWriter, new FileWriter(), new ConstBuilder(), new StaticConstructorBuilder());

            classWriter.Write(new ValidationResultBaseClass(), _basePath);

            Assert.AreEqual(File.ReadAllText("../../../DomainExpected/Generated/Domain/Base/ValidationResult.g.cs"),
                File.ReadAllText("DomainActual/Generated/Domain/Base/ValidationResult.g.cs"));
        }
    }
}