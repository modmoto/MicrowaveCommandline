using GenericWebServiceBuilder.DSL;
using Microsoft.CSharp;
using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.IO;
using System.Reflection;

namespace GenericWebServiceBuilder
{
    class Program
    {
        static void Main(string[] args)
        {
            using (StreamReader sr = new StreamReader("Schema.wsb"))
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
                                    Type = typeof(String),
                                    Name = "Name"
                                }
                            }
                        }
                    },
                    Propteries =
                    {
                        new Property
                        {
                            Type = typeof(String),
                            Name = "Name"
                        },
                        new Property
                        {
                            Type = typeof(int),
                            Name = "Age"
                        },
                    },
                };


                CodeNamespace ns = new CodeNamespace("Domain");

                var iface = new CodeTypeDeclaration($"I{userClass.Name}");
                iface.IsInterface = true;
                var mth = new CodeMemberMethod();
                mth.Name = userClass.Functions[0].Name;
                mth.ReturnType = new CodeTypeReference(userClass.Functions[0].ReturnType);
                mth.Parameters.Add(new CodeParameterDeclarationExpression(userClass.Functions[0].Parameters[0].Type, userClass.Functions[0].Parameters[0].Name));
                iface.Members.Add(mth);
                ns.Types.Add(iface);

                CodeMemberField widthValueField = new CodeMemberField();
                widthValueField.Attributes = MemberAttributes.Private;
                widthValueField.Name = $"_{userClass.Propteries[0].Name}";
                widthValueField.Type = new CodeTypeReference(userClass.Propteries[0].Type);

                CodeMemberProperty widthProperty = new CodeMemberProperty();
                widthProperty.Attributes = MemberAttributes.Public | MemberAttributes.Final;
                widthProperty.Name = userClass.Propteries[0].Name;
                widthProperty.HasGet = true;
                widthProperty.Type = new CodeTypeReference(userClass.Propteries[0].Type);
                widthProperty.GetStatements.Add(new CodeMethodReturnStatement(
                    new CodeFieldReferenceExpression(
                    new CodeThisReferenceExpression(), $"_{userClass.Propteries[0].Name}")));

                var targetUnit = new CodeCompileUnit();
                var targetClass = new CodeTypeDeclaration(userClass.Name);
                targetClass.IsClass = true;
                targetClass.IsPartial = true;
                targetClass.TypeAttributes = TypeAttributes.Public;
                ns.Types.Add(targetClass);
                targetUnit.Namespaces.Add(ns);

                targetClass.Members.Add(widthProperty);
                targetClass.Members.Add(widthValueField);
                targetClass.BaseTypes.Add(iface.Name);
                

                CSharpCodeProvider provider = new CSharpCodeProvider();
                CodeGeneratorOptions options = new CodeGeneratorOptions();
                options.BracingStyle = "C";
                using (StreamWriter sourceWriter = new StreamWriter($"Domain/Generated/{userClass.Name}.g.cs"))
                {
                    provider.GenerateCodeFromCompileUnit(targetUnit, sourceWriter, options);
                }
            }
        }
    }
}
