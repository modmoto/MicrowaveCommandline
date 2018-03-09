using System.CodeDom;
using DslModel.Domain;

namespace DslModelToCSharp
{
    public class InterfaceBuilder : IInterfaceBuilder
    {
        public CodeTypeDeclaration Build(DomainClass userClass)
        {
            var iface = new CodeTypeDeclaration($"I{userClass.Name}") {IsInterface = true};

            foreach (var function in userClass.Methods)
            {
                var method = new CodeMemberMethod
                {
                    Name = function.Name,
                    ReturnType = new CodeTypeReference(function.ReturnType)
                };

                foreach (var parameter in function.Parameters)
                    method.Parameters.Add(new CodeParameterDeclarationExpression(parameter.Type, parameter.Name));

                iface.Members.Add(method);
            }

            return iface;
        }
    }

    public interface IInterfaceBuilder
    {
        CodeTypeDeclaration Build(DomainClass userClass);
    }
}