//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:6.0.8
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Domain.Posts
{
    using System;
    
    
    public class PostUpdateTitleEvent : DomainEventBase
    {
        
        public String Title { get; private set; }
        
        private PostUpdateTitleEvent() : 
                base(Guid.Empty)
        {
        }
        
        public PostUpdateTitleEvent(String Title, Guid EntityId) : 
                base(EntityId)
        {
            this.Title = Title;
        }
    }
}
