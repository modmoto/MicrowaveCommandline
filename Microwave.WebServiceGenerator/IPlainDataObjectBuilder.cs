using System.CodeDom;

namespace Microwave.WebServiceGenerator
{
    public interface IPlainDataObjectBuilder
    {
        void AddNameSpace();
        void AddClassType();
        void AddClassProperties();
        void AddConstructor();
        void AddBaseTypes();
        CodeNamespace Build();
    }
}