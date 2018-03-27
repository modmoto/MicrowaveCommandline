using System.Collections.ObjectModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microwave.LanguageParser.Lexer;
using Microwave.LanguageParser.ParseAutomat;

namespace Microwave.LanguageParser.Tests.ParseAutomat.Members
{
    [TestClass]
    public class DomainEventsTests
    {
        [TestMethod]
        public void EventWithProperties()
        {
            var tokens = new Collection<DslToken>
            {
                new DslToken(TokenType.DomainClass, "DomainClass", 1),
                new DslToken(TokenType.Value, "User", 1),
                new DslToken(TokenType.ObjectBracketOpen, "{", 1),
                new DslToken(TokenType.Value, "UpdateStuff", 2),
                new DslToken(TokenType.ParameterBracketOpen, "(", 2),
                new DslToken(TokenType.ParameterBracketClose, ")", 2),
                new DslToken(TokenType.TypeDefSeparator, ":", 2),
                new DslToken(TokenType.ObjectBracketOpen, "{", 2),
                new DslToken(TokenType.Value, "UserId", 2),
                new DslToken(TokenType.TypeDefSeparator, ":", 2),
                new DslToken(TokenType.Value, "Guid", 2),
                new DslToken(TokenType.ObjectBracketClose, "}", 2),
                new DslToken(TokenType.ObjectBracketClose, "}", 3)
            };

            var parser = new MicrowaveLanguageParser();
            var domainTree = parser.Parse(tokens);

            Assert.AreEqual(1, domainTree.Classes[0].Methods.Count);
            Assert.AreEqual("UpdateStuff", domainTree.Classes[0].Methods[0].Name);
            Assert.AreEqual("ValidationResult", domainTree.Classes[0].Methods[0].ReturnType);

            Assert.AreEqual("UserUpdateStuffEvent", domainTree.Classes[0].Events[0].Name);
            Assert.AreEqual(1, domainTree.Classes[0].Events[0].Properties.Count);
            Assert.AreEqual("UserId", domainTree.Classes[0].Events[0].Properties[0].Name);
            Assert.AreEqual("Guid", domainTree.Classes[0].Events[0].Properties[0].Type);
        }

        [TestMethod]
        public void EventWithProperties_ExceptionAfterTypeDef()
        {
            var tokens = new Collection<DslToken>
            {
                new DslToken(TokenType.DomainClass, "DomainClass", 1),
                new DslToken(TokenType.Value, "User", 1),
                new DslToken(TokenType.ObjectBracketOpen, "{", 1),
                new DslToken(TokenType.Value, "UpdateStuff", 2),
                new DslToken(TokenType.ParameterBracketOpen, "(", 2),
                new DslToken(TokenType.ParameterBracketClose, ")", 2),
                new DslToken(TokenType.TypeDefSeparator, ":", 2),
                new DslToken(TokenType.TypeDefSeparator, ":", 2),
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
        public void EventWithProperties_ExceptionAfterEventOpening()
        {
            var tokens = new Collection<DslToken>
            {
                new DslToken(TokenType.DomainClass, "DomainClass", 1),
                new DslToken(TokenType.Value, "User", 1),
                new DslToken(TokenType.ObjectBracketOpen, "{", 1),
                new DslToken(TokenType.Value, "UpdateStuff", 2),
                new DslToken(TokenType.ParameterBracketOpen, "(", 2),
                new DslToken(TokenType.ParameterBracketClose, ")", 2),
                new DslToken(TokenType.TypeDefSeparator, ":", 2),
                new DslToken(TokenType.ObjectBracketOpen, "{", 2),
                new DslToken(TokenType.ObjectBracketOpen, "{", 2),
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
        public void EventWithProperties_ExceptionAfterEventProp()
        {
            var tokens = new Collection<DslToken>
            {
                new DslToken(TokenType.DomainClass, "DomainClass", 1),
                new DslToken(TokenType.Value, "User", 1),
                new DslToken(TokenType.ObjectBracketOpen, "{", 1),
                new DslToken(TokenType.Value, "UpdateStuff", 2),
                new DslToken(TokenType.ParameterBracketOpen, "(", 2),
                new DslToken(TokenType.ParameterBracketClose, ")", 2),
                new DslToken(TokenType.TypeDefSeparator, ":", 2),
                new DslToken(TokenType.ObjectBracketOpen, "{", 2),
                new DslToken(TokenType.Value, "Name", 2),
                new DslToken(TokenType.Value, "Name", 2),
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
        public void EventWithProperties_ExceptionAfterEventPropTypeDef()
        {
            var tokens = new Collection<DslToken>
            {
                new DslToken(TokenType.DomainClass, "DomainClass", 1),
                new DslToken(TokenType.Value, "User", 1),
                new DslToken(TokenType.ObjectBracketOpen, "{", 1),
                new DslToken(TokenType.Value, "UpdateStuff", 2),
                new DslToken(TokenType.ParameterBracketOpen, "(", 2),
                new DslToken(TokenType.ParameterBracketClose, ")", 2),
                new DslToken(TokenType.TypeDefSeparator, ":", 2),
                new DslToken(TokenType.ObjectBracketOpen, "{", 2),
                new DslToken(TokenType.Value, "Name", 2),
                new DslToken(TokenType.TypeDefSeparator, ":", 2),
                new DslToken(TokenType.TypeDefSeparator, ":", 2),
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
        public void EventWithProperties_ExceptionAfterEventPropFinished()
        {
            var tokens = new Collection<DslToken>
            {
                new DslToken(TokenType.DomainClass, "DomainClass", 1),
                new DslToken(TokenType.Value, "User", 1),
                new DslToken(TokenType.ObjectBracketOpen, "{", 1),
                new DslToken(TokenType.Value, "UpdateStuff", 2),
                new DslToken(TokenType.ParameterBracketOpen, "(", 2),
                new DslToken(TokenType.ParameterBracketClose, ")", 2),
                new DslToken(TokenType.TypeDefSeparator, ":", 2),
                new DslToken(TokenType.ObjectBracketOpen, "{", 2),
                new DslToken(TokenType.Value, "Name", 2),
                new DslToken(TokenType.TypeDefSeparator, ":", 2),
                new DslToken(TokenType.Value, "String", 2),
                new DslToken(TokenType.ObjectBracketOpen, "{", 2),
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
        public void EventWithProperties_EventException()
        {
            var tokens = new Collection<DslToken>
            {
                new DslToken(TokenType.DomainClass, "DomainClass", 1),
                new DslToken(TokenType.Value, "User", 1),
                new DslToken(TokenType.ObjectBracketOpen, "{", 1),
                new DslToken(TokenType.Value, "UpdateStuff", 2),
                new DslToken(TokenType.ParameterBracketOpen, "(", 2),
                new DslToken(TokenType.ParameterBracketClose, ")", 2),
                new DslToken(TokenType.TypeDefSeparator, ":", 2),
                new DslToken(TokenType.TypeDefSeparator, ":", 2),
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
        public void EventWithProperties_Exception()
        {
            var tokens = new Collection<DslToken>
            {
                new DslToken(TokenType.DomainClass, "DomainClass", 1),
                new DslToken(TokenType.Value, "User", 1),
                new DslToken(TokenType.ObjectBracketOpen, "{", 1),
                new DslToken(TokenType.Value, "UpdateStuff", 2),
                new DslToken(TokenType.ParameterBracketOpen, "(", 2),
                new DslToken(TokenType.ParameterBracketClose, ")", 2),
                new DslToken(TokenType.ObjectBracketOpen, "{", 2),
                new DslToken(TokenType.ObjectBracketOpen, "{", 2),
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