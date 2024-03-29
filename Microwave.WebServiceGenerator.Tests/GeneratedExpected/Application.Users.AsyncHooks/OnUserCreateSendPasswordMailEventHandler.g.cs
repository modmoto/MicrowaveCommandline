//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:6.0.8
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Application.Users.AsyncHooks
{
    using System;
    using Application;
    using System.Threading.Tasks;
    using System.Collections.Generic;
    using Domain.Users;
    
    
    public class OnUserCreateSendPasswordMailEventHandler
    {
        
        public OnUserCreateSendPasswordMailAsyncHook AsyncHook { get; private set; }
        
        public IHangfireQueue HangfireQueue { get; private set; }
        
        public IUserRepository UserRepository { get; private set; }
        
        public OnUserCreateSendPasswordMailEventHandler(OnUserCreateSendPasswordMailAsyncHook AsyncHook, IHangfireQueue HangfireQueue, IUserRepository UserRepository)
        {
            this.AsyncHook = AsyncHook;
            this.HangfireQueue = HangfireQueue;
            this.UserRepository = UserRepository;
        }
        
        public async Task Run()
        {
            var events = await HangfireQueue.GetEvents("SendPasswordMail");
            var handledEvents = new List<EventAndJob>();
            var enumerator = events.GetEnumerator();
            for (
            ; enumerator.MoveNext(); 
            )
            {
                var eventWrapper = enumerator.Current;
                var domainEvent = (UserCreateEvent) eventWrapper.DomainEvent;
                var entity = await UserRepository.GetUser(domainEvent.Id);
                var newCreateEvent = new UserCreateEvent(entity, domainEvent.EntityId);
                var hookResult = await AsyncHook.Execute(newCreateEvent);
                if (hookResult.Ok)
                {
                    handledEvents.Add(eventWrapper);
                }
            }
            await HangfireQueue.RemoveEventsFromQueue(handledEvents);
        }
    }
}
