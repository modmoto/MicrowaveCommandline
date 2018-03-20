using System;
using System.IO;
using System.Text.RegularExpressions;
using FileToDslModel;
using FileToDslModel.Lexer;
using FileToDslModel.ParseAutomat;
using NUnit.Framework;

namespace DslModelToCSharp.Tests
{
        [TestFixture]
        public class DependencyInjectionWriterTests : TestBase
        {
            [Test]
            public void Write()
            {
                var hookResultBuilder = new DependencyInjectionWriter(ApplicationNameSpace);

                using (var reader = new StreamReader("Schema.wsb"))
                {
                    var content = reader.ReadToEnd();
                    var domainTree = new DslParser(new Tokenizer(), new Parser()).Parse(content);

                    hookResultBuilder.Write(domainTree.Classes, domainTree.SynchronousDomainHooks, "Application/Base/");
                }

                Assert.AreEqual(Regex.Replace(File.ReadAllText("../../../ApplicationExpected/Generated/Base/GeneratedDependencies.g.cs"), @"\s+", String.Empty),
                    Regex.Replace(File.ReadAllText("Application/Base/GeneratedDependencies.g.cs"), @"\s+", String.Empty));
            }
        }
}