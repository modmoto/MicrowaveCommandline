using System;
using System.IO;
using System.Text.RegularExpressions;
using DslModelToCSharp.Application;
using FileToDslModel;
using FileToDslModel.Lexer;
using FileToDslModel.ParseAutomat;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DslModelToCSharp.Tests.Application
{
    [TestClass]
    public class SyncHookBuilderTests : TestBase
    {
        [TestMethod]
        public void BuildTests()
        {
            var commandHandlerBuilder = new SynchronousHookBuilder(ApplicationNameSpace);

            using (var reader = new StreamReader("Schema.wsb"))
            {
                var content = reader.ReadToEnd();
                var domainTree = new DslParser(new Tokenizer(), new Parser()).Parse(content);
                foreach (var hook in domainTree.SynchronousDomainHooks)
                {
                    var codeNamespace = commandHandlerBuilder.Build(hook);
                    new FileWriter(ApplicationBasePath).WriteToFile($"{hook.Name}Hook", hook.ClassType + "s/Hooks/", codeNamespace);
                }
            }

            new PrivateSetPropertyHackCleaner().ReplaceHackPropertyNames(ApplicationBasePath);

            Assert.AreEqual(Regex.Replace(File.ReadAllText("../../../ApplicationExpected/Generated/Users/Hooks/SendPasswordMailHook.g.cs"), @"\s+", String.Empty),
                Regex.Replace(File.ReadAllText("Application/Users/Hooks/SendPasswordMailHook.g.cs"), @"\s+", String.Empty));
        }

        [TestMethod]
        public void BuildReplacementClass()
        {
            var commandHandlerBuilder = new SynchronousHookBuilder(ApplicationNameSpace);

            using (var reader = new StreamReader("Schema.wsb"))
            {
                var content = reader.ReadToEnd();
                var domainTree = new DslParser(new Tokenizer(), new Parser()).Parse(content);
                foreach (var hook in domainTree.SynchronousDomainHooks)
                {
                    var codeNamespace = commandHandlerBuilder.BuildReplacementClass(hook);
                    new FileWriter(ApplicationBasePath).WriteToFile($"{hook.Name}Hook", hook.ClassType + "s/Hooks/", codeNamespace, false);
                }
            }

            new PrivateSetPropertyHackCleaner().ReplaceHackPropertyNames(ApplicationBasePath);

            Assert.AreEqual(Regex.Replace(File.ReadAllText("../../../ApplicationExpected/Generated/Users/Hooks/SendPasswordMailHook.cs"), @"\s+", String.Empty),
                Regex.Replace(File.ReadAllText("Application/Users/Hooks/SendPasswordMailHook.cs"), @"\s+", String.Empty));
        }


    }
}