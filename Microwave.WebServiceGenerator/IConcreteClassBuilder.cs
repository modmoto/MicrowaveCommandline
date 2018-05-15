﻿using System.CodeDom;

namespace Microwave.WebServiceGenerator
{
    public interface IConcreteClassBuilder
    {
        void AddNameSpace();
        void AddClassType();
        void AddClassProperties();
        void AddConstructor();
        void AddBaseTypes();
        void AddConcreteMethods();
        CodeNamespace Build();
    }
}