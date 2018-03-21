using System;
using System.IO;
using System.Text.RegularExpressions;
using DslModelToCSharp.HttpAdapter;
using FileToDslModel;
using FileToDslModel.Lexer;
using FileToDslModel.ParseAutomat;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DslModelToCSharp.Tests.HttpAdapter
{
    [TestClass]
    public class ControllerBuilderTests : TestBase
    {
        [TestMethod]
        public void Write()
        {
            var storeBuilder = new ControllerBuilder(HttpAdpaterNameSpace);

            using (var reader = new StreamReader("Schema.wsb"))
            {
                var content = reader.ReadToEnd();
                var domainTree = new DslParser(new Tokenizer(), new Parser()).Parse(content);

                foreach (var domainTreeClass in domainTree.Classes)
                {
                    var eventStore = storeBuilder.Build(domainTreeClass);
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