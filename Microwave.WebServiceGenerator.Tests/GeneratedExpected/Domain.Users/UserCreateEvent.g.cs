//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:6.0.8
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Domain.Users
{
    using System;
    
    
    public class UserCreateEvent : DomainEventBase
    {
        
        public User User { get; }
        
        private UserCreateEvent() : 
                base(Guid.Empty)
        {
        }
        
        public UserCreateEvent(User User, Guid EntityId) : 
                base(EntityId)
        {
            this.User = User;
        }
    }
}
