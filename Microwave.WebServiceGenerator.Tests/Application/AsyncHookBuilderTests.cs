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
    public class AsyncHookBuilderTests : TestBase
    {
        [TestMethod]
        public void BuildTests()
        {
            using (var reader = new StreamReader("Schema.mic"))
            {
                var content = reader.ReadToEnd();
                var domainTree =
                    new DslParser(new MicrowaveLanguageTokenizer(), new MicrowaveLanguageParser()).Parse(content);
                var classBuilderDirector = new ClassBuilderDirector();
                foreach (var hook in domainTree.AsyncDomainHooks)
                {
                    var codeNamespace = classBuilderDirector.BuildInstance(new AsyncHookBuilder(hook));
                    new FileWriter(ApplicationBasePath).WriteToFile(hook.ClassType + "s/AsyncHooks/", codeNamespace, false);
                }
            }

            new PrivateSetPropertyHackCleaner().ReplaceHackPropertyNames(ApplicationBasePath);

            Assert.AreEqual(
                Regex.Replace(
                    File.ReadAllText("../../../ApplicationExpected/Generated/Users/AsyncHooks/OnUserCreateSendPasswordMailAsyncHook.cs"),
                    @"\s+", String.Empty),
                Regex.Replace(File.ReadAllText("Application/Users/AsyncHooks/OnUserCreateSendPasswordMailAsyncHook.cs"), @"\s+",
                    String.Empty));
        }
    }
}