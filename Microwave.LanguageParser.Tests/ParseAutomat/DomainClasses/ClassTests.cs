using System.Collections.ObjectModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microwave.LanguageParser.Lexer;
using Microwave.LanguageParser.ParseAutomat;

namespace Microwave.LanguageParser.Tests.ParseAutomat.DomainClasses
{
    [TestClass]
    public class ClassTests
    {
        [TestMethod]
        public void EmptyClass()
        {

            var tokens = new Collection<DslToken>
            {
                new DslToken(TokenType.DomainClass, "DomainClass", 1),
                new DslToken(TokenType.Value, "User", 1),
                new DslToken(TokenType.ObjectBracketOpen, "{", 1),
                new DslToken(TokenType.ObjectBracketClose, "}", 2)
            };

            var parser = new MicrowaveLanguageParser();
            var domainTree = parser.Parse(tokens);

            Assert.AreEqual(1, domainTree.Classes.Count);
            Assert.AreEqual("User", domainTree.Classes[0].Name);
            Assert.AreEqual(0, domainTree.Classes[0].Methods.Count);
            Assert.AreEqual(0, domainTree.Classes[0].Properties.Count);
        }

        [TestMethod]
        public void ExceptionTest()
        {

            var tokens = new Collection<DslToken>
            {
                new DslToken(TokenType.DomainClass, "DomainClass", 1),
                new DslToken(TokenType.DomainClass, "DomainClass", 1),
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

        [TestMethod]
        public void BodyNotOpenedException()
        {

            var tokens = new Collection<DslToken>
            {
                new DslToken(TokenType.DomainClass, "DomainClass", 1),
                new DslToken(TokenType.Value, "User", 1),
                new DslToken(TokenType.Value, "User", 1),
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