using System.Collections.ObjectModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microwave.LanguageParser.Lexer;
using Microwave.LanguageParser.ParseAutomat;

namespace Microwave.LanguageParser.Tests.ParseAutomat.Members.Hooks
{
    [TestClass]
    public class OnChildSynchronousHookTests
    {
        [TestMethod]
        public void OnChildSynchronousHook()
        {
            var tokens = new Collection<DslToken>
            {
                new DslToken(TokenType.DomainClass, "DomainClass", 1),
                new DslToken(TokenType.Value, "User", 1),
                new DslToken(TokenType.ObjectBracketOpen, "{", 1),
                new DslToken(TokenType.Value, "CheckAgeRequirement", 1),
                new DslToken(TokenType.OnChild, "OnChild", 1),
                new DslToken(TokenType.DomainHookEventDefinition, "Post.UpdateTitle", 1),
                new DslToken(TokenType.ObjectBracketClose, "}", 2)

            };

            var parser = new MicrowaveLanguageParser();
            var domainTree = parser.Parse(tokens);

            Assert.AreEqual("CheckAgeRequirement_OnPostUpdateTitle", domainTree.Classes[0].ChildHookMethods[0].Name);
            Assert.AreEqual("ValidationResult", domainTree.Classes[0].ChildHookMethods[0].ReturnType);
            Assert.AreEqual("PostUpdateTitle", domainTree.Classes[0].ChildHookMethods[0].Parameters[0].Type);
            Assert.AreEqual("PostUpdateTitle", domainTree.Classes[0].ChildHookMethods[0].Parameters[0].Name);
            Assert.AreEqual(1, domainTree.Classes[0].ChildHookMethods[0].Parameters.Count);
            Assert.AreEqual("Post", domainTree.OnChildHooks[0].ClassType);
            Assert.AreEqual("UpdateTitle", domainTree.OnChildHooks[0].MethodName);
            Assert.AreEqual("CheckAgeRequirement", domainTree.OnChildHooks[0].Name);
        }
    }
}