using System;
using System.IO;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microwave.LanguageParser;
using Microwave.LanguageParser.Lexer;
using Microwave.LanguageParser.ParseAutomat;
using Microwave.WebServiceGenerator.Domain;

namespace Microwave.WebServiceGenerator.Tests.Domain
{
    [TestClass]
    public class CommandBuilderTests : TestBase
    {
        [TestMethod]
        public void Build()
        {
            var commandBuilder = new CommandBuilder();

            using (var reader = new StreamReader("Schema.mic"))
            {
                var content = reader.ReadToEnd();
                var domainTree = new DslParser(new MicrowaveLanguageTokenizer(), new MicrowaveLanguageParser()).Parse(content);

                foreach (var domainClass in domainTree.Classes)
                {
                    var codeNamespaces = commandBuilder.Build(domainClass);

                    var fileWriter = new FileWriter(DomainBasePath);
                    foreach (var codeNamespace in codeNamespaces)
                    {
                        fileWriter.WriteToFile(codeNamespace.Types[0].Name, $"{domainClass.Name}s/Commands", codeNamespace);
                    }

                    new PrivateSetPropertyHackCleaner().ReplaceHackPropertyNames(DomainBasePath);
                    
                }
            }

            Assert.AreEqual(Regex.Replace(File.ReadAllText("../../../DomainExpected/Generated/Users/Commands/UserCreateCommand.g.cs"), @"\s+", String.Empty),
                Regex.Replace(File.ReadAllText("Domain/Users/Commands/UserCreateCommand.g.cs"), @"\s+", String.Empty));
            Assert.AreEqual(Regex.Replace(File.ReadAllText("../../../DomainExpected/Generated/Users/Commands/UserAddPostCommand.g.cs"), @"\s+", String.Empty),
                Regex.Replace(File.ReadAllText("Domain/Users/Commands/UserAddPostCommand.g.cs"), @"\s+", String.Empty));
            Assert.AreEqual(Regex.Replace(File.ReadAllText("../../../DomainExpected/Generated/Users/Commands/UserUpdateAgeCommand.g.cs"), @"\s+", String.Empty),
                Regex.Replace(File.ReadAllText("Domain/Users/Commands/UserUpdateAgeCommand.g.cs"), @"\s+", String.Empty));
            Assert.AreEqual(Regex.Replace(File.ReadAllText("../../../DomainExpected/Generated/Users/Commands/UserUpdateNameCommand.g.cs"), @"\s+", String.Empty),
                Regex.Replace(File.ReadAllText("Domain/Users/Commands/UserUpdateNameCommand.g.cs"), @"\s+", String.Empty));
            Assert.AreEqual(Regex.Replace(File.ReadAllText("../../../DomainExpected/Generated/Posts/Commands/PostCreateCommand.g.cs"), @"\s+", String.Empty),
                Regex.Replace(File.ReadAllText("Domain/Posts/Commands/PostCreateCommand.g.cs"), @"\s+", String.Empty));
        }
    }
}