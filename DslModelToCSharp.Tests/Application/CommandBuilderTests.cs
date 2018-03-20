using System;
using System.IO;
using System.Text.RegularExpressions;
using DslModelToCSharp.Domain;
using FileToDslModel;
using FileToDslModel.Lexer;
using FileToDslModel.ParseAutomat;
using NUnit.Framework;

namespace DslModelToCSharp.Tests.Application
{
    [TestFixture]
    public class CommandBuilderTests : TestBase
    {
        [Test]
        public void Build()
        {
            var commandBuilder = new CommandBuilder();

            using (var reader = new StreamReader("Schema.wsb"))
            {
                var content = reader.ReadToEnd();
                var domainTree = new DslParser(new Tokenizer(), new Parser()).Parse(content);

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
            Assert.AreEqual(Regex.Replace(File.ReadAllText("../../../DomainExpected/Generated/Users/Commands/UserUpdateAgeCommand.g.cs"), @"\s+", String.Empty),
                Regex.Replace(File.ReadAllText("Domain/Users/Commands/UserUpdateAgeCommand.g.cs"), @"\s+", String.Empty));
            Assert.AreEqual(Regex.Replace(File.ReadAllText("../../../DomainExpected/Generated/Users/Commands/UserUpdateNameCommand.g.cs"), @"\s+", String.Empty),
                Regex.Replace(File.ReadAllText("Domain/Users/Commands/UserUpdateNameCommand.g.cs"), @"\s+", String.Empty));
            Assert.AreEqual(Regex.Replace(File.ReadAllText("../../../DomainExpected/Generated/Posts/Commands/PostCreateCommand.g.cs"), @"\s+", String.Empty),
                Regex.Replace(File.ReadAllText("Domain/Posts/Commands/PostCreateCommand.g.cs"), @"\s+", String.Empty));
        }
    }
}