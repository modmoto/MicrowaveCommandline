//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Domain.Users
{
    using System;
    
    
    public class UserUpdateAgeEvent : IDomainEvent
    {
        
        private Guid _UserId;
        
        private Int32 _Age;
        
        private UserUpdateAgeEvent()
        {
        }
        
        public UserUpdateAgeEvent(Guid UserId, Int32 Age)
        {
            this._UserId = UserId;
            this._Age = Age;
        }
        
        public Guid UserId
        {
            get
            {
                return this._UserId;
            }
        }
        
        public Int32 Age
        {
            get
            {
                return this._Age;
            }
        }
    }
}
