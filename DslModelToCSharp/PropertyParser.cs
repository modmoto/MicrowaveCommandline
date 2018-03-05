using System.CodeDom;
using DslModel;

namespace DslModelToCSharp
{
    public class PropertyParser : IPropertyParser
    {
        public AutoProperty Parse(Property property)
        {
            var field = new CodeMemberField
            {
                Attributes = MemberAttributes.Private,
                Name = $"_{property.Name}",
                Type = new CodeTypeReference(property.Type)
            };

            var csProperty = new CodeMemberProperty
            {
                Attributes = MemberAttributes.Public | MemberAttributes.Final,
                Name = property.Name,
                HasGet = true,
                HasSet = true,
                Type = new CodeTypeReference(property.Type)
            };
            csProperty.GetStatements.Add(new CodeMethodReturnStatement(
                new CodeFieldReferenceExpression(
                    new CodeThisReferenceExpression(), $"_{property.Name}")));
            CodeAssignStatement setStatement = new CodeAssignStatement
            {
                Left = new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), property.Name),
                Right = new CodeFieldReferenceExpression(null, property.Name)
            };
            csProperty.SetStatements.Add(setStatement);

            return new AutoProperty(field, csProperty);
        }
    }

    public interface IPropertyParser
    {
        AutoProperty Parse(Property property);
    }
}