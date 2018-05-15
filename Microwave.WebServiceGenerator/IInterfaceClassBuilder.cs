using System.CodeDom;

namespace Microwave.WebServiceGenerator
{
    public interface IInterfaceClassBuilder
    {
        CodeNamespace BuildNameSpace();
        CodeTypeDeclaration BuildClassType();
        void AddBaseTypes(CodeTypeDeclaration targetClass);
        void AddAbstractMethods(CodeTypeDeclaration targetClass);
    }
}