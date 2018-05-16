using System;
using System.Collections.ObjectModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microwave.LanguageParser.Lexer;
using Microwave.LanguageParser.ParseAutomat;

namespace Microwave.LanguageParser.Tests.ParseAutomat.Members
{
    [TestClass]
    public class LoadFromDbMethodParamTests
    {
        [TestMethod]
        public void LoadFromDbParam()
        {
            var tokens = new Collection<DslToken>
            {
                new DslToken(TokenType.DomainClass, "DomainClass", 1),
                new DslToken(TokenType.Value, "User", 1),
                new DslToken(TokenType.ObjectBracketOpen, "{", 1),
                new DslToken(TokenType.Value, "AddPost", 2),
                new DslToken(TokenType.ParameterBracketOpen, "(", 2),
                new DslToken(TokenType.Value, "NewPost", 2),
                new DslToken(TokenType.TypeDefSeparator, ":", 2),
                new DslToken(TokenType.LoadToken, "@Load", 2),
                new DslToken(TokenType.Value, "Post", 2),
                new DslToken(TokenType.ParameterBracketClose, ")", 2),
                new DslToken(TokenType.TypeDefSeparator, ":", 2),
                new DslToken(TokenType.ObjectBracketOpen, "{", 2),
                new DslToken(TokenType.ObjectBracketClose, "}", 2),
                new DslToken(TokenType.ObjectBracketClose, "}", 3)
            };

            var parser = new MicrowaveLanguageParser();
            var domainTree = parser.Parse(tokens);

            Assert.AreEqual(1, domainTree.Classes[0].Methods.Count);
            Assert.AreEqual(0, domainTree.Classes[0].Properties.Count);
            Assert.AreEqual("AddPost", domainTree.Classes[0].Methods[0].Name);
            Assert.AreEqual(1, domainTree.Classes[0].Methods[0].LoadParameters.Count);
            Assert.AreEqual("NewPost", domainTree.Classes[0].Methods[0].LoadParameters[0].Name);
            Assert.AreEqual("Post", domainTree.Classes[0].Methods[0].LoadParameters[0].Type);
        }

        [TestMethod]
        public void LoadFromDbParam_SecondParameterIsNotALoadParameter()
        {
            var tokens = new Collection<DslToken>
            {
                new DslToken(TokenType.DomainClass, "DomainClass", 1),
                new DslToken(TokenType.Value, "User", 1),
                new DslToken(TokenType.ObjectBracketOpen, "{", 1),
                new DslToken(TokenType.Value, "AddPost", 2),
                new DslToken(TokenType.ParameterBracketOpen, "(", 2),
                new DslToken(TokenType.Value, "NewPost", 2),
                new DslToken(TokenType.TypeDefSeparator, ":", 2),
                new DslToken(TokenType.LoadToken, "@Load", 2),
                new DslToken(TokenType.Value, "Post", 2),
                new DslToken(TokenType.ParamSeparator, ",", 2),
                new DslToken(TokenType.Value, "SecondPost", 2),
                new DslToken(TokenType.TypeDefSeparator, ":", 2),
                new DslToken(TokenType.Value, "Post", 2),
                new DslToken(TokenType.ParameterBracketClose, ")", 2),
                new DslToken(TokenType.TypeDefSeparator, ":", 2),
                new DslToken(TokenType.ObjectBracketOpen, "{", 2),
                new DslToken(TokenType.ObjectBracketClose, "}", 2),
                new DslToken(TokenType.ObjectBracketClose, "}", 3)
            };

            var parser = new MicrowaveLanguageParser();
            var domainTree = parser.Parse(tokens);

            Assert.AreEqual(1, domainTree.Classes[0].Methods.Count);
            Assert.AreEqual(0, domainTree.Classes[0].Properties.Count);
            Assert.AreEqual("AddPost", domainTree.Classes[0].Methods[0].Name);
            Assert.AreEqual(1, domainTree.Classes[0].Methods[0].LoadParameters.Count);
            Assert.AreEqual(1, domainTree.Classes[0].Methods[0].Parameters.Count);
            Assert.AreEqual("SecondPost", domainTree.Classes[0].Methods[0].Parameters[0].Name);
            Assert.AreEqual("NewPost", domainTree.Classes[0].Methods[0].LoadParameters[0].Name);
            Assert.AreEqual("Post", domainTree.Classes[0].Methods[0].Parameters[0].Type);
            Assert.AreEqual("Post", domainTree.Classes[0].Methods[0].LoadParameters[0].Type);
        }

        [TestMethod]
        public void LoadFromDbParam_Error_DoubleLoad()
        {
            var tokens = new Collection<DslToken>
            {
                new DslToken(TokenType.DomainClass, "DomainClass", 1),
                new DslToken(TokenType.Value, "User", 1),
                new DslToken(TokenType.ObjectBracketOpen, "{", 1),
                new DslToken(TokenType.Value, "AddPost", 2),
                new DslToken(TokenType.ParameterBracketOpen, "(", 2),
                new DslToken(TokenType.Value, "NewPost", 2),
                new DslToken(TokenType.TypeDefSeparator, ":", 2),
                new DslToken(TokenType.LoadToken, "@Load", 2),
                new DslToken(TokenType.Value, "Post", 2),
                new DslToken(TokenType.Value, "Post", 2),
            };

            var parser = new MicrowaveLanguageParser();
            try
            {
                parser.Parse(tokens);
            }
            catch (NoTransitionException e)
            {
                Console.WriteLine(e);
                return;
            }

            Assert.Fail();
        }
    }
}