//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Application.Users.Hooks
{
    using System;
    using System.Threading.Tasks;
    using System.Collections.Generic;
    using Domain.Users;
    using Domain;
    
    
    public partial class SendPasswordMailHook
    {
        
        public async Task<HookResult> Execute(UserCreateEvent domainEvent)
        {
            return await Task.FromResult(HookResult.ErrorResult(new List<string>{"A generated Synchronouse Doman Hook Method that is not implemented was called, aborting..."}));
        }
    }
}
