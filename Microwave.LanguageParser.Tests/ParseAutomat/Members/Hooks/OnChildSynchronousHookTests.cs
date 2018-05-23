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
                new DslToken(TokenType.DomainHookEventDefinition, "PinnedPost.UpdateTitle", 1),
                new DslToken(TokenType.ObjectBracketClose, "}", 2)

            };

            var parser = new MicrowaveLanguageParser();
            var domainTree = parser.Parse(tokens);

            Assert.AreEqual("CheckAgeRequirement", domainTree.Classes[0].ChildHookMethods[0].Name);
            Assert.AreEqual("PinnedPost", domainTree.Classes[0].ChildHookMethods[0].OriginFieldName);
            Assert.AreEqual("User", domainTree.Classes[0].ChildHookMethods[0].ContainingClassName);
            Assert.AreEqual("UpdateTitle", domainTree.Classes[0].ChildHookMethods[0].MethodName);
            Assert.AreEqual("ValidationResult", domainTree.Classes[0].ChildHookMethods[0].ReturnType);
            Assert.AreEqual(0, domainTree.Classes[0].ChildHookMethods[0].Parameters.Count);
        }
    }
}