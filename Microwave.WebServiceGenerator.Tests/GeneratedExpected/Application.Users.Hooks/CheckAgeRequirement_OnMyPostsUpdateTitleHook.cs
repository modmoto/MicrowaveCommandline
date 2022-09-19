//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:6.0.8
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Application.Users.Hooks
{
    using System;
    using System.Threading.Tasks;
    using Domain;
    using Application.Users;
    using Domain.Posts;
    
    
    public partial class CheckAgeRequirement_OnMyPostsUpdateTitleHook : IDomainHook
    {
        
        public IUserRepository UserRepository { get; private set; }
        
        public Type EventType { get; private set; } = typeof(PostUpdateTitleEvent);
        
        public CheckAgeRequirement_OnMyPostsUpdateTitleHook(IUserRepository UserRepository)
        {
            this.UserRepository = UserRepository;
        }
        
        public async Task<HookResult> ExecuteSavely(DomainEventBase domainEvent)
        {
            if (domainEvent is PostUpdateTitleEvent parsedEvent)
            {
                var parent = await UserRepository.GetMyPostsParent(parsedEvent.EntityId);
                var domainResult = parent.CheckAgeRequirement_OnMyPostsUpdateTitle(parsedEvent);
                if (domainResult.Ok)
                {
                    await UserRepository.UpdateUser(parent);
                    return HookResult.OkResult(domainResult.DomainEvents);
                }
            }
            throw new Exception("Event is not in the correct list");
        }
    }
}
