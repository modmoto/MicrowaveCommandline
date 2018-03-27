using Microwave.LanguageModel.Domain;

namespace Microwave.ServiceParser.Util
{
    public class NameBuilderUtil
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

        public string EventName(DomainClass domainClass, DomainMethod method)
        {
            return $"{domainClass.Name}{method.Name}Event";
        }

        public string EventName(DomainClass domainClass, CreateMethod method)
        {
            return $"{domainClass.Name}{method.Name}Event";
        }

        public string UpdateApiCommandName(DomainClass domainClass, DomainMethod method)
        {
            return $"{domainClass.Name}{method.Name}ApiCommand";
        }

        public string BuildErrorMessageFor(Parameter loadParam)
        {
            return $"$\"Could not find {loadParam.Type} for {{nameof(apiCommand.{loadParam.Name}Id)}} with ID: {{id}}\"";
        }
    }
}