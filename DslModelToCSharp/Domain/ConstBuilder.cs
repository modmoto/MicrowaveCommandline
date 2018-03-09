using System.CodeDom;
using System.Collections.Generic;
using DslModel.Domain;

namespace DslModelToCSharp
{
    public interface IConstBuilder
    {
        CodeConstructor BuildPrivate(IList<Property> userClassProperties);
        CodeConstructor BuildPublic(IList<Property> userClassProperties);
        CodeConstructor BuildPublicWithBaseCall(IList<Property> domainEventProperties, IList<Property> properties);
        CodeConstructor BuildPublicWithIdCreateInBody(IList<Property> userClassProperties, string IdName);
    }

    public class ConstBuilder : IConstBuilder
    {
        public CodeConstructor BuildPrivate(IList<Property> proterties)
        {
            var constructor = new CodeConstructor();
            foreach (var property in proterties)
            {
                constructor.Parameters.Add(new CodeParameterDeclarationExpression(property.Type, property.Name));
                CodeAssignStatement body = new CodeAssignStatement
                {
                    Left = new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), property.Name),
                    Right = new CodeFieldReferenceExpression(null, property.Name)
                };
                constructor.Statements.Add(body);
            }

            return constructor;
        }

        public CodeConstructor BuildPublic(IList<Property> userClassProperties)
        {
            var codeConstructor = BuildPrivate(userClassProperties);
            codeConstructor.Attributes = MemberAttributes.Public | MemberAttributes.Final;
            return codeConstructor;
        }

        public CodeConstructor BuildPublicWithIdCreateInBody(IList<Property> userClassProperties, string idName)
        {
            var codeConstructor = BuildPublic(userClassProperties);
            codeConstructor.Statements.Add(new CodeAssignStatement(new CodeVariableReferenceExpression("this." + idName), new CodeArgumentReferenceExpression("Guid.NewGuid()")));
            return codeConstructor;
        }

        public CodeConstructor BuildPublicWithBaseCall(IList<Property> domainEventProperties, IList<Property> properties)
        {
            var constructor = BuildPublic(domainEventProperties);
            foreach (var property in properties)
            {
                constructor.Parameters.Add(new CodeParameterDeclarationExpression(property.Type, property.Name));
                constructor.BaseConstructorArgs.Add(new CodeArgumentReferenceExpression(property.Name));
            }

            return constructor;
        }
    }
}