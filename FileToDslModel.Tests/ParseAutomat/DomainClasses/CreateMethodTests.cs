using System.Collections.ObjectModel;
using FileToDslModel.Lexer;
using FileToDslModel.ParseAutomat;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FileToDslModel.Tests.ParseAutomat.DomainClasses
{
    [TestClass]
    public class CreateMethodTests
    {
        [TestMethod]
        public void OneParam()
        {
            var tokens = new Collection<DslToken>
            {
                new DslToken(TokenType.DomainClass, "DomainClass", 1),
                new DslToken(TokenType.Value, "User", 1),
                new DslToken(TokenType.ObjectBracketOpen, "{", 1),
                new DslToken(TokenType.CreateMethod, "Create", 2),
                new DslToken(TokenType.ParameterBracketOpen, "(", 2),
                new DslToken(TokenType.Value, "UserName", 2),
                new DslToken(TokenType.TypeDefSeparator, ":", 2),
                new DslToken(TokenType.Value, "String", 2),
                new DslToken(TokenType.ParameterBracketClose, ")", 2),
                new DslToken(TokenType.ObjectBracketClose, "}", 3)
            };

            var parser = new Parser();
            var domainTree = parser.Parse(tokens);

            Assert.AreEqual(1, domainTree.Classes[0].CreateMethods.Count);
            Assert.AreEqual("Create", domainTree.Classes[0].CreateMethods[0].Name);
            Assert.AreEqual(1, domainTree.Classes[0].CreateMethods[0].Parameters.Count);
            Assert.AreEqual("UserName", domainTree.Classes[0].CreateMethods[0].Parameters[0].Name);
            Assert.AreEqual("String", domainTree.Classes[0].CreateMethods[0].Parameters[0].Type);
            Assert.AreEqual("ValidationResult", domainTree.Classes[0].CreateMethods[0].ReturnType);

            Assert.AreEqual("UserCreateEvent", domainTree.Classes[0].Events[0].Name);
            Assert.AreEqual(1, domainTree.Classes[0].Events[0].Properties.Count);
            Assert.AreEqual("User", domainTree.Classes[0].Events[0].Properties[0].Name);
            Assert.AreEqual("User", domainTree.Classes[0].Events[0].Properties[0].Type);
        }

        [TestMethod]
        public void OneParamExceptionTypeDef()
        {
            var tokens = new Collection<DslToken>
            {
                new DslToken(TokenType.DomainClass, "DomainClass", 1),
                new DslToken(TokenType.Value, "User", 1),
                new DslToken(TokenType.ObjectBracketOpen, "{", 1),
                new DslToken(TokenType.CreateMethod, "Create", 2),
                new DslToken(TokenType.ParameterBracketOpen, "(", 2),
                new DslToken(TokenType.Value, "UserName", 2),
                new DslToken(TokenType.TypeDefSeparator, ":", 2),
                new DslToken(TokenType.TypeDefSeparator, ":", 2),
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

        [TestMethod]
        public void OneParam_Exception()
        {
            var tokens = new Collection<DslToken>
            {
                new DslToken(TokenType.DomainClass, "DomainClass", 1),
                new DslToken(TokenType.Value, "User", 1),
                new DslToken(TokenType.ObjectBracketOpen, "{", 1),
                new DslToken(TokenType.CreateMethod, "Create", 2),
                new DslToken(TokenType.ParameterBracketOpen, "(", 2),
                new DslToken(TokenType.ParameterBracketOpen, "(", 2),
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

        [TestMethod]
        public void MutlipleParam()
        {
            var tokens = new Collection<DslToken>
            {
                new DslToken(TokenType.DomainClass, "DomainClass", 1),
                new DslToken(TokenType.Value, "User", 1),
                new DslToken(TokenType.ObjectBracketOpen, "{", 1),
                new DslToken(TokenType.CreateMethod, "Create", 2),
                new DslToken(TokenType.ParameterBracketOpen, "(", 2),
                new DslToken(TokenType.Value, "UserName", 2),
                new DslToken(TokenType.TypeDefSeparator, ":", 2),
                new DslToken(TokenType.Value, "String", 2),
                new DslToken(TokenType.ParamSeparator, ",", 2),
                new DslToken(TokenType.Value, "Age", 2),
                new DslToken(TokenType.TypeDefSeparator, ":", 2),
                new DslToken(TokenType.Value, "Int32", 2),
                new DslToken(TokenType.ParameterBracketClose, ")", 2),
                new DslToken(TokenType.ObjectBracketClose, "}", 3)
            };

            var parser = new Parser();
            var domainTree = parser.Parse(tokens);

            Assert.AreEqual(1, domainTree.Classes[0].CreateMethods.Count);
            Assert.AreEqual("Create", domainTree.Classes[0].CreateMethods[0].Name);
            Assert.AreEqual(2, domainTree.Classes[0].CreateMethods[0].Parameters.Count);
            Assert.AreEqual("UserName", domainTree.Classes[0].CreateMethods[0].Parameters[0].Name);
            Assert.AreEqual("String", domainTree.Classes[0].CreateMethods[0].Parameters[0].Type);
            Assert.AreEqual("Age", domainTree.Classes[0].CreateMethods[0].Parameters[1].Name);
            Assert.AreEqual("Int32", domainTree.Classes[0].CreateMethods[0].Parameters[1].Type);
            Assert.AreEqual("ValidationResult", domainTree.Classes[0].CreateMethods[0].ReturnType);

            Assert.AreEqual("UserCreateEvent", domainTree.Classes[0].Events[0].Name);
            Assert.AreEqual(1, domainTree.Classes[0].Events[0].Properties.Count);
            Assert.AreEqual("User", domainTree.Classes[0].Events[0].Properties[0].Name);
            Assert.AreEqual("User", domainTree.Classes[0].Events[0].Properties[0].Type);
        }

        [TestMethod]
        public void MutlipleParam_TypeDefException()
        {
            var tokens = new Collection<DslToken>
            {
                new DslToken(TokenType.DomainClass, "DomainClass", 1),
                new DslToken(TokenType.Value, "User", 1),
                new DslToken(TokenType.ObjectBracketOpen, "{", 1),
                new DslToken(TokenType.CreateMethod, "Create", 2),
                new DslToken(TokenType.ParameterBracketOpen, "(", 2),
                new DslToken(TokenType.Value, "UserName", 2),
                new DslToken(TokenType.TypeDefSeparator, ":", 2),
                new DslToken(TokenType.Value, "String", 2),
                new DslToken(TokenType.ParamSeparator, ",", 2),
                new DslToken(TokenType.TypeDefSeparator, ":", 2),
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

        [TestMethod]
        public void MutlipleParam_Exception()
        {
            var tokens = new Collection<DslToken>
            {
                new DslToken(TokenType.DomainClass, "DomainClass", 1),
                new DslToken(TokenType.Value, "User", 1),
                new DslToken(TokenType.ObjectBracketOpen, "{", 1),
                new DslToken(TokenType.CreateMethod, "Create", 2),
                new DslToken(TokenType.ParameterBracketOpen, "(", 2),
                new DslToken(TokenType.Value, "UserName", 2),
                new DslToken(TokenType.TypeDefSeparator, ":", 2),
                new DslToken(TokenType.Value, "String", 2),
                new DslToken(TokenType.Value, "String", 2),
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

        [TestMethod]
        public void ExceptionAfterCreateToken()
        {
            var tokens = new Collection<DslToken>
            {
                new DslToken(TokenType.DomainClass, "DomainClass", 1),
                new DslToken(TokenType.Value, "User", 1),
                new DslToken(TokenType.ObjectBracketOpen, "{", 1),
                new DslToken(TokenType.CreateMethod, "Create", 2),
                new DslToken(TokenType.CreateMethod, "Create", 2),
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

        [TestMethod]
        public void ClassWithCreateMethod_EmptyParams()
        {
            var tokens = new Collection<DslToken>
            {
                new DslToken(TokenType.DomainClass, "DomainClass", 1),
                new DslToken(TokenType.Value, "User", 1),
                new DslToken(TokenType.ObjectBracketOpen, "{", 1),
                new DslToken(TokenType.CreateMethod, "Create", 2),
                new DslToken(TokenType.ParameterBracketOpen, "(", 2),
                new DslToken(TokenType.ParameterBracketClose, ")", 2),
                new DslToken(TokenType.ObjectBracketClose, "}", 3)
            };

            var parser = new Parser();
            var domainTree = parser.Parse(tokens);

            Assert.AreEqual(1, domainTree.Classes[0].CreateMethods.Count);
            Assert.AreEqual("Create", domainTree.Classes[0].CreateMethods[0].Name);
            Assert.AreEqual("ValidationResult", domainTree.Classes[0].CreateMethods[0].ReturnType);

            Assert.AreEqual("UserCreateEvent", domainTree.Classes[0].Events[0].Name);
            Assert.AreEqual(1, domainTree.Classes[0].Events[0].Properties.Count);
            Assert.AreEqual("User", domainTree.Classes[0].Events[0].Properties[0].Name);
            Assert.AreEqual("User", domainTree.Classes[0].Events[0].Properties[0].Type);
        }
    }
}