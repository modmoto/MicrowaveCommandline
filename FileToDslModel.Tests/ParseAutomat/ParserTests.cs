using System.Collections.ObjectModel;
using FileToDslModel.Lexer;
using FileToDslModel.ParseAutomat;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FileToDslModel.Tests.ParseAutomat
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
            Assert.AreEqual("User", domainTree.Classes[0].Name);
            Assert.AreEqual(0, domainTree.Classes[0].Methods.Count);
            Assert.AreEqual(0, domainTree.Classes[0].Properties.Count);
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
            Assert.AreEqual("User", domainTree.Classes[0].Name);
            Assert.AreEqual(0, domainTree.Classes[0].Methods.Count);
            Assert.AreEqual(1, domainTree.Classes[0].Properties.Count);
            Assert.AreEqual("VorName", domainTree.Classes[0].Properties[0].Name);
            Assert.AreEqual("String", domainTree.Classes[0].Properties[0].Type);
        }

        [TestMethod]
        public void Parse_ClassWithMethod_AllEmpty()
        {
            var tokens = new Collection<DslToken>
            {
                new DslToken(TokenType.DomainClass, "DomainClass", 1),
                new DslToken(TokenType.Value, "User", 1),
                new DslToken(TokenType.ObjectBracketOpen, "{", 1),
                new DslToken(TokenType.Value, "Disable", 2),
                new DslToken(TokenType.ParameterBracketOpen, "(", 2),
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
            Assert.AreEqual("Disable", domainTree.Classes[0].Methods[0].Name);
            Assert.AreEqual("ValidationResult", domainTree.Classes[0].Methods[0].ReturnType);

            Assert.AreEqual("UserDisableEvent", domainTree.Classes[0].Events[0].Name);
            Assert.AreEqual(0, domainTree.Classes[0].Events[0].Properties.Count);
        }

        [TestMethod]
        public void Parse_ClassWithMethod_SingleParameter()
        {
            var tokens = new Collection<DslToken>
            {
                new DslToken(TokenType.DomainClass, "DomainClass", 1),
                new DslToken(TokenType.Value, "User", 1),
                new DslToken(TokenType.ObjectBracketOpen, "{", 1),
                new DslToken(TokenType.Value, "UpdateStuff", 2),
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

            Assert.AreEqual(1, domainTree.Classes[0].Methods.Count);
            Assert.AreEqual(0, domainTree.Classes[0].Properties.Count);
            Assert.AreEqual("UpdateStuff", domainTree.Classes[0].Methods[0].Name);
            Assert.AreEqual(1, domainTree.Classes[0].Methods[0].Parameters.Count);
            Assert.AreEqual("UserName", domainTree.Classes[0].Methods[0].Parameters[0].Name);
            Assert.AreEqual("String", domainTree.Classes[0].Methods[0].Parameters[0].Type);
            Assert.AreEqual("ValidationResult", domainTree.Classes[0].Methods[0].ReturnType);

            Assert.AreEqual("UserUpdateStuffEvent", domainTree.Classes[0].Events[0].Name);
            Assert.AreEqual(0, domainTree.Classes[0].Events[0].Properties.Count);
        }

        [TestMethod]
        public void Parse_ClassWithMethod_MultipleParameters()
        {
            var tokens = new Collection<DslToken>
            {
                new DslToken(TokenType.DomainClass, "DomainClass", 1),
                new DslToken(TokenType.Value, "User", 1),
                new DslToken(TokenType.ObjectBracketOpen, "{", 1),
                new DslToken(TokenType.Value, "UpdateStuff", 2),
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

            Assert.AreEqual(1, domainTree.Classes[0].Methods.Count);
            Assert.AreEqual("UpdateStuff", domainTree.Classes[0].Methods[0].Name);
            Assert.AreEqual(2, domainTree.Classes[0].Methods[0].Parameters.Count);
            Assert.AreEqual("UserName", domainTree.Classes[0].Methods[0].Parameters[0].Name);
            Assert.AreEqual("String", domainTree.Classes[0].Methods[0].Parameters[0].Type);
            Assert.AreEqual("Age", domainTree.Classes[0].Methods[0].Parameters[1].Name);
            Assert.AreEqual("Int32", domainTree.Classes[0].Methods[0].Parameters[1].Type);
            Assert.AreEqual("ValidationResult", domainTree.Classes[0].Methods[0].ReturnType);

            Assert.AreEqual("UserUpdateStuffEvent", domainTree.Classes[0].Events[0].Name);
            Assert.AreEqual(0, domainTree.Classes[0].Events[0].Properties.Count);
        }

        [TestMethod]
        public void Parse_ClassWithMethod_EventWithProperties()
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

            var parser = new Parser();
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
        public void Parse_ClassWithCreateMethod_EmptyParams()
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

        [TestMethod]
        public void Parse_ClassWithCreateMethod_OneParam()
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
        public void Parse_ClassWithCreateMethod_MutlipleParam()
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
        public void Parse_ClassWithListProperty()
        {
            var tokens = new Collection<DslToken>
            {
                new DslToken(TokenType.DomainClass, "DomainClass", 1),
                new DslToken(TokenType.Value, "User", 1),
                new DslToken(TokenType.ObjectBracketOpen, "{", 1),
                new DslToken(TokenType.Value, "Posts", 2),
                new DslToken(TokenType.TypeDefSeparator, ":", 2),
                new DslToken(TokenType.ListBracketOpen, "[", 2),
                new DslToken(TokenType.Value, "Post", 2),
                new DslToken(TokenType.ListBracketClose, "]", 2),
                new DslToken(TokenType.ObjectBracketClose, "}", 3)
            };

            var parser = new Parser();
            var domainTree = parser.Parse(tokens);

            Assert.AreEqual(1, domainTree.Classes[0].ListProperties.Count);
            Assert.AreEqual("Posts", domainTree.Classes[0].ListProperties[0].Name);
            Assert.AreEqual("Post", domainTree.Classes[0].ListProperties[0].Type);
        }

        [TestMethod]
        public void Parse_SynchronousHook()
        {
            var tokens = new Collection<DslToken>
            {
                new DslToken(TokenType.SynchronousDomainHook, "SynchronousDomainHook", 1),
                new DslToken(TokenType.Value, "SendPasswordMail", 1),
                new DslToken(TokenType.DomainHookOn, "on", 1),
                new DslToken(TokenType.DomainHookEventDefinition, "User.Create", 1),
            };

            var parser = new Parser();
            var domainTree = parser.Parse(tokens);

            Assert.AreEqual("User", domainTree.SynchronousDomainHooks[0].ClassType);
            Assert.AreEqual("Create", domainTree.SynchronousDomainHooks[0].MethodName);
            Assert.AreEqual("SendPasswordMail", domainTree.SynchronousDomainHooks[0].Name);
        }
    }
}