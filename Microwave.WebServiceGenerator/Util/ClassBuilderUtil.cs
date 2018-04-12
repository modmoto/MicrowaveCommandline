using System.CodeDom;
using System.Reflection;

namespace Microwave.WebServiceGenerator.Util
{
    public class ClassBuilderUtil
    {
        public CodeTypeDeclaration BuildPartial(string name)
        {
            var targetClass = Build(name);
            targetClass.IsPartial = true;
            return targetClass;
        }

        public CodeTypeDeclaration Build(string name)
        {
            var targetClass = new CodeTypeDeclaration(name);
            targetClass.IsClass = true;
            targetClass.TypeAttributes = TypeAttributes.Public;
            return targetClass;
        }

        public CodeTypeDeclaration BuildAsInterface(string name)
        {
            var iface = new CodeTypeDeclaration($"I{name}") {IsInterface = true};
            return iface;
        }
    }
}