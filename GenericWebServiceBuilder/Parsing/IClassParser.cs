using System.CodeDom;
using System.Reflection;
using GenericWebServiceBuilder.DSL;

namespace GenericWebServiceBuilder.Parsing
{
    internal class ClassParser : IClassParser
    {
        public CodeTypeDeclaration Parse(DomainClass userClass)
        {
            var targetClass = new CodeTypeDeclaration(userClass.Name);
            targetClass.IsClass = true;
            targetClass.IsPartial = true;
            targetClass.TypeAttributes = TypeAttributes.Public;
            return targetClass;
        }
    }

    public interface IClassParser
    {
        CodeTypeDeclaration Parse(DomainClass userClass);
    }
}