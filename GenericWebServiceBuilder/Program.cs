using System.IO;
using GenericWebServiceBuilder.DSL;
using GenericWebServiceBuilder.Parsing;

namespace GenericWebServiceBuilder
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            using (var sr = new StreamReader("Schema.wsb"))
            {
                var userClass = new DomainClass
                {
                    Name = "User",
                    Functions =
                    {
                        new DomainFunction
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
                        new DomainFunction
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
                };

                var writer = new DomainClassWriter(new InterfaceParser(), new PropertyParser(), new ClassParser());
                writer.Write(userClass);
            }
        }
    }
}