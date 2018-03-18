using System.IO;
using DslModelToCSharp.Domain;
using FileToDslModel;
using FileToDslModel.Lexer;
using FileToDslModel.ParseAutomat;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DslModelToCSharp.Tests.Application
{
    [TestClass]
    public class CommandBuilderTests : TestBase
    {
        [TestMethod]
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

                    var fileWriter = new FileWriter(BasePathDomain);
                    foreach (var codeNamespace in codeNamespaces)
                    {
                        fileWriter.WriteToFile(codeNamespace.Types[0].Name, $"{domainClass.Name}s/Commands", codeNamespace);
                    }

                    new PrivateSetPropertyHackCleaner().ReplaceHackPropertyNames(BasePathDomain);
                    
                }
            }

            Assert.AreEqual(File.ReadAllText("../../../DomainExpected/Generated/Users/Commands/UserCreateCommand.g.cs"),
                File.ReadAllText("Domain/Users/Commands/UserCreateCommand.g.cs"));
            Assert.AreEqual(File.ReadAllText("../../../DomainExpected/Generated/Users/Commands/UserUpdateAgeCommand.g.cs"),
                File.ReadAllText("Domain/Users/Commands/UserUpdateAgeCommand.g.cs"));
            Assert.AreEqual(File.ReadAllText("../../../DomainExpected/Generated/Users/Commands/UserUpdateNameCommand.g.cs"),
                File.ReadAllText("Domain/Users/Commands/UserUpdateNameCommand.g.cs"));
            Assert.AreEqual(File.ReadAllText("../../../DomainExpected/Generated/Posts/Commands/PostCreateCommand.g.cs"),
                File.ReadAllText("Domain/Posts/Commands/PostCreateCommand.g.cs"));
        }
    }
}