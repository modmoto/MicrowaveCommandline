using System.IO;
using DslModel.Domain;
using DslModelToCSharp.Application;
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
                var domainTree = DslParser.Parse(content);

                foreach (var domainClass in domainTree.Classes)
                {
                    var codeNamespaces = commandBuilder.Build(domainClass);

                    var fileWriter = new FileWriter(BasePathApplication);
                    foreach (var codeNamespace in codeNamespaces)
                    {
                        fileWriter.WriteToFile(codeNamespace.Types[0].Name, $"{domainClass.Name}s/Commands", codeNamespace);
                    }
                    
                }
            }

            Assert.AreEqual(File.ReadAllText("../../../ApplicationExpected/Generated/Users/Commands/UserCreateCommand.g.cs"),
                File.ReadAllText("Application/Users/Commands/UserCreateCommand.g.cs"));
            Assert.AreEqual(File.ReadAllText("../../../ApplicationExpected/Generated/Users/Commands/UserUpdateAgeCommand.g.cs"),
                File.ReadAllText("Application/Users/Commands/UserUpdateAgeCommand.g.cs"));
            Assert.AreEqual(File.ReadAllText("../../../ApplicationExpected/Generated/Users/Commands/UserUpdateNameCommand.g.cs"),
                File.ReadAllText("Application/Users/Commands/UserUpdateNameCommand.g.cs"));
            Assert.AreEqual(File.ReadAllText("../../../ApplicationExpected/Generated/Posts/Commands/PostCreateCommand.g.cs"),
                File.ReadAllText("Application/Posts/Commands/PostCreateCommand.g.cs"));
        }
    }
}