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
        [TestMethod]
        public void TestAll_Snapshot()
        {
            IDomainEventWriter domainEventWriter =
                new DomainEventWriter(new PropBuilder(), new FileWriter(), new ClassBuilder(), new ConstBuilder());
            var classWriter = new DomainClassWriter(new InterfaceBuilder(), new PropBuilder(), new ClassBuilder(),
                domainEventWriter, new FileWriter(), new ConstBuilder(), new StaticConstructorBuilder());
            var tokenizer = new Tokenizer();
            var parser = new Parser();

            var basePath = "DomainActual/Generated";

            using (var reader = new StreamReader("Schema.wsb"))
            {
                var content = reader.ReadToEnd();

                var dslParser = new DslParser(tokenizer, parser);
                var domainTree = dslParser.Parse(content);

                foreach (var domainClass in domainTree.Classes)
                    classWriter.Write(domainClass, basePath);

                classWriter.Write(new ValidationResultBaseClass(), basePath);
                classWriter.Write(new DomainEventBaseClass(), basePath);
                classWriter.Write(new CreationResultBaseClass(), basePath);
            }


            Assert.AreEqual(File.ReadAllText("../../../DomainExpected/Generated/Domain/Base/CreationResult.g.cs"), File.ReadAllText("DomainActual/Generated/Domain/Base/CreationResult.g.cs"));
            Assert.AreEqual(File.ReadAllText("../../../DomainExpected/Generated/Domain/Base/DomainEventBase.g.cs"), File.ReadAllText("DomainActual/Generated/Domain/Base/DomainEventBase.g.cs"));
            Assert.AreEqual(File.ReadAllText("../../../DomainExpected/Generated/Domain/Base/ValidationResult.g.cs"), File.ReadAllText("DomainActual/Generated/Domain/Base/ValidationResult.g.cs"));

            Assert.AreEqual(File.ReadAllText("../../../DomainExpected/Generated/Domain/Posts/Post.g.cs"), File.ReadAllText("DomainActual/Generated/Domain/Posts/Post.g.cs"));
            Assert.AreEqual(File.ReadAllText("../../../DomainExpected/Generated/Domain/Posts/CreatePostEvent.g.cs"), File.ReadAllText("DomainActual/Generated/Domain/Posts/CreatePostEvent.g.cs"));

            Assert.AreEqual(File.ReadAllText("../../../DomainExpected/Generated/Domain/Users/CreateUserEvent.g.cs"), File.ReadAllText("DomainActual/Generated/Domain/Users/CreateUserEvent.g.cs"));
            Assert.AreEqual(File.ReadAllText("../../../DomainExpected/Generated/Domain/Users/User.g.cs"), File.ReadAllText("DomainActual/Generated/Domain/Users/User.g.cs"));
            Assert.AreEqual(File.ReadAllText("../../../DomainExpected/Generated/Domain/Users/UserUpdateAgeEvent.g.cs"), File.ReadAllText("DomainActual/Generated/Domain/Users/UserUpdateAgeEvent.g.cs"));
            Assert.AreEqual(File.ReadAllText("../../../DomainExpected/Generated/Domain/Users/UserUpdateNameEvent.g.cs"), File.ReadAllText("DomainActual/Generated/Domain/Users/UserUpdateNameEvent.g.cs"));
        }
    }
}
