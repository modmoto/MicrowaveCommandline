using DslModel.Domain;

namespace DslModelToCSharp.Application
{
    public class NameBuilder
    {
        public string BuildCommandHandlerName(DomainClass domainClass)
        {
            return $"{domainClass.Name}CommandHandler";
        }

        public string CreateCommandName(DomainClass domainClass, CreateMethod createMethod)
        {
            return $"{domainClass.Name}{createMethod.Name}Command";
        }

        public string UpdateCommandName(DomainClass domainClass, DomainMethod method)
        {
            return $"{domainClass.Name}{method.Name}Command";
        }
    }
}