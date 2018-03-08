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
        private readonly string _domainNameSpace = "Domain";

        [TestInitialize]
        public void Setup()
        {
            if (Directory.Exists("DomainActual")) Directory.Delete("DomainActual", true);
        }

        [TestMethod]
        public void TestAll_Snapshot()
        {
            var fileWriter = new FileWriter("");
            var domainEventWriter =
                new DomainEventWriter(new PropBuilder(), fileWriter, new ClassBuilder(), new ConstBuilder());
            var classWriter = new DomainClassWriter(new InterfaceBuilder(), new PropBuilder(), new ClassBuilder()
                , fileWriter, new ConstBuilder(), new StaticConstructorBuilder(),
                new NameSpaceBuilder(), _domainNameSpace);
            var tokenizer = new Tokenizer();
            var parser = new Parser();

            using (var reader = new StreamReader("Schema.wsb"))
            {
                var content = reader.ReadToEnd();

                var dslParser = new DslParser(tokenizer, parser);
                var domainTree = dslParser.Parse(content);

                foreach (var domainClass in domainTree.Classes)
                {
                    foreach (var domainEvent in domainClass.Events)
                        domainEventWriter.Write(domainEvent, $"{_domainNameSpace}.{domainClass.Name}s", _basePath);
                    classWriter.Write(domainClass);
                }
            }

            Assert.AreEqual(File.ReadAllText("../../../DomainExpected/Generated/Domain/Users/CreateUserEvent.g.cs"),
                File.ReadAllText("Domain/Users/CreateUserEvent.g.cs"));
            Assert.AreEqual(File.ReadAllText("../../../DomainExpected/Generated/Domain/Users/User.g.cs"),
                File.ReadAllText("Domain/Users/User.g.cs"));
            Assert.AreEqual(File.ReadAllText("../../../DomainExpected/Generated/Domain/Users/UserUpdateAgeEvent.g.cs"),
                File.ReadAllText("Domain/Users/UserUpdateAgeEvent.g.cs"));
            Assert.AreEqual(File.ReadAllText("../../../DomainExpected/Generated/Domain/Users/UserUpdateNameEvent.g.cs"),
                File.ReadAllText("Domain/Users/UserUpdateNameEvent.g.cs"));
        }

        [TestMethod]
        public void CreateionResultBase_Builder()
        {
            var fileWriter = new FileWriter("");
            var classWriter = new DomainClassWriter(new InterfaceBuilder(), new PropBuilder(), new ClassBuilder(),
                fileWriter, new ConstBuilder(), new StaticConstructorBuilder(),
                new NameSpaceBuilder(), _domainNameSpace);

            classWriter.Write(new CreationResultBaseClass());

            Assert.AreEqual(File.ReadAllText("../../../DomainExpected/Generated/Domain/Base/CreationResult.g.cs"),
                File.ReadAllText("Domain/Base/CreationResult.g.cs"));
        }

        [TestMethod]
        public void DomainEventBaseClass_Builder()
        {
            var domainEventBaseClassBuilder = new DomainEventBaseClassBuilder(new PropBuilder(), new ConstBuilder(),
                new FileWriter(""), new NameSpaceBuilder(), new ClassBuilder(), _domainNameSpace);
            domainEventBaseClassBuilder.Build(new DomainEventBaseClass().Name, new DomainEventBaseClass().Properties);

            Assert.AreEqual(File.ReadAllText("../../../DomainExpected/Generated/Domain/Base/DomainEventBase.g.cs"),
                File.ReadAllText("Domain/Base/DomainEventBase.g.cs"));
        }

        [TestMethod]
        public void ValidationResultBaseClass_Builder()
        {
            var classWriter = new DomainClassWriter(new InterfaceBuilder(), new PropBuilder(), new ClassBuilder(),
                new FileWriter(""), new ConstBuilder(), new StaticConstructorBuilder(),
                new NameSpaceBuilder(),
                _domainNameSpace);

            classWriter.Write(new ValidationResultBaseClass());

            Assert.AreEqual(File.ReadAllText("../../../DomainExpected/Generated/Domain/Base/ValidationResult.g.cs"),
                File.ReadAllText("Domain/Base/ValidationResult.g.cs"));
        }
    }
}