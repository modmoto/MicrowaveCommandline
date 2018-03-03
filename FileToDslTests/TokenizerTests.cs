using GenericWebServiceBuilder.FileToDSL.Lexer;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FileToDslTests
{
    [TestClass]
    public class TokenizerTests
    {
        [TestMethod]
        public void Tokenize_Mixed()
        {
            var tokenizer = new Tokenizer();
            var tokens = tokenizer.Tokenize(@"DomainClass User{
	                                Id: Guid;
	                                
                                    UpdateName(Name: String): UpdateNameEvent;

	                                Posts: [Post];
                                }");
            Assert.AreEqual(24, tokens.Count);

            Assert.AreEqual(TokenType.DomainClass, tokens[0].TokenType); 
            Assert.AreEqual(TokenType.Value, tokens[1].TokenType); 
            Assert.AreEqual(TokenType.OpenParenthesis, tokens[2].TokenType); 
            Assert.AreEqual(TokenType.Value, tokens[3].TokenType); 
            Assert.AreEqual(TokenType.Value, tokens[7].TokenType); 
            Assert.AreEqual("UpdateName", tokens[7].Value);
            Assert.AreEqual("Name", tokens[9].Value);
            Assert.AreEqual("String", tokens[11].Value);
        }

        [TestMethod]
        public void Tokenize_DomainClass()
        {
            var tokenizer = new Tokenizer();
            var tokens = tokenizer.Tokenize(@"DomainClass User{
                                }");
            Assert.AreEqual(5, tokens.Count);

            Assert.AreEqual(TokenType.DomainClass, tokens[0].TokenType);
            Assert.AreEqual(TokenType.Value, tokens[1].TokenType);
            Assert.AreEqual("User", tokens[1].Value);
            Assert.AreEqual(TokenType.OpenParenthesis, tokens[2].TokenType);
            Assert.AreEqual(TokenType.CloseParenthesis, tokens[3].TokenType);
        }

        [TestMethod]
        public void Tokenize_DomainEvent()
        {
            var tokenizer = new Tokenizer();
            var tokens = tokenizer.Tokenize(@"DomainEvent CreateUser{
                                }");
            Assert.AreEqual(5, tokens.Count);

            Assert.AreEqual(TokenType.DomainEvent, tokens[0].TokenType);
            Assert.AreEqual(TokenType.Value, tokens[1].TokenType);
            Assert.AreEqual("CreateUser", tokens[1].Value);
            Assert.AreEqual(TokenType.OpenParenthesis, tokens[2].TokenType);
            Assert.AreEqual(TokenType.CloseParenthesis, tokens[3].TokenType);
        }

        [TestMethod]
        public void Tokenize_DomainClass_Property()
        {
            var tokenizer = new Tokenizer();
            var tokens = tokenizer.Tokenize(@"DomainClass User{
                                                Id: Guid;
                                            }");
            Assert.AreEqual(9, tokens.Count);
            
            Assert.AreEqual("Id", tokens[3].Value);
            Assert.AreEqual(TokenType.TypeDef, tokens[4].TokenType);
            Assert.AreEqual("Guid", tokens[5].Value);
            Assert.AreEqual(TokenType.PropertyDefEnd, tokens[6].TokenType);
        }
    }
}
