using System.CodeDom;

namespace DslModelToCSharp
{
    public class ClassFactory
    {
        public CodeNamespace BuildInstance(ICSharpClassBuilder builder)
        {
            var nameSpace = builder.BuildNameSpace();
            var targetClass = builder.BuildClassType();
            nameSpace.Types.Add(targetClass);
            builder.AddClassProperties(targetClass);
            builder.AddConstructor(targetClass);
            builder.AddBaseTypes(targetClass);
            return nameSpace;
        }
    }
}