using System.CodeDom;

namespace DslModelToCSharp.Domain
{
    public abstract class CSharpClassBuilder
    {
        public abstract CodeNamespace BuildNameSpace();
        public abstract CodeTypeDeclaration BuildClassType();
        public abstract void AddClassProperties(CodeTypeDeclaration targetClass);
        public abstract void AddConstructor(CodeTypeDeclaration targetClass);
        public abstract void AddBaseTypes(CodeTypeDeclaration targetClass);
    }
}