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
        private readonly string _domainNameSpace = "Domain";
        private string _basePath = "Generated/";


        [TestInitialize]
        public void Setup()
        {
            if (Directory.Exists("DomainActual")) Directory.Delete("DomainActual", true);
        }

        [TestMethod]
        public void TestAll_Snapshot()
        {
            var fileWriter = new FileWriter(_basePath);
            var domainEventWriter =
                new DomainEventWriter(new PropBuilder(), fileWriter, new ClassBuilder(), new ConstBuilder());
            var classWriter = new DomainClassWriter(new InterfaceBuilder(), new PropBuilder(), new ClassBuilder()
                , fileWriter, new ConstBuilder(), new StaticConstructorBuilder(),
                new NameSpaceBuilder(), _domainNameSpace);
            var tokenizer = new Tokenizer();
            var parser = new Parser();

            var baseClassBuilder = new DomainEventBaseClassWriter(new PropBuilder(), new ConstBuilder(), fileWriter,
                new NameSpaceBuilder(), new ClassBuilder(), _domainNameSpace);
            var domainBuilder = new DomainBuilder(classWriter, domainEventWriter, baseClassBuilder);
            using (var reader = new StreamReader("Schema.wsb"))
            {
                var content = reader.ReadToEnd();

                var dslParser = new DslParser(tokenizer, parser);
                var domainTree = dslParser.Parse(content);

                domainBuilder.Build(domainTree, _domainNameSpace, _basePath);
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
            var fileWriter = new FileWriter(_basePath);
            var classWriter = new DomainClassWriter(new InterfaceBuilder(), new PropBuilder(), new ClassBuilder(),
                fileWriter, new ConstBuilder(), new StaticConstructorBuilder(),
                new NameSpaceBuilder(), _domainNameSpace);

            classWriter.Write(new CreationResultBaseClass());
            new PrivateSetPropertyHackCleaner().ReplaceHackPropertyNames(_basePath);

            Assert.AreEqual(File.ReadAllText("../../../DomainExpected/Generated/Base/CreationResult.g.cs"),
                File.ReadAllText("Generated/Base/CreationResult.g.cs"));
        }

        [TestMethod]
        public void DomainEventBaseClass_Builder()
        {
            var domainEventBaseClassBuilder = new DomainEventBaseClassWriter(new PropBuilder(), new ConstBuilder(),
                new FileWriter(_basePath), new NameSpaceBuilder(), new ClassBuilder(), _domainNameSpace);
            domainEventBaseClassBuilder.Build(new DomainEventBaseClass().Name, new DomainEventBaseClass().Properties);
            new PrivateSetPropertyHackCleaner().ReplaceHackPropertyNames(_basePath);

            Assert.AreEqual(File.ReadAllText("../../../DomainExpected/Generated/Base/DomainEventBase.g.cs"),
                File.ReadAllText("Generated/Base/DomainEventBase.g.cs"));
        }

        [TestMethod]
        public void ValidationResultBaseClass_Builder()
        {
            var classWriter = new DomainClassWriter(new InterfaceBuilder(), new PropBuilder(), new ClassBuilder(),
                new FileWriter(_basePath), new ConstBuilder(), new StaticConstructorBuilder(),
                new NameSpaceBuilder(),
                _domainNameSpace);

            classWriter.Write(new ValidationResultBaseClass());
            new PrivateSetPropertyHackCleaner().ReplaceHackPropertyNames(_basePath);

            Assert.AreEqual(File.ReadAllText("../../../DomainExpected/Generated/Base/ValidationResult.g.cs"),
                File.ReadAllText("Generated/Base/ValidationResult.g.cs"));
        }
    }
}