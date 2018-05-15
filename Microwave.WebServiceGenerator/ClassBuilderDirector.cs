using System.CodeDom;

namespace Microwave.WebServiceGenerator
{
    public class ClassBuilderDirector
    {
        public CodeNamespace BuildInstance(IConcreteClassBuilder builder)
        {
            var nameSpace = builder.BuildNameSpace();
            var targetClass = builder.BuildClassType();
            nameSpace.Types.Add(targetClass);
            builder.AddClassProperties(targetClass);
            builder.AddConstructor(targetClass);
            builder.AddBaseTypes(targetClass);
            builder.AddConcreteMethods(targetClass);
            return nameSpace;
        }

        public CodeNamespace BuildInstance(IPlainDataObjectBuilder builder)
        {
            builder.AddNameSpace();
            builder.AddClassType();
            builder.AddClassProperties();
            builder.AddConstructor();
            builder.AddBaseTypes();
            return builder.Build();
        }
    }
}