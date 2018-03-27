﻿using System;
using System.IO;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microwave.LanguageParser;
using Microwave.LanguageParser.Lexer;
using Microwave.LanguageParser.ParseAutomat;

namespace Microwave.ServiceParser.Tests
{
        [TestClass]
        public class DependencyInjectionWriterTests : TestBase
        {
            [TestMethod]
            public void Write()
            {
                var hookResultBuilder = new DependencyInjectionWriter(ApplicationNameSpace);

                using (var reader = new StreamReader("Schema.mic"))
                {
                    var content = reader.ReadToEnd();
                    var domainTree = new DslParser(new MicrowaveLanguageTokenizer(), new MicrowaveLanguageParser()).Parse(content);

                    hookResultBuilder.Write(domainTree.Classes, domainTree.SynchronousDomainHooks, "Application/Base/");
                }

                Assert.AreEqual(Regex.Replace(File.ReadAllText("../../../ApplicationExpected/Generated/Base/GeneratedDependencies.g.cs"), @"\s+", String.Empty),
                    Regex.Replace(File.ReadAllText("Application/Base/GeneratedDependencies.g.cs"), @"\s+", String.Empty));
            }
        }
}