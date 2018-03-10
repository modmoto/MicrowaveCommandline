using System.IO;
using DslModelToCSharp.Application;
using FileToDslModel;
using FileToDslModel.Lexer;
using FileToDslModel.ParseAutomat;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DslModelToCSharp.Tests.Application
{
    [TestClass]
    public class CommandHandlerBuilderTests : TestBase
    {
        [TestMethod]
        public void Build()
        {
            var commandHandlerBuilder = new CommandHandlerBuilder(ApplicationNameSpace);

            using (var reader = new StreamReader("Schema.wsb"))
            {
                var content = reader.ReadToEnd();
                var domainTree = new DslParser(new Tokenizer(), new Parser()).Parse(content);
                foreach (var domainClass in domainTree.Classes)
                {
                    var codeNamespace = commandHandlerBuilder.Build(domainClass);
                    new FileWriter(BasePathApplication).WriteToFile(codeNamespace.Types[0].Name, domainClass.Name + "s", codeNamespace);
                }
            }

            new PrivateSetPropertyHackCleaner().ReplaceHackPropertyNames(BasePathApplication);

            Assert.AreEqual(File.ReadAllText("../../../ApplicationExpected/Generated/Posts/PostCommandHandler.g.cs"),
                File.ReadAllText("Application/Posts/PostCommandHandler.g.cs"));
            Assert.AreEqual(File.ReadAllText("../../../ApplicationExpected/Generated/Users/UserCommandHandler.g.cs"),
                File.ReadAllText("Application/Users/UserCommandHandler.g.cs"));
        }
    }
}