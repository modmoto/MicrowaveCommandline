//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
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
        
        private Boolean _Ok;
        
        private IList<DomainEventBase> _DomainEvents;
        
        private IList<string> _DomainErrors;
        
        private ValidationResult(Boolean Ok, IList<DomainEventBase> DomainEvents, IList<string> DomainErrors)
        {
            this._Ok = Ok;
            this._DomainEvents = DomainEvents;
            this._DomainErrors = DomainErrors;
        }
        
        public Boolean Ok
        {
            get
            {
                return this._Ok;
            }
        }
        
        public IList<DomainEventBase> DomainEvents
        {
            get
            {
                return this._DomainEvents;
            }
        }
        
        public IList<string> DomainErrors
        {
            get
            {
                return this._DomainErrors;
            }
        }
    }
}