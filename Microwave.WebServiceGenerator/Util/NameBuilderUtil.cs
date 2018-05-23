using System.Collections.Generic;
using System.Linq;
using Microwave.LanguageModel;

namespace Microwave.WebServiceGenerator.Util
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

        public string ErrorMessageFor(Parameter loadParam)
        {
            return $"$\"Could not find {loadParam.Type} for {{nameof(apiCommand.{loadParam.Name}Id)}} with ID: {{apiCommand.{loadParam.Name}Id}}\"";
        }

        public string AsyncEventHookName(AsyncDomainHook hook)
        {
            return $"On{hook.ClassType}{hook.MethodName}{hook.Name}AsyncHook";
        }

        public string AsyncEventHookHandlerName(AsyncDomainHook hook)
        {
            return $"On{hook.ClassType}{hook.MethodName}{hook.Name}EventHandler";
        }

        public string HooKEventName(AsyncDomainHook domainHook)
        {
            return $"{domainHook.ClassType}{domainHook.MethodName}Event";
        }

        public string OnChildHookMethodName(OnChildHookMethod hook)
        {
            return $"{hook.Name}_On{hook.OriginFieldName}{hook.MethodName}";
        }

        public string GetClassName(OnChildHookMethod onChildHook, List<Property> classListProperties, List<ListProperty> listProperties)
        {
            return classListProperties.FirstOrDefault(prop => prop.Name == onChildHook.OriginFieldName)?.Type ?? listProperties.FirstOrDefault(prop => prop.Name == onChildHook.OriginFieldName)?.Type;
        }
    }
}