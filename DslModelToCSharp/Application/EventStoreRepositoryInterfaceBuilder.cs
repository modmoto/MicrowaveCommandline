using System.CodeDom;
using DslModel.Application;
using DslModelToCSharp.Domain;

namespace DslModelToCSharp.Application
{
    public class EventStoreRepositoryInterfaceBuilder
    {
        private string _nameSpace;
        private InterfaceBuilder _interfaceBuilder;
        private NameSpaceBuilder _nameSpaceBuilder;

        public EventStoreRepositoryInterfaceBuilder(string nameSpace)
        {
            _nameSpace = nameSpace;
            _interfaceBuilder = new InterfaceBuilder();
            _nameSpaceBuilder = new NameSpaceBuilder();
        }

        public CodeNamespace Build(EventStoreRepositoryInterface hookClass)
        {
            var targetClass = _interfaceBuilder.Build(hookClass);
            var nameSpace = _nameSpaceBuilder.BuildWithTask(_nameSpace);
            nameSpace.Types.Add(targetClass);
            return nameSpace;
        }
    }
}