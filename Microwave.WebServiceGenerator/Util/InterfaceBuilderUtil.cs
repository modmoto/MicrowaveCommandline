using System.CodeDom;
using System.Reflection;
using Microwave.LanguageModel;

namespace Microwave.WebServiceGenerator.Util
{
    public class InterfaceBuilderUtil : IInterfaceBuilder
    {
        private NameBuilderUtil _nameBuilderUtil;

        public InterfaceBuilderUtil()
        {
            _nameBuilderUtil = new NameBuilderUtil();
        }

        public CodeTypeDeclaration BuildForCommand(DomainClass generatedClass)
        {
            var iface = new CodeTypeDeclaration($"{generatedClass.Name}Base");
            iface.TypeAttributes = TypeAttributes.Abstract | TypeAttributes.Public;

            foreach (var function in generatedClass.Methods)
            {
                var method = new CodeMemberMethod
                {
                    Name = function.Name,
                    ReturnType = new CodeTypeReference(function.ReturnType),
                    Attributes = MemberAttributes.Abstract | MemberAttributes.Public
                };

                method.Parameters.Add(new CodeParameterDeclarationExpression
                {
                    Type = new CodeTypeReference($"{generatedClass.Name}{function.Name}Command"),
                    Name = "command"
                });

                iface.Members.Add(method);
            }

            foreach (var function in generatedClass.ChildHookMethods)
            {
                var method = new CodeMemberMethod
                {
                    Name = _nameBuilderUtil.OnChildHookMethodName(function),
                    ReturnType = new CodeTypeReference(function.ReturnType),
                    Attributes = MemberAttributes.Abstract | MemberAttributes.Public
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
        CodeTypeDeclaration BuildForCommand(DomainClass generatedClass);
    }
}