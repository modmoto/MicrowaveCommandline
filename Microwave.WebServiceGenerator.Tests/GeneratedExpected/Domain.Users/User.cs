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
    using System.Collections.Generic;
    using Domain.Posts;
    
    
    public partial class User
    {
        
        public static CreationResult<User> Create(UserCreateCommand command)
        {
            // TODO: Implement this method;
            var newGuid = Guid.NewGuid();
            var entity = new User(newGuid, command);
            return CreationResult<User>.OkResult(new List<DomainEventBase> { new UserCreateEvent(entity, newGuid) }, entity);
        }
        
        public override ValidationResult UpdateAge(UserUpdateAgeCommand command)
        {
            // TODO: Implement this method;
            return ValidationResult.ErrorResult(new List<string>{"The Method \"UpdateAge\" in Class \"User\" that is not implemented was called, aborting..."});
        }
        
        public override ValidationResult UpdateName(UserUpdateNameCommand command)
        {
            // TODO: Implement this method;
            return ValidationResult.ErrorResult(new List<string>{"The Method \"UpdateName\" in Class \"User\" that is not implemented was called, aborting..."});
        }
        
        public override ValidationResult AddPost(UserAddPostCommand command)
        {
            // TODO: Implement this method;
            return ValidationResult.ErrorResult(new List<string>{"The Method \"AddPost\" in Class \"User\" that is not implemented was called, aborting..."});
        }
        
        public override ValidationResult CheckAgeRequirement_OnMyPostsUpdateTitle(PostUpdateTitleEvent hookEvent)
        {
            // TODO: Implement this method;
            return ValidationResult.ErrorResult(new List<string>{"The Method \"CheckAgeRequirement_OnMyPostsUpdateTitle\" in Class \"User\" that is not implemented was called, aborting..."});
        }
        
        public override ValidationResult CheckAgeRequirement_OnPinnedPostUpdateTitle(PostUpdateTitleEvent hookEvent)
        {
            // TODO: Implement this method;
            return ValidationResult.ErrorResult(new List<string>{"The Method \"CheckAgeRequirement_OnPinnedPostUpdateTitle\" in Class \"User\" that is not implemented was called, aborting..."});
        }
    }
}
