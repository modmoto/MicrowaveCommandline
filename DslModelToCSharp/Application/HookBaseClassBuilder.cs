using System.CodeDom;
using DslModel.Application;
using DslModelToCSharp.Domain;
using DslModelToCSharp.Util;

namespace DslModelToCSharp.Application
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
            var nameSpace = _nameSpaceBuilderUtil.BuildWithDomainImport(_nameSpace);
            _propertyBuilderUtil.BuildForInterface(targetClass, hookClass.Properties);
            nameSpace.Types.Add(targetClass);
            return nameSpace;
        }
    }
}