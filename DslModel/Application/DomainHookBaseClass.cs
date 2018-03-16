﻿using System.Collections.Generic;
using DslModel.Domain;

namespace DslModel.Application
{
    public class DomainHookBaseClass : DomainClass
    {
        public DomainHookBaseClass()
        {
            Name = "IDomainHook";
            Properties = new List<Property> {new Property {Name = "EventType", Type = "Type"}};
            Methods = new List<DomainMethod>
            {
                new DomainMethod
                {
                    Name = "ExecuteSave",
                    ReturnType = "HookResult",
                    Parameters = {new Parameter {Name = "domainEvent", Type = "DomainEventBase"}}
                }
            };
        }
    }
}