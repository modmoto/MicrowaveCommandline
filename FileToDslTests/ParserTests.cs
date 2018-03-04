using System.Collections.ObjectModel;
using System.Linq;
using GenericWebServiceBuilder.FileToDslModel.Lexer;
using GenericWebServiceBuilder.FileToDslModel.ParseAutomat;
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

            var parser = new Parser();
            var domainTree = parser.Parse(tokens);

            Assert.AreEqual(1, domainTree.Classes.Count);
            Assert.AreEqual("User", domainTree.Classes.ToList()[0].Name);
            Assert.AreEqual(0, domainTree.Classes.ToList()[0].Methods.Count);
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

            var parser = new Parser();
            var domainTree = parser.Parse(tokens);

            Assert.AreEqual(1, domainTree.Classes.Count);
            Assert.AreEqual("User", domainTree.Classes.ToList()[0].Name);
            Assert.AreEqual(0, domainTree.Classes.ToList()[0].Methods.Count);
            Assert.AreEqual(1, domainTree.Classes.ToList()[0].Propteries.Count);
            Assert.AreEqual("VorName", domainTree.Classes.ToList()[0].Propteries.ToList()[0].Name);
            Assert.AreEqual("String", domainTree.Classes.ToList()[0].Propteries.ToList()[0].Type);
        }

        [TestMethod]
        public void Parse_ClassWithMethod_AllEmpty()
        {
            var tokens = new Collection<DslToken>
            {
                new DslToken(TokenType.DomainClass, "DomainClass", 1),
                new DslToken(TokenType.Value, "User", 1),
                new DslToken(TokenType.ObjectBracketOpen, "{", 1),
                new DslToken(TokenType.Value, "CreateUser", 2),
                new DslToken(TokenType.ParameterBracketOpen, "(", 2),
                new DslToken(TokenType.ParameterBracketClose, ")", 2),
                new DslToken(TokenType.TypeDefSeparator, ":", 2),
                new DslToken(TokenType.ObjectBracketOpen, "{", 2),
                new DslToken(TokenType.ObjectBracketClose, "}", 2),
                new DslToken(TokenType.ObjectBracketClose, "}", 3)
            };

            var parser = new Parser();
            var domainTree = parser.Parse(tokens);

            Assert.AreEqual(1, domainTree.Classes.ToList()[0].Methods.Count);
            Assert.AreEqual(0, domainTree.Classes.ToList()[0].Propteries.Count);
            Assert.AreEqual("CreateUser", domainTree.Classes.ToList()[0].Methods.ToList()[0].Name);
            Assert.AreEqual("CreateUserEvent", domainTree.Classes.ToList()[0].Methods.ToList()[0].ReturnType);

            Assert.AreEqual("CreateUserEvent", domainTree.Events.ToList()[0].Name);
            Assert.AreEqual(0, domainTree.Events.ToList()[0].Properties.Count);
        }

        [TestMethod]
        public void Parse_ClassWithMethod_SingleParameter()
        {
            var tokens = new Collection<DslToken>
            {
                new DslToken(TokenType.DomainClass, "DomainClass", 1),
                new DslToken(TokenType.Value, "User", 1),
                new DslToken(TokenType.ObjectBracketOpen, "{", 1),
                new DslToken(TokenType.Value, "CreateUser", 2),
                new DslToken(TokenType.ParameterBracketOpen, "(", 2),
                new DslToken(TokenType.Value, "UserName", 2),
                new DslToken(TokenType.TypeDefSeparator, ":", 2),
                new DslToken(TokenType.Value, "String", 2),
                new DslToken(TokenType.ParameterBracketClose, ")", 2),
                new DslToken(TokenType.TypeDefSeparator, ":", 2),
                new DslToken(TokenType.ObjectBracketOpen, "{", 2),
                new DslToken(TokenType.ObjectBracketClose, "}", 2),
                new DslToken(TokenType.ObjectBracketClose, "}", 3)
            };

            var parser = new Parser();
            var domainTree = parser.Parse(tokens);

            Assert.AreEqual(1, domainTree.Classes.ToList()[0].Methods.Count);
            Assert.AreEqual(0, domainTree.Classes.ToList()[0].Propteries.Count);
            Assert.AreEqual("CreateUser", domainTree.Classes.ToList()[0].Methods.ToList()[0].Name);
            Assert.AreEqual(1, domainTree.Classes.ToList()[0].Methods.ToList()[0].Parameters.Count);
            Assert.AreEqual("UserName", domainTree.Classes.ToList()[0].Methods.ToList()[0].Parameters.ToList()[0].Name);
            Assert.AreEqual("String", domainTree.Classes.ToList()[0].Methods.ToList()[0].Parameters.ToList()[0].Type);
            Assert.AreEqual("CreateUserEvent", domainTree.Classes.ToList()[0].Methods.ToList()[0].ReturnType);

            Assert.AreEqual("CreateUserEvent", domainTree.Events.ToList()[0].Name);
            Assert.AreEqual(0, domainTree.Events.ToList()[0].Properties.Count);
        }

        [TestMethod]
        public void Parse_ClassWithMethod_MultipleParameters()
        {
            var tokens = new Collection<DslToken>
            {
                new DslToken(TokenType.DomainClass, "DomainClass", 1),
                new DslToken(TokenType.Value, "User", 1),
                new DslToken(TokenType.ObjectBracketOpen, "{", 1),
                new DslToken(TokenType.Value, "CreateUser", 2),
                new DslToken(TokenType.ParameterBracketOpen, "(", 2),
                new DslToken(TokenType.Value, "UserName", 2),
                new DslToken(TokenType.TypeDefSeparator, ":", 2),
                new DslToken(TokenType.Value, "String", 2),
                new DslToken(TokenType.ParamSeparator, ",", 2),
                new DslToken(TokenType.Value, "Age", 2),
                new DslToken(TokenType.TypeDefSeparator, ":", 2),
                new DslToken(TokenType.Value, "Int32", 2),
                new DslToken(TokenType.ParameterBracketClose, ")", 2),
                new DslToken(TokenType.TypeDefSeparator, ":", 2),
                new DslToken(TokenType.ObjectBracketOpen, "{", 2),
                new DslToken(TokenType.ObjectBracketClose, "}", 2),
                new DslToken(TokenType.ObjectBracketClose, "}", 3)
            };

            var parser = new Parser();
            var domainTree = parser.Parse(tokens);

            Assert.AreEqual(1, domainTree.Classes.ToList()[0].Methods.Count);
            Assert.AreEqual("CreateUser", domainTree.Classes.ToList()[0].Methods.ToList()[0].Name);
            Assert.AreEqual(2, domainTree.Classes.ToList()[0].Methods.ToList()[0].Parameters.Count);
            Assert.AreEqual("UserName", domainTree.Classes.ToList()[0].Methods.ToList()[0].Parameters.ToList()[0].Name);
            Assert.AreEqual("String", domainTree.Classes.ToList()[0].Methods.ToList()[0].Parameters.ToList()[0].Type);
            Assert.AreEqual("Age", domainTree.Classes.ToList()[0].Methods.ToList()[0].Parameters.ToList()[1].Name);
            Assert.AreEqual("Int32", domainTree.Classes.ToList()[0].Methods.ToList()[0].Parameters.ToList()[1].Type);
            Assert.AreEqual("CreateUserEvent", domainTree.Classes.ToList()[0].Methods.ToList()[0].ReturnType);

            Assert.AreEqual("CreateUserEvent", domainTree.Events.ToList()[0].Name);
            Assert.AreEqual(0, domainTree.Events.ToList()[0].Properties.Count);
        }

        [TestMethod]
        public void Parse_ClassWithMethod_EventWithProperties()
        {
            var tokens = new Collection<DslToken>
            {
                new DslToken(TokenType.DomainClass, "DomainClass", 1),
                new DslToken(TokenType.Value, "User", 1),
                new DslToken(TokenType.ObjectBracketOpen, "{", 1),
                new DslToken(TokenType.Value, "CreateUser", 2),
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

            var parser = new Parser();
            var domainTree = parser.Parse(tokens);

            Assert.AreEqual(1, domainTree.Classes.ToList()[0].Methods.Count);
            Assert.AreEqual("CreateUser", domainTree.Classes.ToList()[0].Methods.ToList()[0].Name);
            Assert.AreEqual("CreateUserEvent", domainTree.Classes.ToList()[0].Methods.ToList()[0].ReturnType);

            Assert.AreEqual("CreateUserEvent", domainTree.Events.ToList()[0].Name);
            Assert.AreEqual(1, domainTree.Events.ToList()[0].Properties.Count);
            Assert.AreEqual("UserId", domainTree.Events.ToList()[0].Properties.ToList()[0].Name);
            Assert.AreEqual("Guid", domainTree.Events.ToList()[0].Properties.ToList()[0].Type);
        }
    }
}