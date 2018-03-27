using System;
using System.IO;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microwave.LanguageParser;
using Microwave.LanguageParser.Lexer;
using Microwave.LanguageParser.ParseAutomat;
using Microwave.WebServiceGenerator.Application;

namespace Microwave.WebServiceGenerator.Tests.Application
{
    [TestClass]
    public class CommandHandlerBuilderTests : TestBase
    {
        [TestMethod]
        public void Build()
        {
            var commandHandlerBuilder = new CommandHandlerBuilder(ApplicationNameSpace);

            using (var reader = new StreamReader("Schema.mic"))
            {
                var content = reader.ReadToEnd();
                var domainTree = new DslParser(new MicrowaveLanguageTokenizer(), new MicrowaveLanguageParser()).Parse(content);
                foreach (var domainClass in domainTree.Classes)
                {
                    var codeNamespace = commandHandlerBuilder.Build(domainClass);
                    new FileWriter(ApplicationBasePath).WriteToFile(codeNamespace.Types[0].Name, domainClass.Name + "s", codeNamespace);
                }
            }

            new PrivateSetPropertyHackCleaner().ReplaceHackPropertyNames(ApplicationBasePath);

            Assert.AreEqual(Regex.Replace(File.ReadAllText("../../../ApplicationExpected/Generated/Posts/PostCommandHandler.g.cs"), @"\s+", String.Empty),
                Regex.Replace(File.ReadAllText("Application/Posts/PostCommandHandler.g.cs"), @"\s+", String.Empty));
            Assert.AreEqual(Regex.Replace(File.ReadAllText("../../../ApplicationExpected/Generated/Users/UserCommandHandler.g.cs"), @"\s+", String.Empty),
            Regex.Replace(File.ReadAllText("Application/Users/UserCommandHandler.g.cs"), @"\s+", String.Empty));
        }
    }
}