﻿using System.CodeDom;
using Microwave.LanguageModel.Application;
using Microwave.ServiceParser.Util;

namespace Microwave.ServiceParser.Application
{
    public class HookBaseClassBuilder
    {
        private readonly string _nameSpace;
        private readonly NameSpaceBuilderUtil _nameSpaceBuilderUtil;
        private readonly PropertyBuilderUtil _propertyBuilderUtil;
        private InterfaceBuilderUtil _interfaceBuilderUtil;

        public HookBaseClassBuilder(string nameSpace)
        {
            _nameSpace = nameSpace;
            _interfaceBuilderUtil = new InterfaceBuilderUtil();
            _propertyBuilderUtil = new PropertyBuilderUtil();
            _nameSpaceBuilderUtil = new NameSpaceBuilderUtil();
        }

        public CodeNamespace Build(DomainHookBaseClass hookClass)
        {
            var targetClass = _interfaceBuilderUtil.Build(hookClass);
            var nameSpace = _nameSpaceBuilderUtil.WithName(_nameSpace).WithDomain().Build();
            _propertyBuilderUtil.BuildForInterface(targetClass, hookClass.Properties);
            nameSpace.Types.Add(targetClass);
            return nameSpace;
        }
    }
}