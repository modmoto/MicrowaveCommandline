using System.Collections.Generic;
using System.Collections.ObjectModel;
using GenericWebServiceBuilder.DomainSpecificGrammar;
using GenericWebServiceBuilder.FileToDSL.Lexer;

namespace GenericWebServiceBuilder.FileToDSL
{
    public class DslParser
    {
        private readonly ITokenizer _tokenizer;

        public DslParser(ITokenizer tokenizer)
        {
            _tokenizer = tokenizer;
        }

        public DomainTree Parse(string file)
        {
            var dslTokens = _tokenizer.Tokenize(file);

            var events = new List<DomainEvent>
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
                        }
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
                            Name = "UserId"
                        },
                        new Property
                        {
                            Type = "Int32",
                            Name = "Age"
                        }
                    }
                }
            };

            var classes = new List<DomainClass>
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
    }
}