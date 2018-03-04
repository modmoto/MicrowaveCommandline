using System.CodeDom;
using System.Reflection;
using DslModel;

namespace DslModelToCSharp
{
    public class ClassParser : IClassParser
    {
        public CodeTypeDeclaration Parse(DomainClass userClass)
        {
            var targetClass = new CodeTypeDeclaration(userClass.Name);
            targetClass.IsClass = true;
            targetClass.IsPartial = true;
            targetClass.TypeAttributes = TypeAttributes.Public;
            return targetClass;
        }

        public CodeTypeDeclaration Parse(DomainEvent domainEvent)
        {
            var targetClass = new CodeTypeDeclaration(domainEvent.Name);
            targetClass.IsClass = true;
            targetClass.TypeAttributes = TypeAttributes.Public;
            return targetClass;
        }
    }

    public interface IClassParser
    {
        CodeTypeDeclaration Parse(DomainClass userClass);
        CodeTypeDeclaration Parse(DomainEvent domainEvent);
    }
}