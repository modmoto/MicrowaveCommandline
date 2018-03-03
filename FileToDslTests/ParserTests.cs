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
            var parser = new Parser();
            var tokens = new Collection<DslToken>
            {
                new DslToken(TokenType.DomainClass, "DomainClass"),
                new DslToken(TokenType.Value, "User"),
                new DslToken(TokenType.ObjectBracketOpen, "{"),
                new DslToken(TokenType.ObjectBracketClose, "}")
            };
            var domainTree = parser.Parse(tokens);

            Assert.AreEqual(1, domainTree.Classes.Count);
            Assert.AreEqual("User", domainTree.Classes.ToList()[0].Name);
            Assert.AreEqual(0, domainTree.Classes.ToList()[0].Functions.Count);
            Assert.AreEqual(0, domainTree.Classes.ToList()[0].Propteries.Count);
        }
    }
}