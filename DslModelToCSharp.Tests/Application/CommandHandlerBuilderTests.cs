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
                    new FileWriter(ApplicationBasePath).WriteToFile(codeNamespace.Types[0].Name, domainClass.Name + "s", codeNamespace);
                }
            }

            new PrivateSetPropertyHackCleaner().ReplaceHackPropertyNames(ApplicationBasePath);

            Assert.AreEqual(File.ReadAllText("../../../ApplicationExpected/Generated/Posts/PostCommandHandler.g.cs").Replace("\\r\\n", "").Replace("\\n", "").Replace("\\r", ""),
                File.ReadAllText("Application/Posts/PostCommandHandler.g.cs").Replace("\\r\\n", "").Replace("\\n", "").Replace("\\r", ""));
            Assert.AreEqual(File.ReadAllText("../../../ApplicationExpected/Generated/Users/UserCommandHandler.g.cs").Replace("\\n", "").Replace("\\r\\n", "").Replace("\\n", "").Replace("\\r", ""),
                File.ReadAllText("Application/Users/UserCommandHandler.g.cs").Replace("\\r\\n", "").Replace("\\n", "").Replace("\\r", ""));
        }
    }
}