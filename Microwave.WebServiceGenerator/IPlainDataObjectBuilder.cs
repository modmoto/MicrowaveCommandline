﻿using System.CodeDom;

namespace Microwave.WebServiceGenerator
{
    public interface IPlainDataObjectBuilder
    {
        CodeNamespace BuildNameSpace();
        CodeTypeDeclaration BuildClassType();
        void AddClassProperties(CodeTypeDeclaration targetClass);
        void AddConstructor(CodeTypeDeclaration targetClass);
        void AddBaseTypes(CodeTypeDeclaration targetClass);
    }
}