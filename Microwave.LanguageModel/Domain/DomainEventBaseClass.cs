﻿using System.Collections.Generic;

namespace Microwave.LanguageModel.Domain
{
    public class DomainEventBaseClass
    {
        public string Name => "DomainEventBase";
        public IList<Property> Properties => new List<Property>
        {
            new Property { Name = "Id", Type = "Guid" },
            new Property { Name = "EntityId", Type = "Guid" }
        };
    }
}