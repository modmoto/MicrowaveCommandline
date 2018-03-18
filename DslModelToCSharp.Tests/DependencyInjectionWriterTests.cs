using System.IO;
using FileToDslModel;
using FileToDslModel.Lexer;
using FileToDslModel.ParseAutomat;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DslModelToCSharp.Tests
{
        [TestClass]
        public class DependencyInjectionWriterTests : TestBase
        {
            [TestMethod]
            public void Write()
            {
                var hookResultBuilder = new DependencyInjectionWriter(ApplicationNameSpace);

                using (var reader = new StreamReader("Schema.wsb"))
                {
                    var content = reader.ReadToEnd();
                    var domainTree = new DslParser(new Tokenizer(), new Parser()).Parse(content);

                    hookResultBuilder.Write(domainTree.Classes, domainTree.SynchronousDomainHooks, "Application/Base/");
                }

                Assert.AreEqual(File.ReadAllText("../../../ApplicationExpected/Generated/Base/GeneratedDependencies.g.cs"),
                    File.ReadAllText("Application/Base/GeneratedDependencies.g.cs"));
            }
        }
}