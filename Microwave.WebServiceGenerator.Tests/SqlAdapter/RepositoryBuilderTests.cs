using System;
using System.IO;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microwave.LanguageParser;
using Microwave.LanguageParser.Lexer;
using Microwave.LanguageParser.ParseAutomat;
using Microwave.WebServiceGenerator.SqlAdapter;

namespace Microwave.WebServiceGenerator.Tests.SqlAdapter
{
    [TestClass]
    public class RepositoryBuilderTests : TestBase
    {
        [TestMethod]
        public void Write()
        {
            var storeBuilder = new RepositoryBuilder(SqlAdpaterNameSpace);

            using (var reader = new StreamReader("Schema.mic"))
            {
                var content = reader.ReadToEnd();
                var domainTree = new DslParser(new MicrowaveLanguageTokenizer(), new MicrowaveLanguageParser()).Parse(content);

                foreach (var domainTreeClass in domainTree.Classes)
                {
                    var eventStore = storeBuilder.Build(domainTreeClass);
                    new FileWriter(SqlAdpaterNameSpace).WriteToFile(domainTreeClass.Name + "s", eventStore);
                }
                
            }

            new PrivateSetPropertyHackCleaner().ReplaceHackPropertyNames(SqlAdpaterBasePath);

            Assert.AreEqual(Regex.Replace(File.ReadAllText("../../../SqlAdapterExpected/Generated/Users/UserRepository.g.cs"), @"\s+", String.Empty),
                Regex.Replace(File.ReadAllText("SqlAdapter/Users/UserRepository.g.cs"), @"\s+", String.Empty));
            Assert.AreEqual(Regex.Replace(File.ReadAllText("../../../SqlAdapterExpected/Generated/Posts/PostRepository.g.cs"), @"\s+", String.Empty),
                Regex.Replace(File.ReadAllText("SqlAdapter/Posts/PostRepository.g.cs"), @"\s+", String.Empty));
        }
    }
}