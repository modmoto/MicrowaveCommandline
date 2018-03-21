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
    public class ApiCommandBuilderTests : TestBase
    {
        [TestMethod]
        public void Build()
        {
            var commandBuilder = new ApiCommandBuilder();

            using (var reader = new StreamReader("Schema.wsb"))
            {
                var content = reader.ReadToEnd();
                var domainTree = new DslParser(new Tokenizer(), new Parser()).Parse(content);
                foreach (var domainClass in domainTree.Classes)
                {
                    foreach (var method in domainClass.LoadMethods)
                    {
                        var codeNamespace = commandBuilder.Build(method, domainClass);
                        new FileWriter(ApplicationBasePath).WriteToFile(codeNamespace.Types[0].Name, domainClass.Name + "s", codeNamespace);
                    }                        
                }
            }

            new PrivateSetPropertyHackCleaner().ReplaceHackPropertyNames(ApplicationBasePath);

            Assert.AreEqual(Regex.Replace(File.ReadAllText("../../../ApplicationExpected/Generated/Users/UserAddPostApiCommand.g.cs"), @"\s+", String.Empty),
            Regex.Replace(File.ReadAllText("Application/Users/UserAddPostApiCommand.g.cs"), @"\s+", String.Empty));
        }
    }
}