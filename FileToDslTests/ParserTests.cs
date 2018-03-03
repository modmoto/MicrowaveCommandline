using System.Collections.ObjectModel;
using System.Linq;
using GenericWebServiceBuilder.FileToDSL.Lexer;
using GenericWebServiceBuilder.FileToDSL.ParseAutomat;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FileToDslTests
{
    [TestClass]
    public class ParserTests
    {
        [TestMethod]
        public void Parse_EmptyClass()
        {
            
            var tokens = new Collection<DslToken>
            {
                new DslToken(TokenType.DomainClass, "DomainClass", 1),
                new DslToken(TokenType.Value, "User", 1),
                new DslToken(TokenType.ObjectBracketOpen, "{", 1),
                new DslToken(TokenType.ObjectBracketClose, "}", 2)
            };

            var parser = new Parser(tokens);
            var domainTree = parser.Parse();

            Assert.AreEqual(1, domainTree.Classes.Count);
            Assert.AreEqual("User", domainTree.Classes.ToList()[0].Name);
            Assert.AreEqual(0, domainTree.Classes.ToList()[0].Functions.Count);
            Assert.AreEqual(0, domainTree.Classes.ToList()[0].Propteries.Count);
        }

        [TestMethod]
        public void Parse_ClassWithProperty()
        {

            var tokens = new Collection<DslToken>
            {
                new DslToken(TokenType.DomainClass, "DomainClass", 1),
                new DslToken(TokenType.Value, "User", 1),
                new DslToken(TokenType.ObjectBracketOpen, "{", 1),
                new DslToken(TokenType.Value, "VorName", 2),
                new DslToken(TokenType.TypeDefSeparator, ":", 2),
                new DslToken(TokenType.Value, "String", 2),
                new DslToken(TokenType.ObjectBracketClose, "}", 3)
            };

            var parser = new Parser(tokens);
            var domainTree = parser.Parse();

            Assert.AreEqual(1, domainTree.Classes.Count);
            Assert.AreEqual("User", domainTree.Classes.ToList()[0].Name);
            Assert.AreEqual(0, domainTree.Classes.ToList()[0].Functions.Count);
            Assert.AreEqual(1, domainTree.Classes.ToList()[0].Propteries.Count);
            Assert.AreEqual("VorName", domainTree.Classes.ToList()[0].Propteries.ToList()[0].Name);
            Assert.AreEqual("String", domainTree.Classes.ToList()[0].Propteries.ToList()[0].Type);
        }
    }
}