using System.CodeDom;
using Microwave.LanguageModel.Application;
using Microwave.WebServiceGenerator.Util;

namespace Microwave.WebServiceGenerator.Application
{
    public class EventStoreRepositoryInterfaceBuilder
    {
        private string _nameSpace;
        private InterfaceBuilderUtil _interfaceBuilderUtil;
        private NameSpaceBuilderUtil _nameSpaceBuilderUtil;

        public EventStoreRepositoryInterfaceBuilder(string nameSpace)
        {
            _nameSpace = nameSpace;
            _interfaceBuilderUtil = new InterfaceBuilderUtil();
            _nameSpaceBuilderUtil = new NameSpaceBuilderUtil();
        }

        public CodeNamespace Build(EventStoreRepositoryInterface hookClass)
        {
            var targetClass = _interfaceBuilderUtil.Build(hookClass);
            var nameSpace = _nameSpaceBuilderUtil.WithName(_nameSpace).WithDomain().WithTask().WithList().Build();
            nameSpace.Types.Add(targetClass);
            return nameSpace;
        }
    }
}