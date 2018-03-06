using System.CodeDom;
using DslModel;

namespace DslModelToCSharp
{
    public class PropBuilder : IPropertyBuilder
    {
        public AutoProperty Build(Property property)
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
                Type = new CodeTypeReference(property.Type)
            };
            csProperty.GetStatements.Add(new CodeMethodReturnStatement(
                new CodeFieldReferenceExpression(
                    new CodeThisReferenceExpression(), $"_{property.Name}")));

            return new AutoProperty(field, csProperty);
        }
    }

    public interface IPropertyBuilder
    {
        AutoProperty Build(Property property);
    }
}