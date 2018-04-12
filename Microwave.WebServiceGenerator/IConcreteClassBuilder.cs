﻿using System.CodeDom;

namespace Microwave.WebServiceGenerator
{
    public interface IConcreteClassBuilder
    {
        CodeNamespace BuildNameSpace();
        CodeTypeDeclaration BuildClassType();
        void AddClassProperties(CodeTypeDeclaration targetClass);
        void AddConstructor(CodeTypeDeclaration targetClass);
        void AddBaseTypes(CodeTypeDeclaration targetClass);
        void AddConcreteMethods(CodeTypeDeclaration targetClass);
    }
}