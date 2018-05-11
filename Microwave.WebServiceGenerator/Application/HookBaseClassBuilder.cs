using System.CodeDom;
using Microwave.WebServiceGenerator.Util;
using Microwave.WebServiceModel.Application;

namespace Microwave.WebServiceGenerator.Application
{
    public class HookBaseClassBuilder
    {
        private readonly string _nameSpace;
        private readonly NameSpaceBuilderUtil _nameSpaceBuilderUtil;
        private readonly PropertyBuilderUtil _propertyBuilderUtil;
        private readonly InterfaceBuilderUtil _interfaceBuilderUtil;

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
            var nameSpace = _nameSpaceBuilderUtil.WithName(_nameSpace).WithDomain().WithTask().Build();
            _propertyBuilderUtil.BuildForInterface(targetClass, hookClass.Properties);
            nameSpace.Types.Add(targetClass);
            return nameSpace;
        }
    }
}