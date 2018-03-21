using FileToDslModel.Lexer;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FileToDslModel.Tests.Lexer
{
    [TestClass]
    public class TokenizerTests
    {
        [TestMethod]
        public void Tokenize_Mixed()
        {
            var tokenizer = new Tokenizer();
            var tokens = tokenizer.Tokenize(@"DomainClass User{
	                                Id: Guid
	                                
                                    UpdateName(Name: String): {
		                                UserId: Guid
		                                Age: Int32
	                                }

	                                Posts: [Post]
                                }");
            Assert.AreEqual(27, tokens.Count);

            Assert.AreEqual(TokenType.DomainClass, tokens[0].TokenType);
            Assert.AreEqual(TokenType.Value, tokens[1].TokenType);
            Assert.AreEqual(TokenType.ObjectBracketOpen, tokens[2].TokenType);
            Assert.AreEqual(TokenType.Value, tokens[3].TokenType);
            Assert.AreEqual(TokenType.ParameterBracketOpen, tokens[7].TokenType);
            Assert.AreEqual("UpdateName", tokens[6].Value);
            Assert.AreEqual("Name", tokens[8].Value);
            Assert.AreEqual("String", tokens[10].Value);
        }

        [TestMethod]
        public void Tokenize_LineCounter()
        {
            var tokenizer = new Tokenizer();
            var tokens = tokenizer.Tokenize("DomainClass User{\r\nId: Guid\r\nPosts: [Post]\r\n}");

            Assert.AreEqual(12, tokens.Count);
            Assert.AreEqual(1, tokens[0].LineNumber);
            Assert.AreEqual(1, tokens[1].LineNumber);
            Assert.AreEqual(1, tokens[2].LineNumber);
            Assert.AreEqual(2, tokens[3].LineNumber);
            Assert.AreEqual(2, tokens[4].LineNumber);
            Assert.AreEqual(2, tokens[5].LineNumber);
            Assert.AreEqual(3, tokens[6].LineNumber);
            Assert.AreEqual(3, tokens[7].LineNumber);
            Assert.AreEqual(3, tokens[8].LineNumber);
            Assert.AreEqual(3, tokens[9].LineNumber);
            Assert.AreEqual(3, tokens[10].LineNumber);
            Assert.AreEqual(4, tokens[11].LineNumber);
        }

        [TestMethod]
        public void Tokenize_DomainClass()
        {
            var tokenizer = new Tokenizer();
            var tokens = tokenizer.Tokenize(@"DomainClass User{
                                }");
            Assert.AreEqual(4, tokens.Count);

            Assert.AreEqual(TokenType.DomainClass, tokens[0].TokenType);
            Assert.AreEqual(TokenType.Value, tokens[1].TokenType);
            Assert.AreEqual("User", tokens[1].Value);
            Assert.AreEqual(TokenType.ObjectBracketOpen, tokens[2].TokenType);
            Assert.AreEqual(TokenType.ObjectBracketClose, tokens[3].TokenType);
        }

        [TestMethod]
        public void Tokenize_DomainClass_Property()
        {
            var tokenizer = new Tokenizer();
            var tokens = tokenizer.Tokenize(@"DomainClass User{
                                                Id: Guid
                                            }");
            Assert.AreEqual(7, tokens.Count);

            Assert.AreEqual("Id", tokens[3].Value);
            Assert.AreEqual(TokenType.TypeDefSeparator, tokens[4].TokenType);
            Assert.AreEqual("Guid", tokens[5].Value);
        }

        [TestMethod]
        public void Tokenize_DomainClass_Method()
        {
            var tokenizer = new Tokenizer();
            var tokens = tokenizer.Tokenize(@"DomainClass User{
                                                UpdateName(Name: String): {
		                                            UserId: Guid
		                                            Age: Int32
	                                            }
                                            }");
            Assert.AreEqual(19, tokens.Count);

            Assert.AreEqual("UpdateName", tokens[3].Value);
            Assert.AreEqual(TokenType.ParameterBracketOpen, tokens[4].TokenType);
            Assert.AreEqual(TokenType.TypeDefSeparator, tokens[6].TokenType);
            Assert.AreEqual(TokenType.TypeDefSeparator, tokens[9].TokenType);
            Assert.AreEqual(TokenType.ObjectBracketOpen, tokens[10].TokenType);
            Assert.AreEqual("UserId", tokens[11].Value);
            Assert.AreEqual(TokenType.TypeDefSeparator, tokens[12].TokenType);
            Assert.AreEqual("Guid", tokens[13].Value);
        }

        [TestMethod]
        public void Tokenize_CreateMethod()
        {
            var tokenizer = new Tokenizer();
            var tokens = tokenizer.Tokenize(@"DomainClass User{
                                                Create(Name: String)
                                            }");
            Assert.AreEqual(10, tokens.Count);

            Assert.AreEqual("Create", tokens[3].Value);
            Assert.AreEqual(TokenType.CreateMethod, tokens[3].TokenType);
            Assert.AreEqual(TokenType.ParameterBracketOpen, tokens[4].TokenType);
            Assert.AreEqual(TokenType.TypeDefSeparator, tokens[6].TokenType);
        }

        [TestMethod]
        public void Tokenize_CreateMethod_MultipleParams()
        {
            var tokenizer = new Tokenizer();
            var tokens = tokenizer.Tokenize(@"DomainClass User{
                                                Create(Name: String, Age: Int32)
                                            }");
            Assert.AreEqual(14, tokens.Count);

            Assert.AreEqual("Create", tokens[3].Value);
            Assert.AreEqual(TokenType.CreateMethod, tokens[3].TokenType);
            Assert.AreEqual(TokenType.ParameterBracketOpen, tokens[4].TokenType);
            Assert.AreEqual(TokenType.TypeDefSeparator, tokens[6].TokenType);
            Assert.AreEqual(TokenType.ParamSeparator, tokens[8].TokenType);
            Assert.AreEqual("Age", tokens[9].Value);
            Assert.AreEqual(TokenType.TypeDefSeparator, tokens[10].TokenType);
            Assert.AreEqual("Int32", tokens[11].Value);
        }

        [TestMethod]
        public void Tokenize_DomainHook()
        {
            var tokenizer = new Tokenizer();
            var tokens = tokenizer.Tokenize(@"DomainClass User{
                                                Create()
                                            }
                                            SynchronousDomainHook SendPasswordMail on User.Create");
            Assert.AreEqual(11, tokens.Count);


            Assert.AreEqual(TokenType.SynchronousDomainHook, tokens[7].TokenType);
            Assert.AreEqual(TokenType.Value, tokens[8].TokenType);
            Assert.AreEqual("SendPasswordMail", tokens[8].Value);
            Assert.AreEqual(TokenType.DomainHookOn, tokens[9].TokenType);
            Assert.AreEqual("User.Create", tokens[10].Value);
            Assert.AreEqual(TokenType.DomainHookEventDefinition, tokens[10].TokenType);
        }

        [TestMethod]
        public void Tokenize_ListProperty()
        {
            var tokenizer = new Tokenizer();
            var tokens = tokenizer.Tokenize(@"DomainClass User{
                                                Posts: [Post]
                                            }");
            Assert.AreEqual(9, tokens.Count);

            Assert.AreEqual(TokenType.ListBracketOpen, tokens[5].TokenType);
            Assert.AreEqual(TokenType.Value, tokens[6].TokenType);
            Assert.AreEqual("Post", tokens[6].Value);
            Assert.AreEqual(TokenType.ListBracketClose, tokens[7].TokenType);
        }
    }
}