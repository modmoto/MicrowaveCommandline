//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:6.0.8
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Application
{
    using System;
    using Domain;
    using System.Threading.Tasks;
    using System.Collections.Generic;
    using System.Linq;
    
    
    public class EventStore : IEventStore
    {
        
        public IEventStoreRepository EventStoreRepository { get; }
        
        public IEnumerable<IDomainHook> DomainHooks { get; }
        
        public EventStore(IEventStoreRepository EventStoreRepository, IEnumerable<IDomainHook> DomainHooks)
        {
            this.EventStoreRepository = EventStoreRepository;
            this.DomainHooks = DomainHooks;
        }
        
        public async Task<HookResult> AppendAll(List<DomainEventBase> domainEvents)
        {
            var domainEventsFromHooks = new List<DomainEventBase>();
            var enumerator = domainEvents.GetEnumerator();
            for (
            ; enumerator.MoveNext(); 
            )
            {
                var domainEvent = enumerator.Current;
                var domainHooks = DomainHooks.Where(hook => hook.EventType == domainEvent.GetType());
                var enumeratorHook = domainHooks.GetEnumerator();
                for (
                ; enumeratorHook.MoveNext(); 
                )
                {
                    var domainHook = enumeratorHook.Current;
                    var validationResult = await domainHook.ExecuteSavely(domainEvent);
                    if (!validationResult.Ok)
                    {
                        return validationResult;
                    }
                    domainEventsFromHooks.AddRange(validationResult.DomainEvents);
                }
            }
            await EventStoreRepository.AddEvents(domainEvents);
            await EventStoreRepository.AddEvents(domainEventsFromHooks);
            return HookResult.OkResult();
        }
    }
}
