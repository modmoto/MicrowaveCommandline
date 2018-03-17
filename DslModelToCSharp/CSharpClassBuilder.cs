using System.CodeDom;

namespace DslModelToCSharp
{
    public interface ICSharpClassBuilder
    {
        CodeNamespace BuildNameSpace();
        CodeTypeDeclaration BuildClassType();
        void AddClassProperties(CodeTypeDeclaration targetClass);
        void AddConstructor(CodeTypeDeclaration targetClass);
        void AddBaseTypes(CodeTypeDeclaration targetClass);
        void AddConcreteMethods(CodeTypeDeclaration targetClass);
    }
}