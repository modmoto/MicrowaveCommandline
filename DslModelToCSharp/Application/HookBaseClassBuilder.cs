using System.CodeDom;
using DslModel.Application;

namespace DslModelToCSharp.Application
{
    public class HookBaseClassBuilder
    {
        private readonly string _nameSpace;
        private readonly NameSpaceBuilder _nameSpaceBuilder;
        private readonly PropBuilder _propertyBuilder;
        private InterfaceBuilder _interfaceBuilder;

        public HookBaseClassBuilder(string nameSpace)
        {
            _nameSpace = nameSpace;
            _interfaceBuilder = new InterfaceBuilder();
            _propertyBuilder = new PropBuilder();
            _nameSpaceBuilder = new NameSpaceBuilder();
        }

        public CodeNamespace Write(DomainHookBaseClass hookClass)
        {
            var targetClass = _interfaceBuilder.Build(hookClass);

            var nameSpace = _nameSpaceBuilder.BuildWithDomainImport(_nameSpace);

            _propertyBuilder.BuildForInterface(targetClass, hookClass.Properties);


            nameSpace.Types.Add(targetClass);

            return nameSpace;
        }
    }
}