using System.CodeDom;

namespace GenericWebServiceBuilder.DslModelToCSharp
{
    public class AutoProperty
    {
        public AutoProperty(CodeMemberField field, CodeMemberProperty property)
        {
            Field = field;
            Property = property;
        }

        public CodeMemberField Field { get; }
        public CodeMemberProperty Property { get; }
    }
}