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
                            Type = typeof(Guid),
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
                            Type = typeof(Guid),
                            Name = "UserId",
                        },
                        new Property
                        {
                            Type = typeof(int),
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
                            ReturnType = typeof(UserCreateEvent),
                            Parameters =
                            {
                                new Parameter
                                {
                                    Type = typeof(string),
                                    Name = "Name"
                                }
                            }
                        },
                        new DomainMethod
                        {
                            Name = "UpdateAge",
                            ReturnType = typeof(UserCreateEvent),
                            Parameters =
                            {
                                new Parameter
                                {
                                    Type = typeof(int),
                                    Name = "Age"
                                }
                            }
                        }
                    },
                    Propteries =
                    {
                        new Property
                        {
                            Type = typeof(string),
                            Name = "Name"
                        },
                        new Property
                        {
                            Type = typeof(int),
                            Name = "Age"
                        }
                    }
                }
            };

            return new DomainTree(classes, events);
        }
    }
}