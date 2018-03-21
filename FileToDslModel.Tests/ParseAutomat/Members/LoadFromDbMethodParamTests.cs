﻿using System.Collections.ObjectModel;
using FileToDslModel.Lexer;
using FileToDslModel.ParseAutomat;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FileToDslModel.Tests.ParseAutomat.Members
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

            var parser = new Parser();
            var domainTree = parser.Parse(tokens);

            Assert.AreEqual(1, domainTree.Classes[0].Methods.Count);
            Assert.AreEqual(0, domainTree.Classes[0].Properties.Count);
            Assert.AreEqual("AddPost", domainTree.Classes[0].Methods[0].Name);
            Assert.AreEqual(1, domainTree.Classes[0].Methods[0].LoadParameters.Count);
            Assert.AreEqual("NewPost", domainTree.Classes[0].Methods[0].LoadParameters[0].Name);
            Assert.AreEqual("Post", domainTree.Classes[0].Methods[0].LoadParameters[0].Type);
        }
    }
}