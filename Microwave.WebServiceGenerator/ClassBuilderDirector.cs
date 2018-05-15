using System.CodeDom;

namespace Microwave.WebServiceGenerator
{
    public class ClassBuilderDirector
    {
        public CodeNamespace BuildInstance(IConcreteClassBuilder builder)
        {
            builder.AddNameSpace();
            builder.AddClassType();
            builder.AddClassProperties();
            builder.AddConstructor();
            builder.AddBaseTypes();
            builder.AddConcreteMethods();
            return builder.Build();
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