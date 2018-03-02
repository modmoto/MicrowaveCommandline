using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using GenericWebServiceBuilder.DomainSpecificGrammar;

namespace GenericWebServiceBuilder.FileToDSL
{
    internal class DslParser
    {
        public DomainTree Parse(string file)
        {
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
    }
}