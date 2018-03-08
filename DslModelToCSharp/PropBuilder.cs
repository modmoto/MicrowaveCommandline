using System.CodeDom;
using System.Collections.Generic;
using DslModel;

namespace DslModelToCSharp
{
    public class PropBuilder : IPropertyBuilder
    {
        public CodeTypeDeclaration Build(CodeTypeDeclaration generatedClass, IList<Property> properties)
        {
            foreach (var property in properties)
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
                generatedClass.Members.Add(field);
                generatedClass.Members.Add(csProperty);
            }

            return generatedClass;
        }
    }

    public interface IPropertyBuilder
    {
        CodeTypeDeclaration Build(CodeTypeDeclaration generatedClass, IList<Property> properties);
    }
}