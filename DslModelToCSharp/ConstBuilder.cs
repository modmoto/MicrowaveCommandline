using System.CodeDom;
using System.Collections.Generic;
using DslModel;

namespace DslModelToCSharp
{
    public interface IConstBuilder
    {
        CodeConstructor BuildPrivate(IList<Property> userClassProperties);
        CodeConstructor BuildPublic(IList<Property> userClassProperties);
    }

    public class ConstBuilder : IConstBuilder
    {
        public CodeConstructor BuildPrivate(IList<Property> proterties)
        {
            var constructor = new CodeConstructor();
            foreach (var proptery in proterties)
            {
                constructor.Parameters.Add(new CodeParameterDeclarationExpression(proptery.Type, proptery.Name));
                CodeAssignStatement body = new CodeAssignStatement
                {
                    Left = new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), $"_{proptery.Name}"),
                    Right = new CodeFieldReferenceExpression(null, proptery.Name)
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
    }
}