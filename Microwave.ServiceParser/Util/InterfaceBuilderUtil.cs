using System.CodeDom;
using Microwave.LanguageModel.Domain;

namespace Microwave.ServiceParser.Util
{
    public class InterfaceBuilderUtil : IInterfaceBuilder
    {
        public CodeTypeDeclaration BuildForCommand(DomainClass userClass)
        {
            var iface = new CodeTypeDeclaration($"I{userClass.Name}") {IsInterface = true};

            foreach (var function in userClass.Methods)
            {
                var method = new CodeMemberMethod
                {
                    Name = function.Name,
                    ReturnType = new CodeTypeReference(function.ReturnType)
                };

                method.Parameters.Add(new CodeParameterDeclarationExpression
                {
                    Type = new CodeTypeReference($"{userClass.Name}{function.Name}Command"),
                    Name = "command"
                });

                iface.Members.Add(method);
            }

            return iface;
        }

        public CodeTypeDeclaration Build(DomainClass hookClass)
        {
            var iface = new CodeTypeDeclaration($"{hookClass.Name}") {IsInterface = true};

            foreach (var function in hookClass.Methods)
            {
                var method = new CodeMemberMethod
                {
                    Name = function.Name,
                    ReturnType = new CodeTypeReference(function.ReturnType)
                };

                foreach (var parameter in function.Parameters)
                    method.Parameters.Add(new CodeParameterDeclarationExpression
                    {
                        Type = new CodeTypeReference(parameter.Type),
                        Name = parameter.Name
                    });

                iface.Members.Add(method);
            }

            return iface;
        }
    }

    public interface IInterfaceBuilder
    {
        CodeTypeDeclaration BuildForCommand(DomainClass userClass);
    }
}