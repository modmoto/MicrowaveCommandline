using System.CodeDom;
using GenericWebServiceBuilder.DomainSpecificGrammar;

namespace GenericWebServiceBuilder.DomainToCSharp
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
                Type = new CodeTypeReference(property.Type)
            };
            csProperty.GetStatements.Add(new CodeMethodReturnStatement(
                new CodeFieldReferenceExpression(
                    new CodeThisReferenceExpression(), $"_{property.Name}")));

            return new AutoProperty(field, csProperty);
        }
    }

    public interface IPropertyParser
    {
        AutoProperty Parse(Property property);
    }
}