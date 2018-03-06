using System.CodeDom;
using System.Reflection;
using DslModel;

namespace DslModelToCSharp
{
    public class ClassBuilder : IClassParser
    {
        public CodeTypeDeclaration Build(DomainClass userClass)
        {
            var targetClass = new CodeTypeDeclaration(userClass.Name);
            targetClass.IsClass = true;
            targetClass.IsPartial = true;
            targetClass.TypeAttributes = TypeAttributes.Public;
            return targetClass;
        }

        public CodeTypeDeclaration Build(DomainEvent domainEvent)
        {
            var targetClass = new CodeTypeDeclaration(domainEvent.Name);
            targetClass.IsClass = true;
            targetClass.TypeAttributes = TypeAttributes.Public;
            return targetClass;
        }
    }

    public interface IClassParser
    {
        CodeTypeDeclaration Build(DomainClass userClass);
        CodeTypeDeclaration Build(DomainEvent domainEvent);
    }
}