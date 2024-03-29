//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:6.0.8
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Domain
{
    using System;
    using System.Collections.Generic;
    
    
    public class ValidationResult
    {
        
        public Boolean Ok { get; private set; }
        
        public List<DomainEventBase> DomainEvents { get; private set; }
        
        public List<string> DomainErrors { get; private set; }
        
        private ValidationResult(Boolean Ok, List<DomainEventBase> DomainEvents, List<string> DomainErrors)
        {
            this.Ok = Ok;
            this.DomainEvents = DomainEvents;
            this.DomainErrors = DomainErrors;
        }
        
        public static ValidationResult OkResult(List<DomainEventBase> DomainEvents)
        {
            return new ValidationResult(true, DomainEvents, new List<string>());
        }
        
        public static ValidationResult ErrorResult(List<string> DomainErrors)
        {
            return new ValidationResult(false, new List<DomainEventBase>(), DomainErrors);
        }
    }
}
