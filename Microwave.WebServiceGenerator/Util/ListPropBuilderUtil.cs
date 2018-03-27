using System.CodeDom;
using System.Collections.Generic;
using Microwave.LanguageModel.Domain;

namespace Microwave.WebServiceGenerator.Util
{
    public class ListPropBuilderUtil
    {
        public CodeTypeDeclaration Build(CodeTypeDeclaration generatedClass, IList<ListProperty> properties)
        {
            foreach (var property in properties)
            {
                var field = new CodeMemberField
                {
                    Name = $"{property.Name} {{ get; private set; }} = new List<{property.Type}>();NewHackGuid302315ed-3a05-4992-9f76-4cf075cde515",
                    Attributes = MemberAttributes.Public | MemberAttributes.Final,
                    Type = new CodeTypeReference($"List<{property.Type}>")
                };

                generatedClass.Members.Add(field);
            }

            return generatedClass;
        }
    }
}