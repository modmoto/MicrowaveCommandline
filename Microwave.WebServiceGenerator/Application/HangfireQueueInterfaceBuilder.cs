using System.CodeDom;
using Microwave.WebServiceGenerator.Util;
using Microwave.WebServiceModel.Application;

namespace Microwave.WebServiceGenerator.Application
{
    public class HangfireQueueInterfaceBuilder
    {
        private readonly string _nameSpace;
        private readonly NameSpaceBuilderUtil _nameSpaceBuilderUtil;
        private readonly InterfaceBuilderUtil _interfaceBuilderUtil;

        public HangfireQueueInterfaceBuilder(string nameSpace)
        {
            _nameSpace = nameSpace;
            _interfaceBuilderUtil = new InterfaceBuilderUtil();
            _nameSpaceBuilderUtil = new NameSpaceBuilderUtil();
        }

        public CodeNamespace Build(HangfireQueueInterface eventStore)
        {
            var codeNamespace = _nameSpaceBuilderUtil.WithName(_nameSpace).WithDomain().WithTask().WithList().Build();
            var codeTypeDeclaration = _interfaceBuilderUtil.Build(eventStore);
            codeNamespace.Types.Add(codeTypeDeclaration);
            return codeNamespace;
        }
    }
}