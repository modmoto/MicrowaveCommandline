using System;
using System.Collections.ObjectModel;
using FileToDslModel.Lexer;
using FileToDslModel.ParseAutomat;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FileToDslModel.Tests.ParseAutomat
{
    [TestClass]
    public class StartTests
    {
        [TestMethod]
        public void Parse_StartState_Eexception()
        {
            var tokens = new Collection<DslToken>
            {
                new DslToken(TokenType.ObjectBracketOpen, "{", 1),
            };

            var parser = new Parser();
            try
            {
                parser.Parse(tokens);
            }
            catch (NoTransitionException e)
            {
                Assert.IsTrue(e.Message.Contains("Unexpected Token"));
                return;
            }

            Assert.Fail();
        }
    }
}