using System.CodeDom;
using System.Collections.Generic;
using DslModel;

namespace DslModelToCSharp
{
    public class PropBuilder : IPropertyBuilder
    {
        public CodeTypeDeclaration Build(CodeTypeDeclaration generatedClass, IList<Property> properties)
        {
            if (generatedClass.Name.StartsWith("Create"))
            {
                foreach (var property in properties)
                {
                    var field = new CodeMemberField
                    {
                        Name = property.Name + " { get; }NewHackGuid302315ed-3a05-4992-9f76-4cf075cde515",
                        Attributes = MemberAttributes.Public | MemberAttributes.Final,
                        Type = new CodeTypeReference(property.Type)
                    };

                    generatedClass.Members.Add(field);
                }
            }
            else
            {
                foreach (var property in properties)
                {
                    var field = new CodeMemberField
                    {
                        Name = property.Name + " { get; private set; }NewHackGuid302315ed-3a05-4992-9f76-4cf075cde515",
                        Attributes = MemberAttributes.Public | MemberAttributes.Final,
                        Type = new CodeTypeReference(property.Type)
                    };

                    generatedClass.Members.Add(field);
                }
            }

            return generatedClass;
        }
    }

    public interface IPropertyBuilder
    {
        CodeTypeDeclaration Build(CodeTypeDeclaration generatedClass, IList<Property> properties);
    }
}