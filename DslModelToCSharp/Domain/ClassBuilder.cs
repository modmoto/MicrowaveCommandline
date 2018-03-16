using System.CodeDom;
using System.Reflection;

namespace DslModelToCSharp.Domain
{
    public class ClassBuilder : IClassBuilder
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
    }

    public interface IClassBuilder
    {
        CodeTypeDeclaration Build(string name);
        CodeTypeDeclaration BuildPartial(string name);
    }
}