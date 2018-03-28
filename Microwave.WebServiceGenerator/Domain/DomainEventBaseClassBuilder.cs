﻿using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using Microwave.LanguageModel.Domain;
using Microwave.WebServiceGenerator.Util;
using Microwave.WebServiceModel.Domain;

namespace Microwave.WebServiceGenerator.Domain
{
    public class DomainEventBaseClassBuilder : ICSharpClassBuilder
    {
        private readonly ConstructorBuilderUtil _constructorBuilderUtil;
        private readonly NameSpaceBuilderUtil _nameSpaceBuilderUtil;
        private readonly IClassBuilder _classBuilder;
        private readonly PropertyBuilderUtil _propertyBuilderUtil;

        public DomainEventBaseClassBuilder()
        {
            _propertyBuilderUtil = new PropertyBuilderUtil();
            _constructorBuilderUtil = new ConstructorBuilderUtil();
            _nameSpaceBuilderUtil = new NameSpaceBuilderUtil();
            _classBuilder = new ClassBuilderUtil();
        }


        public CodeNamespace BuildNameSpace()
        {
            return _nameSpaceBuilderUtil.WithName("Domain").WithList().WithStopWatch().Build();
        }

        public CodeTypeDeclaration BuildClassType()
        {
            return _classBuilder.Build(new DomainEventBaseClass().Name);
        }

        public void AddClassProperties(CodeTypeDeclaration targetClass)
        {
            _propertyBuilderUtil.Build(targetClass, new DomainEventBaseClass().Properties);
        }

        public void AddConstructor(CodeTypeDeclaration targetClass)
        {
            var properties = new DomainEventBaseClass().Properties;
            var constructor = _constructorBuilderUtil.BuildPublicWithIdCreateInBody(new List<Property> { properties[1] }, properties[0].Name, properties[2].Name);
            targetClass.Members.Add(constructor);
        }

        public void AddBaseTypes(CodeTypeDeclaration targetClass)
        {
        }

        public void AddConcreteMethods(CodeTypeDeclaration targetClass)
        {
        }
    }
}