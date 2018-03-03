using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using GenericWebServiceBuilder.DomainSpecificGrammar;

namespace GenericWebServiceBuilder.FileToDSL
{
    internal class DslParser
    {
        public DomainTree Parse(string file)
        {
            var tokenizer = new Tokenizer();
            var dslTokens = tokenizer.Tokenize(file);

            ICollection<DomainEvent> events = new Collection<DomainEvent>
            {
                new DomainEvent
                {
                    Name = "UserCreateEvent",
                    Properties =
                    {
                        new Property
                        {
                            Type = "Guid",
                            Name = "UserId"
                        },
                    }
                },
                new DomainEvent
                {
                    Name = "UpdateAgeEvent",
                    Properties =
                    {
                        new Property
                        {
                            Type = "Guid",
                            Name = "UserId",
                        },
                        new Property
                        {
                            Type = "Int32",
                            Name = "Age",
                        },
                    }
                }
            };

            ICollection<DomainClass> classes = new Collection<DomainClass>
            {
                new DomainClass
                {
                    Name = "User",
                    Functions =
                    {
                        new DomainMethod
                        {
                            Name = "Create",
                            ReturnType = "UserCreateEvent",
                            Parameters =
                            {
                                new Parameter
                                {
                                    Type = "String",
                                    Name = "Name"
                                }
                            }
                        },
                        new DomainMethod
                        {
                            Name = "UpdateAge",
                            ReturnType = "UpdateAgeEvent",
                            Parameters =
                            {
                                new Parameter
                                {
                                    Type = "Int32",
                                    Name = "Age"
                                }
                            }
                        }
                    },
                    Propteries =
                    {
                        new Property
                        {
                            Type = "String",
                            Name = "Name"
                        },
                        new Property
                        {
                            Type = "Int32",
                            Name = "Age"
                        }
                    }
                }
            };

            return new DomainTree(classes, events);
        }

        public enum TokenType
        {
            NotDefined,
            OpenParenthesis,
            CloseParenthesis,
            DomainFunction,
            DomainClass,
            DomainEvent,
            ListBracketOpen,
            ListBracketClose,
            Value,
            SequenceTerminator,
            ParameterBracketOpen,
            ParameterBracketClose,
            TypeNameDef,
            TypeName,
            TypeDef,
            LineEnd
        }

        public class TokenDefinition
        {
            private Regex _regex;
            private readonly TokenType _returnsToken;

            public TokenDefinition(TokenType returnsToken, string regexPattern)
            {
                _regex = new Regex(regexPattern, RegexOptions.IgnoreCase);
                _returnsToken = returnsToken;
            }

            public TokenMatch Match(string inputString)
            {
                var match = _regex.Match(inputString);
                if (match.Success)
                {
                    string remainingText = string.Empty;
                    if (match.Length != inputString.Length)
                        remainingText = inputString.Substring(match.Length);

                    return new TokenMatch
                    {
                        IsMatch = true,
                        RemainingText = remainingText,
                        TokenType = _returnsToken,
                        Value = match.Value
                    };
                }
                else
                {
                    return new TokenMatch { IsMatch = false };
                }

            }
        }

        public class TokenMatch
        {
            public bool IsMatch { get; set; }
            public TokenType TokenType { get; set; }
            public string Value { get; set; }
            public string RemainingText { get; set; }
        }

        public class DslToken
        {
            public DslToken(TokenType tokenType)
            {
                TokenType = tokenType;
                Value = string.Empty;
            }

            public DslToken(TokenType tokenType, string value)
            {
                TokenType = tokenType;
                Value = value;
            }

            public TokenType TokenType { get; set; }
            public string Value { get; set; }
        }

        public class Tokenizer{
            private List<TokenDefinition> _tokenDefinitions;

            public Tokenizer()
            {
                _tokenDefinitions = new List<TokenDefinition>
                {
                    new TokenDefinition(TokenType.OpenParenthesis, "^\\{"),
                    new TokenDefinition(TokenType.CloseParenthesis, "^\\}"),

                    new TokenDefinition(TokenType.ParameterBracketOpen, "^\\("),
                    new TokenDefinition(TokenType.ParameterBracketClose, "^\\)"),

                    new TokenDefinition(TokenType.TypeDef, "^\\:"),
                    new TokenDefinition(TokenType.LineEnd, "^\\;"),

                    new TokenDefinition(TokenType.DomainFunction, "^DomainFunction"),
                    new TokenDefinition(TokenType.DomainClass, "^DomainClass"),
                    new TokenDefinition(TokenType.DomainEvent, "^DomainEvent"),

                    new TokenDefinition(TokenType.Value, "^\\w+"),
                };

            }

            public List<DslToken> Tokenize(string lqlText)
            {
                var tokens = new List<DslToken>();
                string remainingText = lqlText;

                while (!string.IsNullOrWhiteSpace(remainingText))
                {
                    var match = FindMatch(remainingText);
                    if (match.IsMatch)
                    {
                        tokens.Add(new DslToken(match.TokenType, match.Value));
                        remainingText = match.RemainingText;
                    }
                    else
                    {
                        remainingText = remainingText.Substring(1);
                    }
                }

                tokens.Add(new DslToken(TokenType.SequenceTerminator, string.Empty));

                return tokens;
            }

            private TokenMatch FindMatch(string lqlText)
            {
                foreach (var tokenDefinition in _tokenDefinitions)
                {
                    var match = tokenDefinition.Match(lqlText);
                    if (match.IsMatch)
                        return match;
                }

                return new TokenMatch() { IsMatch = false };
            }
        }
    }
}