using System;
using System.IO;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microwave.LanguageParser;
using Microwave.LanguageParser.Lexer;
using Microwave.LanguageParser.ParseAutomat;
using Microwave.ServiceParser.HttpAdapter;

namespace Microwave.ServiceParser.Tests.HttpAdapter
{
    [TestClass]
    public class ControllerBuilderTests : TestBase
    {
        [TestMethod]
        public void Write()
        {
            var controllerBuilder = new ControllerBuilder(HttpAdpaterNameSpace);

            using (var reader = new StreamReader("Schema.mic"))
            {
                var content = reader.ReadToEnd();
                var domainTree = new DslParser(new MicrowaveLanguageTokenizer(), new MicrowaveLanguageParser()).Parse(content);

                foreach (var domainTreeClass in domainTree.Classes)
                {
                    var eventStore = controllerBuilder.Build(domainTreeClass);
                    new FileWriter(HttpAdpaterNameSpace).WriteToFile(eventStore.Types[0].Name, domainTreeClass.Name + "s", eventStore);
                }
            }

            new PrivateSetPropertyHackCleaner().ReplaceHackPropertyNames(HttpAdpaterBasePath);

            Assert.AreEqual(Regex.Replace(File.ReadAllText("../../../HttpAdapterExpected/Generated/Users/UserController.g.cs"), @"\s+", String.Empty),
                Regex.Replace(File.ReadAllText("HttpAdapter/Users/UserController.g.cs"), @"\s+", String.Empty));
            Assert.AreEqual(Regex.Replace(File.ReadAllText("../../../HttpAdapterExpected/Generated/Posts/PostController.g.cs"), @"\s+", String.Empty),
                Regex.Replace(File.ReadAllText("HttpAdapter/Posts/PostController.g.cs"), @"\s+", String.Empty));
        }
    }
}