using System.CodeDom;

namespace DslModelToCSharp.Domain
{
    public class ClassFactory
    {
        public CodeNamespace BuildInstance(CSharpClassBuilder builder)
        {
            var nameSpace = builder.BuildNameSpace();
            var targetClass = builder.BuildClassType();
            nameSpace.Types.Add(targetClass);
            builder.AddClassProperties(targetClass);
            builder.AddConstructor(targetClass);
            return nameSpace;
        }
    }
}