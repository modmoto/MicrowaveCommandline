using System;
using System.IO;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microwave.LanguageParser;
using Microwave.LanguageParser.Lexer;
using Microwave.LanguageParser.ParseAutomat;
using Microwave.WebServiceGenerator.Domain;
using Microwave.WebServiceModel.Domain;

namespace Microwave.WebServiceGenerator.Tests.Domain
{
    [TestClass]
    public class DomainClassWriterTests : TestBase
    {
        [TestMethod]
        public void TestDomainClasses()
        {
            var domainBuilder = new DomainWriter(DomainNameSpace, DomainBasePath, SolutionBasePath);
            using (var reader = new StreamReader("Schema.mic"))
            {
                var content = reader.ReadToEnd();
                var domainTree = new DslParser(new MicrowaveLanguageTokenizer(), new MicrowaveLanguageParser()).Parse(content);
                domainBuilder.Build(domainTree, DomainBasePath);
            }

            Assert.AreEqual(Regex.Replace(File.ReadAllText("../../../DomainExpected/Generated/Users/User.g.cs"), @"\s+", String.Empty),
            Regex.Replace(File.ReadAllText("Domain/Users/User.g.cs"), @"\s+", String.Empty));

            Assert.AreEqual(Regex.Replace(File.ReadAllText("../../../DomainExpected/Generated/Posts/Post.g.cs"), @"\s+", String.Empty),
            Regex.Replace(File.ReadAllText("Domain/Posts/Post.g.cs"), @"\s+", String.Empty));
        }

        [TestMethod]
        public void TestCreateEvents()
        {
            var domainBuilder = new DomainWriter(DomainNameSpace, DomainBasePath, SolutionBasePath);
            using (var reader = new StreamReader("Schema.mic"))
            {
                var content = reader.ReadToEnd();
                var domainTree = new DslParser(new MicrowaveLanguageTokenizer(), new MicrowaveLanguageParser()).Parse(content);
                domainBuilder.Build(domainTree, DomainBasePath);
            }

            Assert.AreEqual(Regex.Replace(File.ReadAllText("../../../DomainExpected/Generated/Users/UserCreateEvent.g.cs"), @"\s+", String.Empty),
                Regex.Replace(File.ReadAllText("Domain/Users/UserCreateEvent.g.cs"), @"\s+", String.Empty));
        }

        [TestMethod]
        public void TestUpdateEvents()
        {
            var domainBuilder = new DomainWriter(DomainNameSpace, DomainBasePath, SolutionBasePath);
            using (var reader = new StreamReader("Schema.mic"))
            {
                var content = reader.ReadToEnd();
                var domainTree = new DslParser(new MicrowaveLanguageTokenizer(), new MicrowaveLanguageParser()).Parse(content);
                domainBuilder.Build(domainTree, DomainBasePath);
            }

            Assert.AreEqual(Regex.Replace(File.ReadAllText("../../../DomainExpected/Generated/Users/UserUpdateAgeEvent.g.cs"), @"\s+", String.Empty),
                Regex.Replace(File.ReadAllText("Domain/Users/UserUpdateAgeEvent.g.cs"), @"\s+", String.Empty));
            Assert.AreEqual(Regex.Replace(File.ReadAllText("../../../DomainExpected/Generated/Users/UserAddPostEvent.g.cs"), @"\s+", String.Empty),
                Regex.Replace(File.ReadAllText("Domain/Users/UserAddPostEvent.g.cs"), @"\s+", String.Empty));
            Assert.AreEqual(Regex.Replace(File.ReadAllText("../../../DomainExpected/Generated/Users/UserUpdateNameEvent.g.cs"), @"\s+", String.Empty),
            Regex.Replace(File.ReadAllText("Domain/Users/UserUpdateNameEvent.g.cs"), @"\s+", String.Empty));
        }

        [TestMethod]
        public void CreationResultBase_Builder()
        {
            var creationResult = new CreationResultBaseClassBuilder(DomainNameSpace).Build(new CreationResultBaseClass());
            new FileWriter(DomainBasePath).WriteToFile("CreationResult", "Base", creationResult);

            new PrivateSetPropertyHackCleaner().ReplaceHackPropertyNames(DomainBasePath);

            Assert.AreEqual(Regex.Replace(File.ReadAllText("../../../DomainExpected/Generated/Base/CreationResult.g.cs"), @"\s+", String.Empty),
            Regex.Replace(File.ReadAllText("Domain/Base/CreationResult.g.cs"), @"\s+", String.Empty));
        }

        [TestMethod]
        public void DomainEventBaseClass_Builder()
        {
            var classFactory = new ClassFactory();
            var baseClass = classFactory.BuildInstance(new DomainEventBaseClassBuilder());
            new FileWriter(DomainBasePath).WriteToFile("DomainEventBase", "Base", baseClass);

            new PrivateSetPropertyHackCleaner().ReplaceHackPropertyNames(DomainBasePath);

            Assert.AreEqual(Regex.Replace(File.ReadAllText("../../../DomainExpected/Generated/Base/DomainEventBase.g.cs"), @"\s+", String.Empty),
            Regex.Replace(File.ReadAllText("Domain/Base/DomainEventBase.g.cs"), @"\s+", String.Empty));
        }
    }
}