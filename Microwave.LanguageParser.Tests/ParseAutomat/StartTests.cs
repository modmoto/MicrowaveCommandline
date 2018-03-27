using System.Collections.ObjectModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microwave.LanguageParser.Lexer;
using Microwave.LanguageParser.ParseAutomat;

namespace Microwave.LanguageParser.Tests.ParseAutomat
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

            var parser = new MicrowaveLanguageParser();
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